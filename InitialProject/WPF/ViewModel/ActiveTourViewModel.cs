using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
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
    public class ActiveTourViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public static User LoggedUser { get; set; }
        public static ObservableCollection<Tour> ActiveTour { get; set; }
        public static ObservableCollection<TourPoint> TourPoints { get; set; }
        public ICommand ConfirmAttendenceCommand { get; set; }

        private readonly TourService _tourService;
        private readonly TourPointService _tourPointService;
        private readonly TourAttendanceService _tourAttendanceService;
        public int brojac = 0;

        public delegate void EventHandler1();

        public event EventHandler1 ConfirmEvent;

        public ActiveTourViewModel(User user)
        {
            InitializeCommands();
            LoggedUser = user;
            _tourService= new TourService();
            _tourPointService = new TourPointService();
            _tourAttendanceService= new TourAttendanceService();
            ActiveTour = new ObservableCollection<Tour>(_tourService.GetActiveTour());
            TourPoints = new ObservableCollection<TourPoint>(_tourPointService.GetAllByTourId(ActiveTour[0].Id));
        }

        private void InitializeCommands()
        {
            ConfirmAttendenceCommand = new RelayCommand(Execute_ConfirmAttendenceCommand, CanExecute_Command);   
        }

        


        private void Execute_ConfirmAttendenceCommand(object obj)
        {
            ConfirmEvent?.Invoke();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
