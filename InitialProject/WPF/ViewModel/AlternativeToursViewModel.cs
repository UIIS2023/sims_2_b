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
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class AlternativeToursViewModel : ViewModelBase
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Tour> AlternativeToursMainList { get; set; }
        public static ObservableCollection<Tour> AlternativeToursCopyList { get; set; }
        public User LoggedInUser { get; set; }
        public Tour SelectedTour { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        public Tour SelectedAlternativeTour { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public Action CloseAction { get; set; }
        private int AgainGuestNum { get; set; }
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly IMessageBoxService _messageBoxService;
        public ICommand ReserveAlternativeCommand { get; set; }
        public ICommand ViewGalleryCommand { get; set; }
        public ICommand AlternativeFilteringCommand { get; set; }
        public ICommand RestartCommand { get; set; }
        private int _maxGuestNum;
        public int MaxGuestNum
        {
            get => _maxGuestNum;
            set
            {
                if (value != _maxGuestNum)
                {
                    _maxGuestNum = value;
                    OnPropertyChanged("MaxGuestNum");
                }
            }
        }
        public AlternativeToursViewModel(User user, Tour tour, TourReservation tourReservation, int againGuestNum, Tour alternativeTour)
        {
            SelectedTour = tour;
            SelectedTourReservation = tourReservation;
            AgainGuestNum = againGuestNum;
            SelectedAlternativeTour = alternativeTour;
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _messageBoxService = new MessageBoxService();
            InitializeProperties(user);
            InitializeCommands();
            FilingMainList();
            FilingCopyList();

        }

        private static void FilingCopyList()
        {
            AlternativeToursCopyList.Clear();

            foreach (Tour t in AlternativeToursMainList)
            {
                AlternativeToursCopyList.Add(t);
            }
        }

        private void FilingMainList()
        {
            foreach (Tour tours in AlternativeToursCopyList)
            {
                if (SelectedTourReservation != null)
                {
                    ReservedAlternativeTourList(tours);
                }
                else
                {
                    AlternativeTourList(tours);
                }

            }
        }

        private void InitializeProperties(User user)
        {
            LoggedInUser = user;
            Tours = new ObservableCollection<Tour>(_tourService.GetAllByUser(user));
            AlternativeToursMainList = new ObservableCollection<Tour>();
            AlternativeToursCopyList = new ObservableCollection<Tour>(_tourService.GetUpcomingTours());
            Locations = new ObservableCollection<Location>();
        }

        private void InitializeCommands()
        {
            ReserveAlternativeCommand = new RelayCommand(Execute_ReserveAlternativeCommand, CanExecute_Command);
            ViewGalleryCommand =  new RelayCommand(Execute_ViewGalleryCommand, CanExecute_Command);
            AlternativeFilteringCommand = new RelayCommand(Execute_AlternativeFilteringCommand, CanExecute_Command);
            RestartCommand =  new RelayCommand(Execute_RestartCommand, CanExecute_Command);
        }

        private void Execute_RestartCommand(object obj)
        {
            AlternativeToursMainList.Clear();
            foreach (Tour t in AlternativeToursCopyList)
            {
                AlternativeToursMainList.Add(t);
            }
        }

        private void Execute_AlternativeFilteringCommand(object obj)
        {
            AlternativeTourFiltering filterAlternativeTour = new AlternativeTourFiltering();
            filterAlternativeTour.Show();
        }

        private void Execute_ViewGalleryCommand(object obj)
        {
            ViewTourGalleryGuest viewTourGalleryGuest = new ViewTourGalleryGuest(LoggedInUser, SelectedTour);
            viewTourGalleryGuest.Show();
        }

        private void Execute_ReserveAlternativeCommand(object obj)
        {
            if (SelectedAlternativeTour != null)
            {
                ReserveAlternativeTour();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you can reserve");
            }
            CloseAction();
        }

        private void ReserveAlternativeTour()
        {
            SelectedAlternativeTour.FreeSetsNum=SelectedAlternativeTour.MaxGuestNum;
            SelectedAlternativeTour.FreeSetsNum -= AgainGuestNum;
            string TourName = _tourService.GetTourNameById(SelectedAlternativeTour.Id);
            TourReservation newAlternativeTour = new TourReservation(SelectedAlternativeTour.Id, TourName, LoggedInUser.Id, AgainGuestNum, SelectedAlternativeTour.FreeSetsNum, -1);
            TourReservation savedAlternativeTour = _tourReservationService.Save(newAlternativeTour);
            TourReservationsViewModel.ReservedTours.Add(savedAlternativeTour);
        }


        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void AlternativeTourList(Tour tours)
        {
            if (SelectedTour.Location.Country == tours.Location.Country && SelectedTour.Location.City == tours.Location.City && AgainGuestNum <= tours.MaxGuestNum)
            {
                AlternativeToursMainList.Add(tours);
            }
        }

        private void ReservedAlternativeTourList(Tour tours)
        {
            Location location = _tourService.GetLocationById(SelectedTourReservation.IdTour);
            if (location.Country == tours.Location.Country && location.City == tours.Location.City && AgainGuestNum <= tours.MaxGuestNum)
            {
                AlternativeToursMainList.Add(tours);
            }
        }
    }
}
