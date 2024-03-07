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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class TourAttendenceViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public static ObservableCollection<TourAttendance> ToursAttended { get; set; }
        public TourAttendance SelectedAttendedTour { get; set; }
        public static string TourPointName { get; set; }
        
        public User LoggedUser { get; set; }
        private readonly TourAttendanceService _tourAttendenceService;
        private readonly TourService _tourService;
        private readonly TourPointService _tourPointService;
        private readonly IMessageBoxService _messageBoxService;
        public ICommand RateTourCommand { get; set; }

        public TourAttendenceViewModel(User user)
        {
            LoggedUser =user;
            _tourAttendenceService = new TourAttendanceService();
            ToursAttended =  new ObservableCollection<TourAttendance>(_tourAttendenceService.GetAllAttendedToursByUser(user));
            _tourService = new TourService();
            _tourPointService = new TourPointService();
            _messageBoxService = new MessageBoxService();
            InitializeCommands();
            BindData();
        }

        private void BindData()
        {
            foreach (TourAttendance tourAttendance in ToursAttended)
            {
                tourAttendance.Tour = _tourService.GetById(tourAttendance.IdTour);
                tourAttendance.TourPointName = _tourPointService.GetTourPointNameByTourPointId(tourAttendance.IdTourPoint);
            }
        }

        private void InitializeCommands()
        {
            RateTourCommand = new RelayCommand(Execute_RateTourCommand, CanExecute_Command);   
        }


        private void Execute_RateTourCommand(object obj)
        {
            if (SelectedAttendedTour != null)
            {
                if (SelectedAttendedTour.Rated==false)
                {
                    RateTour rateTour = new RateTour(LoggedUser, SelectedAttendedTour);
                    rateTour.Show();
                }
                else
                {
                    _messageBoxService.ShowMessage("This attended tour was already rated, you can rate, you can rate some unrated ones");
                }
            }
            else
            {
                _messageBoxService.ShowMessage("You need to select attended tour you want to rate");
            }
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
