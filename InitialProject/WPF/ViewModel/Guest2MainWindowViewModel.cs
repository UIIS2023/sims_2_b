using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repository;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class Guest2MainWindowViewModel : ViewModelBase
    {/*
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Tour> ToursMainList { get; set; }
        public static ObservableCollection<Tour> ToursCopyList { get; set; }
        public static ObservableCollection<TourReservation> ReservedTours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTour { get; set; }
        public TourReservation SelectedReservedTour { get; set; }
        public User LoggedInUser { get; set; }
        public TourPoint CurrentPoint { get; set; }
        public Tour ActiveTour { get; set; }
        public Action CloseAction { get; set; }
        public List<Tour> tours { get; set; }
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly LocationRepository _locationRepository;
        private readonly TourAttendanceService _tourAttendanceService;
        public ICommand ReserveTourCommand { get; set; }
        public ICommand ViewTourGalleryCommand { get; set; }
        public ICommand AddFiltersCommand { get; set; }
        public ICommand RestartCommand { get; set; }
        public ICommand ToursCommand { get; set; }
        public ICommand ReservationsCommand { get; set; }
        public ICommand VouchersCommand { get; set; }
        public ICommand ActiveTourCommand { get; set; }
        public ICommand TourAttendenceCommand { get; set; }
        public ICommand CheckNotificationsCommand { get; set; }
        public ICommand MyAccountCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        private readonly IMessageBoxService _messageBoxService;

        public Guest2MainWindowViewModel(User user)
        {
            _tourReservationService= new TourReservationService();
            _tourService = new TourService();
            _locationRepository = new LocationRepository();
            _tourAttendanceService = new TourAttendanceService();
            _messageBoxService = new MessageBoxService();
            InitializeProperties(user);
            InitializeCommands();
            BindLocation();
        }

        private void InitializeProperties(User user)
        {
            LoggedInUser = user;
            Tours = new ObservableCollection<Tour>(_tourService.GetUpcomingTours());
            ToursMainList = new ObservableCollection<Tour>(_tourService.GetUpcomingTours());
            ToursCopyList = new ObservableCollection<Tour>(_tourService.GetUpcomingTours());
            ReservedTours = new ObservableCollection<TourReservation>(_tourReservationService.GetByUser(user));
            Locations = new ObservableCollection<Location>();
            ReservedTours = new ObservableCollection<TourReservation>(_tourReservationService.GetByUser(user));

        }

        private void InitializeCommands()
        {
            ReserveTourCommand = new RelayCommand(Execute_ReserveTourCommand, CanExecute_Command);
            AddFiltersCommand =  new RelayCommand(Execute_AddFiltersCommand, CanExecute_Command);
            ViewTourGalleryCommand = new RelayCommand(Execute_ViewTourGalleryCommand, CanExecute_Command);
            RestartCommand = new RelayCommand(Execute_RestartCommand, CanExecute_Command);
            ToursCommand = new RelayCommand(Execute_ToursCommand, CanExecute_Command);
            ReservationsCommand = new RelayCommand(Execute_ReservationsCommand, CanExecute_Command);
            VouchersCommand = new RelayCommand(Execute_VouchersCommand, CanExecute_Command);
            ActiveTourCommand =new RelayCommand(Execute_ActiveTourCommand, CanExecute_Command);
            TourAttendenceCommand = new RelayCommand(Execute_TourAttendenceCommand, CanExecute_Command);
            CheckNotificationsCommand =  new RelayCommand(Execute_CheckNotificationsCommand, CanExecute_Command);
            MyAccountCommand =new RelayCommand(Execute_MyAccountCommand, CanExecute_Command);
            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
        }

        private void Execute_ReservationsCommand(object obj)
        {
            TourReservations tourReservations = new TourReservations(LoggedInUser);
            tourReservations.Show();
            CloseAction();
        }

        private void Execute_LogOutCommand(object obj)
        {
            CloseAction();
        }

        private void Execute_MyAccountCommand(object obj)
        {
            Guest2Account guest2Account = new Guest2Account(LoggedInUser);
            guest2Account.Show();
            CloseAction();
        }

        private void Execute_CheckNotificationsCommand(object obj)
        {
            int brojac = 0;
            Tour activ = new Tour();
            GetCurrentActiveTour(ref brojac, ref activ);
            NewNotification(brojac, activ);
        }

        private void NewNotification(int brojac, Tour activ)
        {
            string message = LoggedInUser.Username + " are you present at current active tour " + activ.Name + "?";
            string title = "Confirmation window";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            MessageBoxResult(brojac, activ, result);
        }

        private void MessageBoxResult(int brojac, Tour activ, MessageBoxResult result)
        {
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                ActiveTour activeTour = new ActiveTour(LoggedInUser, brojac);
                activeTour.Show();
                CloseAction();
            }
            else
            {
                DeleteFromAttendedTours(activ);

            }
        }

        private void DeleteFromAttendedTours(Tour activ)
        {
            foreach (TourAttendance tA in _tourAttendanceService.GetAllAttendedToursByUser(LoggedInUser))
            {
                if (activ.Id ==  tA.IdTour)
                {
                    _tourAttendanceService.Delete(tA);
                }
            }
        }

        private void GetCurrentActiveTour(ref int brojac, ref Tour activ)
        {
            foreach (TourAttendance tourAttendence in _tourAttendanceService.GetAllAttendedToursByUser(LoggedInUser))
            {
                Tour tour = _tourService.GetById(tourAttendence.IdTour);
                if (tour.Active==true)
                {
                    brojac =1;
                    activ=tour;
                }
            }
        }

        private void Execute_ToursCommand(object obj)
        {

            Guest2MainWindow guest2MainWindow = new Guest2MainWindow(LoggedInUser);
            guest2MainWindow.Show();
            CloseAction();
        }

        private void Execute_VouchersCommand(object obj)
        {

            TourVouchers tourVouchers = new TourVouchers(LoggedInUser, SelectedTour, null);
            tourVouchers.Show();
            CloseAction();

        }
        private void Execute_TourAttendenceCommand(object obj)
        {
            TourAttendence tourAttendance = new TourAttendence(LoggedInUser);
            tourAttendance.Show();
            CloseAction();
        }

        private void Execute_ActiveTourCommand(object obj)
        {
            ActiveTour activeTour = new ActiveTour(LoggedInUser, 0);
            activeTour.Show();
            CloseAction();
        }

        private void Execute_RestartCommand(object obj)
        {
            ToursMainList.Clear();
            foreach (Tour t in ToursCopyList)
            {
                t.Location = _locationRepository.GetById(t.IdLocation);
                ToursMainList.Add(t);
            }
        }

        private void Execute_ViewTourGalleryCommand(object obj)
        {
            if (SelectedTour != null)
            {
                  ViewTourGallery viewTourGallery = new ViewTourGallery(SelectedTour);
                  viewTourGallery.Show();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you want to see");
            }
        }

        private void Execute_AddFiltersCommand(object obj)
        {
            TourFiltering tourFiltering = new TourFiltering();
            tourFiltering.Show();
        }

        private void Execute_ReserveTourCommand(object obj)
        {
            if (SelectedTour != null)
            {
                ReserveTour resTour = new ReserveTour(SelectedTour, SelectedReservedTour, LoggedInUser);
                resTour.Show();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you can reserve");
            }
        }

        private void BindLocation()
        {
            foreach (Tour tour in ToursCopyList)
            {
                tour.Location = _locationRepository.GetById(tour.IdLocation);
            }
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }*/
    }
}
