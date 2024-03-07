using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class MenuWindowGuest2ViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public Tour tour { get; set; }
        public TourReservation tourReservation { get; set; }
        public TourRequest tourRequest { get; set; }
        public Notifications notifications { get; set; }
        public Action CloseAction { get; set; }
        public UserControl CurrentUserControl { get; set; }
        public ComplexTourRequests complexTourRequests { get; set; }

        private int requestNumber;
        public ICommand ToursCommand { get; set; }
        public ICommand ReservationsCommand { get; set; }
        public ICommand TourRequestsCommand { get; set; }
        public ICommand ComplexTourRequestCommand { get; set; }
        public ICommand VouchersCommand { get; set; }
        public ICommand StatisticsCommand { get; set; }
        public ICommand ActiveTourCommand { get; set; }
        public ICommand TourAttendenceCommand { get; set; }
        public ICommand CheckNotificationsCommand { get; set; }
        public ICommand MyAccountCommand { get; set; }

        private readonly TourService _tourService;
        private readonly TourAttendanceService _tourAttendanceService;
        private readonly MessageBoxService _messageService;

        public MenuWindowGuest2ViewModel(User guest)
        {
            LoggedInUser=guest;
            var toursViewModel = new ToursViewModel(guest);
            CurrentUserControl = new ToursGuest2(guest, toursViewModel);
            _tourService = new TourService();
            _tourAttendanceService = new TourAttendanceService();
            _messageService =  new MessageBoxService();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ToursCommand = new RelayCommand(Execute_ToursCommand, CanExecute_Command);
            ReservationsCommand = new RelayCommand(Execute_ReservationsCommand, CanExecute_Command);
            TourRequestsCommand = new RelayCommand(Execute_TourRequestsCommand, CanExecute_Command);
            ComplexTourRequestCommand = new RelayCommand(Execute_ComplexTourRequestCommand, CanExecute_Command);
            VouchersCommand = new RelayCommand(Execute_VouchersCommand, CanExecute_Command);
            StatisticsCommand = new RelayCommand(Execute_StatisticsCommand, CanExecute_Command);
            ActiveTourCommand =new RelayCommand(Execute_ActiveTourCommand, CanExecute_Command);
            TourAttendenceCommand = new RelayCommand(Execute_TourAttendenceCommand, CanExecute_Command);
            CheckNotificationsCommand =  new RelayCommand(Execute_CheckNotificationsCommand, CanExecute_Command);
            MyAccountCommand =new RelayCommand(Execute_MyAccountCommand, CanExecute_Command);
        }

        private void Execute_ComplexTourRequestCommand(object obj)
        {
            var complexTourRequestViewModel = new ComplexTourRequestViewModel(LoggedInUser, complexTourRequests);
            CurrentUserControl.Content = new AllComplexTourRequests(LoggedInUser, complexTourRequestViewModel);

            complexTourRequestViewModel.CreateComplexTourRequestEvent += OnCreateComplexTourRequest;
            complexTourRequestViewModel.AllComplexTourPartsEvent += OnAllComplexTourParts;
        }


        private void OnAllComplexTourParts(ComplexTourRequests complexTourRequests)
        {
            ComplexTourRequestParts complexTourRequestParts = new ComplexTourRequestParts(LoggedInUser, complexTourRequests);
            complexTourRequestParts.Show();
        }

        private void OnCreateComplexTourRequest()
        {
            CreateComplexTourRequest createComplexTourRequest = new CreateComplexTourRequest(LoggedInUser, complexTourRequests);
            createComplexTourRequest.Show();
        }

        private void Execute_StatisticsCommand(object obj)
        {
            var tourStatisticsGuest2ViewModel = new TourStatisticsGuest2ViewModel(LoggedInUser);
            CurrentUserControl.Content = new TourStatisticsGuest2(LoggedInUser, tourStatisticsGuest2ViewModel);

        }

        private void Execute_TourRequestsCommand(object obj)
        {
            var tourRequestsViewModel = new TourRequestsViewModel(LoggedInUser, tourRequest);
            CurrentUserControl.Content = new TourRequests(LoggedInUser, tourRequestsViewModel);

            tourRequestsViewModel.CreateTourRequest += OnCreateTourRequest;
        }

        private void OnCreateTourRequest()
        {
            CreateTourRequest createTourRequest = new CreateTourRequest(LoggedInUser, requestNumber, complexTourRequests);
            createTourRequest.Show();
        }

        private void Execute_MyAccountCommand(object obj)
        {
            var guest2AccountViewModel = new Guest2AccountViewModel(LoggedInUser);
            CurrentUserControl.Content = new Guest2Account(LoggedInUser, guest2AccountViewModel);

            guest2AccountViewModel.LogOutEvent += OnLogOutEvent;

        }

        private void OnLogOutEvent()
        {

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            //ZATVARANJE PROZORA

            foreach (Window window in Application.Current.Windows)
            {
                if (window is MenuWindowGuest2)
                {
                    window.Close();
                }
            }

            
        }

        private void Execute_CheckNotificationsCommand(object obj)
        {
            var notificationsGuest2ViewModel = new NotificationsGuest2ViewModel(LoggedInUser);
            CurrentUserControl.Content = new NotificationsGuest2(LoggedInUser, notificationsGuest2ViewModel);

            notificationsGuest2ViewModel.CheckAcceptedTourRequests += OnCheckAcceptedTourRequests;
            notificationsGuest2ViewModel.CheckCreatedTours += OnCheckCreatedTours;
            notificationsGuest2ViewModel.CheckVouchers += OnCheckVouchers;
        }

        private void OnCheckVouchers()
        {
            var tourVouchersViewModel = new TourVouchersViewModel(LoggedInUser, tour, tourReservation);
            CurrentUserControl.Content = new TourVouchers(LoggedInUser, tour, tourReservation, tourVouchersViewModel);
        }

        private void OnCheckAcceptedTourRequests()
        {

            var tourRequestsViewModel = new TourRequestsViewModel(LoggedInUser, tourRequest);
            CurrentUserControl.Content = new TourRequests(LoggedInUser, tourRequestsViewModel);
        }

        private void OnCheckCreatedTours()
        {
           

            var toursViewModel = new ToursViewModel(LoggedInUser);
            CurrentUserControl.Content = new ToursGuest2(LoggedInUser, toursViewModel);
        }


        private void Execute_TourAttendenceCommand(object obj)
        {
            var tourAttendanceViewModel = new TourAttendenceViewModel(LoggedInUser);
            CurrentUserControl.Content = new TourAttendence(LoggedInUser, tourAttendanceViewModel);
        }

        private void Execute_ActiveTourCommand(object obj)
        {
                Tour activ = new Tour();
                int brojac = 0;
                foreach (TourAttendance tourAttendence in _tourAttendanceService.GetAllAttendedToursByUser(LoggedInUser))
                {
                    Tour tour = _tourService.GetById(tourAttendence.IdTour);
                    if (tour.Active==true)
                    {
                        activ=tour;
                        brojac++;
                    }
                }
                if(brojac!=0 )
                {
                    string message = LoggedInUser.Username + " are you present at current active tour " + activ.Name + "?";
                    string title = "Confirmation window";
                    MessageBoxButton buttons = MessageBoxButton.YesNo;
                    MessageBoxResult result = MessageBox.Show(message, title, buttons);
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        var activeTourViewModel = new ActiveTourViewModel(LoggedInUser);
                        CurrentUserControl.Content = new ActiveTour(LoggedInUser, activeTourViewModel);

                        activeTourViewModel.ConfirmEvent += OnConfirmEvent;
                    }
                    else
                    {
                        foreach (TourAttendance tA in _tourAttendanceService.GetAllAttendedToursByUser(LoggedInUser))
                        {
                            if (activ.Id ==  tA.IdTour)
                            {
                                _tourAttendanceService.Delete(tA);
                            }
                        }

                        var toursViewModel = new ToursViewModel(LoggedInUser);
                        CurrentUserControl.Content = new ToursGuest2(LoggedInUser, toursViewModel);

                    }
                 }
            else
                {
                _messageService.ShowMessage("There isn't any active tour currentlly!");
                }
                
        }

        private void OnConfirmEvent()
        {
            var tourAttendanceViewModel = new TourAttendenceViewModel(LoggedInUser);
            CurrentUserControl.Content = new TourAttendence(LoggedInUser, tourAttendanceViewModel);
        }

        private void Execute_VouchersCommand(object obj)
        {
            var tourVouchersViewModel = new TourVouchersViewModel(LoggedInUser, tour, tourReservation);
            CurrentUserControl.Content = new TourVouchers(LoggedInUser, tour,tourReservation, tourVouchersViewModel);

        }

        private void Execute_ReservationsCommand(object obj)
        {
            var tourReservationsViewModel = new TourReservationsViewModel(LoggedInUser);
            CurrentUserControl.Content = new TourReservations(LoggedInUser, tourReservationsViewModel);
        }

        
        private void Execute_ToursCommand(object obj)
        {
            var toursViewModel = new ToursViewModel(LoggedInUser);
            CurrentUserControl.Content = new ToursGuest2(LoggedInUser, toursViewModel);

            toursViewModel.ReserveEvent += OnReserveEvent;
        }


        private void OnReserveEvent()
        {
            var tourReservationsViewModel = new TourReservationsViewModel(LoggedInUser);
            CurrentUserControl.Content = new TourReservations(LoggedInUser, tourReservationsViewModel);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

    }
}
