using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class RateGuestsViewModel : ViewModelBase
	{
		public static AccommodationReservation SelectedReservation { get; set; }

		public static User LogginUser { get; set; }

		private readonly AccommodationReservationService accommodationReservationService;

		private readonly GuestReviewService guestReviewService;

		private readonly OwnerReviewService ownerReviewService;

		public Action CloseAction { get; set; }

		public RateGuestsViewModel(User user, AccommodationReservation reservation)
		{
			SelectedReservation = reservation;
			LogginUser = user;
			accommodationReservationService = new AccommodationReservationService();
			guestReviewService = new GuestReviewService();
			ownerReviewService = new OwnerReviewService();
			InitializeCommands();

		}

		private void InitializeCommands()
		{
			ConfirmGrade = new RelayCommand(Execute_ConfirmGrade, CanExecute_Command);
			CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
		}


		private bool CanExecute_Command(object parameter)
		{
			return true;
		}

        private void Execute_CancelCommand(object sender)
		{
			CloseAction();
		}

		private void Execute_ConfirmGrade(object sender)
		{
			
			GuestReview newReview = new GuestReview(LogginUser.Id, SelectedReservation.Id, int.Parse(CleanlinessGrade), int.Parse(RuleGrade), Comment1,SelectedReservation.IdGuest);
			GuestReview savedReview = guestReviewService.Save(newReview);
			OwnerMainWindowViewModel.FilteredReservations.Remove(SelectedReservation);

			foreach(OwnerReview review in ownerReviewService.GetAll())
			{
				if (savedReview.IdReservation == review.ReservationId)
				{
					OwnerMainWindowViewModel.FilteredReviews.Add(review);
				}
			}
			CloseAction();
		}

		private RelayCommand confirmGrade;
		public RelayCommand ConfirmGrade
		{
			get { return confirmGrade; }
			set
			{
				confirmGrade = value;
			}
		}

		private RelayCommand cancelCommand;
		public RelayCommand CancelCommand
		{
			get { return cancelCommand; }
			set
			{
				cancelCommand = value;
			}
		}

		private string _cleanlinessGrade;
		public string CleanlinessGrade
		{
			get => _cleanlinessGrade;
			set
			{
				if (value != _cleanlinessGrade)
				{
					_cleanlinessGrade = value;
					OnPropertyChanged();
				}
			}
		}

		private string _ruleGrade;
		public string RuleGrade
		{
			get => _ruleGrade;
			set
			{
				if (value != _ruleGrade)
				{
					_ruleGrade = value;
					OnPropertyChanged();
				}
			}
		}

		private string _comment;
		public string Comment1
		{
			get => _comment;
			set
			{
				if (value != _comment)
				{
					_comment = value;
					OnPropertyChanged();
				}
			}
		}

	}
}
