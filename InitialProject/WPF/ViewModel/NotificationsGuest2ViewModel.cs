using GalaSoft.MvvmLight.Command;
using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using User = InitialProject.Domain.Model.User;

namespace InitialProject.WPF.ViewModel
{
    public class NotificationsGuest2ViewModel : ViewModelBase
    {
        public static User LoggedInUser { get; set; }
        private readonly NotificationService notificationService;
        private readonly TourRequestService tourRequestService;
        private readonly TourService tourService;
        private readonly VoucherService voucherService;
        public delegate void EventHandler();
        public event EventHandler CheckAcceptedTourRequests;
        public event EventHandler CheckCreatedTours;
        public event EventHandler CheckVouchers;

        public RelayCommand<Notifications> NotificationSelectedCommand { get; private set; }

        private Notifications _selectedNotification;
        public Notifications SelectedNotification
        {
            get { return _selectedNotification; }
            set
            {
                _selectedNotification = value;
                OnPropertyChanged(nameof(SelectedNotification));
                HandleNotificationSelected(value);
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

        public NotificationsGuest2ViewModel(User user)
        {
            LoggedInUser = user;
            notificationService = new NotificationService();
            tourRequestService = new TourRequestService();
            tourService = new TourService();
            voucherService = new VoucherService();
            Notifications = new ObservableCollection<Notifications>(notificationService.NotifyGuest2(user));
            NotificationSelectedCommand = new RelayCommand<Notifications>(OnNotificationSelected);
        }

        private void OnNotificationSelected(Notifications selectedNotification)
        {
            SelectedNotification = selectedNotification;
        }

        private void HandleNotificationSelected(Notifications selectedNotification)
        {
            if (selectedNotification != null)
            {
                selectedNotification.IsRead = true;
                notificationService.Update(selectedNotification);

                if (selectedNotification.NotifType == NotificationType.CheckAcceptedTourRequest)
                {
                    TourRequest approvedTours = notificationService.GetTourRequestByNotification(selectedNotification);


                    MoreDetailsRequest moreDetailsRequest = new MoreDetailsRequest(LoggedInUser, approvedTours);
                    moreDetailsRequest.Show();

                    CheckAcceptedTourRequests?.Invoke();
                }

                else if (selectedNotification.NotifType == NotificationType.CheckCreatedTour)
                {
                    Tour createdTour = notificationService.GetTourByNotification(selectedNotification);

                    ViewTourGalleryGuest viewTourGalleryGuest = new ViewTourGalleryGuest(LoggedInUser, createdTour);
                    viewTourGalleryGuest.Show();

                    CheckCreatedTours?.Invoke();
                }
                else if(selectedNotification.NotifType == NotificationType.VoucherWon)
                {
                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                    DateOnly futureDate = today.AddMonths(6);
                    Voucher voucher = new Voucher(LoggedInUser.Id, "Won voucher", futureDate);

                    Voucher savedVoucher = voucherService.Save(voucher);
                    TourVouchersViewModel.VouchersMainList.Add(savedVoucher);
                    CheckVouchers?.Invoke();
                }
            }
        }

    }
}
