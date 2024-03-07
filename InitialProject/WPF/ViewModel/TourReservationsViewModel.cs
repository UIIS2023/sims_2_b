using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
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
    public class TourReservationsViewModel : ViewModelBase
    {
        public static ObservableCollection<TourReservation> ReservedTours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTour { get; set; }
        public TourReservation SelectedReservedTour { get; set; }
        public User LoggedInUser { get; set; }
        public TourPoint CurrentPoint { get; set; }
        public Tour ActiveTour { get; set; }
        public Action CloseAction { get; set; }
        private readonly TourReservationService _tourReservationService;
        private readonly IMessageBoxService _messageBoxService;
        
        public ICommand ChangeGuestNumCommand { get; set; }
        public ICommand GiveUpReservationCommand { get; set; }


        public TourReservationsViewModel(User user)
        {
            _tourReservationService = new TourReservationService();
            _messageBoxService = new MessageBoxService();
            InitializeProperties(user);
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            GiveUpReservationCommand =  new RelayCommand(Execute_GiveUpReservationCommand, CanExecute_Command);
            ChangeGuestNumCommand =new RelayCommand(Execute_ChangeGuestNumCommand, CanExecute_Command);
            
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void InitializeProperties(User user)
        {
            LoggedInUser = user;
            ReservedTours = new ObservableCollection<TourReservation>(_tourReservationService.GetByUser(user));
            Locations = new ObservableCollection<Location>();
        }

        

        private void Execute_ChangeGuestNumCommand(object obj)
        {
            if (SelectedReservedTour != null)
            {
                ReserveTour resTour = new ReserveTour(SelectedTour, SelectedReservedTour, LoggedInUser);
                resTour.Show();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you want to change");
            }
        }

        private void Execute_GiveUpReservationCommand(object obj)
        {
            string message = "Are your sure you want to delete this reservation?";
            string title = "Confirmation window";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _tourReservationService.Delete(SelectedReservedTour);
                ReservedTours.Remove(SelectedReservedTour);
            }
            
        }

        
    }
}
