using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace InitialProject.WPF.ViewModel
{
	public class ReservationMovingViewModel : ViewModelBase
	{
		public static ObservableCollection<AccommodationReservation> Reservations { get; set; }

		public static ObservableCollection<ReservationDisplacementRequest> Requests { get; set; }


		private readonly AccommodationReservationService accommodationReservationService;


		private readonly ReservationDisplacementRequestService reservationDisplacementRequestService;

		private readonly IMessageBoxService messageBoxService;

		public static User LoggedInUser { get; set; }

		Notifier notifier = new Notifier(cfg =>
		{
			cfg.PositionProvider = new WindowPositionProvider(
				parentWindow: Application.Current.MainWindow,
				corner: Corner.TopRight,
				offsetX: 10,
				offsetY: 10);

			cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
				notificationLifetime: TimeSpan.FromSeconds(3),
				maximumNotificationCount: MaximumNotificationCount.FromCount(5));

			cfg.Dispatcher = Application.Current.Dispatcher;
			cfg.DisplayOptions.Width = 350;


		});


		private ReservationDisplacementRequest _selectedRequest;
		public ReservationDisplacementRequest SelectedRequest
		{
			get { return _selectedRequest; }
			set
			{
				_selectedRequest = value;
				OnPropertyChanged(nameof(SelectedRequest));
			}
		}


		public ReservationMovingViewModel(User user)
		{
			accommodationReservationService = new AccommodationReservationService();
			reservationDisplacementRequestService = new ReservationDisplacementRequestService();
			messageBoxService = new MessageBoxService();
			InitializeProperties(user);
			InitializeCommands();
		}

		public void InitializeProperties(User user)
		{
			LoggedInUser = user;
			Reservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetByOwnerId(LoggedInUser.Id));
			Requests = new ObservableCollection<ReservationDisplacementRequest>(reservationDisplacementRequestService.GetByOwnerId(LoggedInUser.Id));
			foreach (ReservationDisplacementRequest request in reservationDisplacementRequestService.GetByOwnerId(LoggedInUser.Id))
			{
				if (request.Type == RequestType.Rejected || request.Type == RequestType.Approved)
				{
					Requests.Remove(request);
				}
			}

		}

		public void InitializeCommands()
		{
			Check = new RelayCommand(Execute_Check, CanExecute_Command);
			Refuse = new RelayCommand(Execute_Refuse, CanExecute_Command);
			Accept = new RelayCommand(Execute_Accept, CanAccept);
		}

		private bool CanExecute_Command(object parameter)
		{
			return true;
		}

		private bool CanAccept(object obj)
		{
			return true;
		}

		

		private void Execute_Check(object sender)
		{
			var selectedRequest = SelectedRequest;

			reservationDisplacementRequestService.BindPaticularData(SelectedRequest);

			List<AccommodationReservation> reservations = accommodationReservationService.GetByAccommodationId(SelectedRequest.Reservation.IdAccommodation);

			List<AccommodationReservation> overlappingReservations = accommodationReservationService.GetOverlappingReservations(SelectedRequest.Reservation.IdAccommodation, SelectedRequest.NewStartDate, SelectedRequest.NewEndDate, reservations);

			overlappingReservations.Remove(accommodationReservationService.GetById(SelectedRequest.ReservationId));

			var options = new MessageOptions { FontSize = 30 };

			if (overlappingReservations.Count == 0)
			{
				notifier.ShowInformation("The selected date is free, you can move the reservation.", options);
			}
			else
			{
				notifier.ShowInformation("The selected date is not available, you cannot move the reservation.", options);
			}

			
		}



		private void Execute_Refuse(object sender)
		{
			var selectedRequest = SelectedRequest;
			string RejectionReason = ShowMyDialogBox();
			selectedRequest.Comment = RejectionReason;
			selectedRequest.Type = RequestType.Rejected;
			reservationDisplacementRequestService.Update(selectedRequest);
			Requests.Remove(selectedRequest);
		}

		private void Execute_Accept(object sender)
		{
			var selectedRequest = SelectedRequest;
			AccommodationReservation reservation = accommodationReservationService.GetById(selectedRequest.ReservationId);
			reservation.StartDate = selectedRequest.NewStartDate;
			reservation.EndDate = selectedRequest.NewEndDate;
			selectedRequest.Type = RequestType.Approved;
			reservationDisplacementRequestService.Update(selectedRequest);
			accommodationReservationService.Update(reservation);
			Requests.Remove(selectedRequest);

			var options = new MessageOptions { FontSize = 30 };
			notifier.ShowSuccess("You have successfully moved your reservation", options);
			RefreshReservations();

		}

		private void RefreshReservations()
		{
			Reservations.Clear();
			foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetByOwnerId(LoggedInUser.Id)) 
			{
				Reservations.Add(accommodationReservation);
			}
		}

		

		private RelayCommand check;
		public RelayCommand Check
		{
			get { return check; }
			set
			{
				check = value;
			}
		}

		private RelayCommand refuse;
		public RelayCommand Refuse
		{
			get { return refuse; }
			set
			{
				refuse = value;
			}
		}

		private RelayCommand accept;
		public RelayCommand Accept
		{
			get { return accept; }
			set
			{
				accept = value;
			}
		}

		public string ShowMyDialogBox()
		{
			
			MyDialogBoxViewModel viewModel = new MyDialogBoxViewModel();
			MyDialogBox view = new MyDialogBox();
			view.DataContext = viewModel;

			
			bool? dialogResult = view.ShowDialog();

			if (dialogResult == true)
			{
				
				string userInput = viewModel.UserInput;
				return userInput;

				
			}
			return null;
		}
	}
}
