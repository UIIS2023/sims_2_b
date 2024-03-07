using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
    public class TourGuestsViewModel : ViewModelBase
    {
        public static ObservableCollection<TourReservation> Users { get; set; }

        public Tour Tour;
        public TourReservation SelectedUser { get; set; }
        public TourPoint CurrentPoint { get; set; }

        private readonly TourReservationService _tourReservationService;
        private readonly UserService _userService;
        private readonly TourAttendanceService _tourAttendanceService;

       

        private RelayCommand _addGuests;
        public RelayCommand AddGuestCommand
        {
            get => _addGuests;
            set
            {
                if (value != _addGuests)
                {
                    _addGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _done;
        public RelayCommand DoneAddingCommand
        {
            get => _done;
            set
            {
                if (value != _done)
                {
                    _done = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourGuestsViewModel(Tour tour, TourPoint tourPoint)
        {
            _tourReservationService = new TourReservationService();
            _userService = new UserService();
            _tourAttendanceService = new TourAttendanceService();

            CurrentPoint = tourPoint;
            Tour = tour;
            Users = new ObservableCollection<TourReservation>(_tourReservationService.GetByTour(CurrentPoint.IdTour));
           
            AddGuestCommand = new RelayCommand(Execute_AddGuest, CanExecute_Command);
            DoneAddingCommand = new RelayCommand(Execute_DoneAdding, CanExecute_Command);
        }

        private void Execute_DoneAdding(object obj)
        {

        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_AddGuest(object obj)
        {
            User user = _userService.GetById(SelectedUser.Id);
            CurrentPoint.Guests.Add(user);
            TourAttendance tourAttendance = new TourAttendance(CurrentPoint.IdTour, Tour.IdUser, SelectedUser.Id, CurrentPoint.Id, SelectedUser.UsedVoucher, CurrentPoint.Name);
            _tourAttendanceService.Save(tourAttendance);
            Users.Remove(SelectedUser);
            
        }

    }
}
