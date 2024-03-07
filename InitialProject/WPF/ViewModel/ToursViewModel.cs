using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
    public class ToursViewModel : ViewModelBase
    {
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
        public ICommand ReportCommand { get; set; }
        private readonly IMessageBoxService _messageBoxService;

        public delegate void EventHandler1();
        public event EventHandler1 ReserveEvent;


        public ToursViewModel(User user)
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
            ReportCommand = new RelayCommand(Execute_ReportCommand, CanExecute_Command);
        }

        private void Execute_ReportCommand(object obj)
        {
            ReportGuest2ViewModel reportGuest2ViewModel = new ReportGuest2ViewModel(LoggedInUser);
            ReportGuest2 reportGuest2 = new ReportGuest2(LoggedInUser, reportGuest2ViewModel);
            reportGuest2.Show();
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
                ViewTourGalleryGuest viewTourGalleryGuest = new ViewTourGalleryGuest(LoggedInUser, SelectedTour);
                viewTourGalleryGuest.Show();
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

                ReserveEvent?.Invoke();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you want to reserve");
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
        }
    }
}
