using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class OwnerNotificationsViewModel : ViewModelBase
	{
		public static User LoggedInUser { get; set; }


		private readonly NotificationService notificationService;

		public delegate void EventHandler();

		public event EventHandler RateGuests;

		public event EventHandler CheckRequests;

		public event EventHandler CheckForum;


		private Notifications _selectedNotification;
		public Notifications SelectedNotification
		{
			get { return _selectedNotification; }
			set
			{
				_selectedNotification = value;
				OnPropertyChanged(nameof(SelectedNotification));
			}
		}

		private ObservableCollection<Notifications> _notifications;
		public ObservableCollection<Notifications> Notifications
		{
			get { return _notifications; }
			set
			{
				_notifications = value;
				OnPropertyChanged(nameof(Notifications));
			}
		}



		private RelayCommand checkOut;
		public RelayCommand CheckOut
		{
			get { return checkOut; }
			set
			{
				checkOut = value;
			}
		}

		

		public OwnerNotificationsViewModel(User owner)
		{
			LoggedInUser = owner;
			notificationService = new NotificationService();
			Notifications = new ObservableCollection<Notifications>(notificationService.NotifyOwner(owner));
			CheckOut = new RelayCommand(Execute_CheckOut, CanExecute);
		}

		private bool CanExecute(object parameter)
		{
			return true;
		}

		private void Execute_CheckOut(object sender)
		{
			var selectedNotification = SelectedNotification;
			if (selectedNotification != null)
			{
				selectedNotification.IsRead = true;
				notificationService.Update(selectedNotification);

				if (selectedNotification.NotifType == NotificationType.RateGuest)
				{
					RateGuests?.Invoke();
				}

				if (selectedNotification.NotifType == NotificationType.CheckRequests)
				{
					CheckRequests?.Invoke();
				}

				if (selectedNotification.NotifType == NotificationType.Forum)
				{
					CheckForum?.Invoke();
				}
			}
		}



		}
}

