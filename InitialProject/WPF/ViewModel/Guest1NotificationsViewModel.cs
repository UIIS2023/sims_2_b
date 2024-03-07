using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using InitialProject.Applications.UseCases;
using InitialProject.Converters;
using InitialProject.Domain.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModel
{
    public class Guest1NotificationsViewModel : ViewModelBase
    {
        public RelayCommand<string> NotificationSelectedCommand { get; private set; }
        public Action CloseAction;
        public static User LoggedInUser { get; set; }
        private readonly IMessenger _messenger;

        private string _selectedNotification;
        public string SelectedNotification
        {
            get { return _selectedNotification; }
            set
            {
                _selectedNotification = value;
                OnPropertyChanged(nameof(SelectedNotification));
                HandleNotificationSelected(value);
            }
        }

        private SortedObservableCollection<string> _notifications;
        public SortedObservableCollection<string> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
            }
        }
        public Guest1NotificationsViewModel(User user)
        {
            Notifications = new SortedObservableCollection<string>();
            _messenger = Messenger.Default;
            LoggedInUser = user;

            NotificationSelectedCommand = new RelayCommand<string>(OnNotificationSelected);

            Execute_Notifications();
        }

        private void OnNotificationSelected(string selectedNotification)
        {
            SelectedNotification = selectedNotification;
        }

        private void HandleNotificationSelected(string selectedNotification)
        {
            if (selectedNotification != null)
            {
                if (selectedNotification.Contains("zahtev za pomeranje"))
                {
                    // Send a navigation message to the main view model to navigate to the request tab
                    _messenger.Send(new NotificationMessage("NavigateToRequestTab"));
                }
                else if (selectedNotification.Contains("message"))
                {
                    // Send a navigation message to the main view model to navigate to the message tab
                    _messenger.Send(new NotificationMessage("NavigateToMessageTab"));
                }
                else if (selectedNotification.Contains("booking"))
                {
                    // Send a navigation message to the main view model to navigate to the booking tab
                    _messenger.Send(new NotificationMessage("NavigateToBookingTab"));
                }
                CloseAction();
            }
        }
        private int latestNotificationIndex = -1;

        private void Execute_Notifications()
        {
            bool hasNewNotifications = false;

            hasNewNotifications = IsNotified(hasNewNotifications);

            if (!hasNewNotifications)
            {
                Notifications.Add("Nema novih obavestenja!");
                CloseAction();
            }
            
        }

        private bool IsNotified(bool hasNewNotifications)
        {
            for (int i = latestNotificationIndex + 1; i < Guest1MainWindowViewModel.RequestsList.Count; i++)
            {
                ReservationDisplacementRequest r = Guest1MainWindowViewModel.RequestsList[i];

                if (r.Type == RequestType.Approved && r.IdUser==LoggedInUser.Id)
                {
                    Notifications.Add("Vlasnik je odobrio zahtev za pomeranje rezervacije " + r.Reservation.Accommodation.Name);
                    hasNewNotifications = true;
                }
                else if (r.Type == RequestType.Rejected && r.IdUser == LoggedInUser.Id)
                {
                    Notifications.Add("Vlasnik nije odobrio zahtev za pomeranje rezervacije " + r.Reservation.Accommodation.Name);
                    hasNewNotifications = true;
                }

                latestNotificationIndex = i;
            }

            return hasNewNotifications;
        }
    }
}
