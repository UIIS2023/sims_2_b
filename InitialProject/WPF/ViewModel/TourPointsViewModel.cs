using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
    public class TourPointsViewModel : ViewModelBase
    {
        public static ObservableCollection<TourPoint> Points { get; set; }
        public Tour SelectedTour { get; set; }
        public int MaxOrder { get; set; }

        public int Order = 0;

        private readonly TourPointService _tourPointService;
        private readonly TourService _tourService;
        private readonly MessageBoxService _messageBoxService;

        public delegate void EventHandler1(Tour tour, TourPoint point);

        public event EventHandler1 GuestsEvent;


        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    _active = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand suddenEnd;
        public RelayCommand SuddenEndCommand
        {
            get => suddenEnd;
            set
            {
                if (value != suddenEnd)
                {
                    suddenEnd= value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand pause;
        public RelayCommand PauseCommand
        {
            get => pause;
            set
            {
                if (value != pause)
                {
                    pause = value;
                    OnPropertyChanged();
                }
            }
        }


        public TourPointsViewModel(Tour tour)
        {
            SelectedTour = tour;
            _tourPointService = new TourPointService();
            _tourService = new TourService();
            _messageBoxService = new MessageBoxService();
            Points = new ObservableCollection<TourPoint>(_tourPointService.GetAllByTourId(SelectedTour.Id));
            MaxOrder = _tourPointService.GetMaxOrder(tour.Id); 
            SuddenEndCommand = new RelayCommand(Execute_SuddenEnd, CanExecute_Command);
            PauseCommand = new RelayCommand(Execute_Pause, CanExecute_Command);
        }

        private void Execute_Pause(object obj)
        {
            _messageBoxService.ShowMessage("Tour is paused");
            SelectedTour.Paused= true;
            _tourService.Update(SelectedTour);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_SuddenEnd(object obj)
        {
            _messageBoxService.ShowMessage("Tour is done");
            SelectedTour.Active = false;
            _tourService.Update(SelectedTour);
        }

        public void Changed()
        {
            Order++;
            if (Order == MaxOrder)
            {
                DoneTour(SelectedTour);
            }
            else
            {
                AddTourGuests();
            }
        }

        private void AddTourGuests()
        {
            foreach (TourPoint point in Points)
            {
                if (point.Active && !point.GuestAdded)
                {
                    point.GuestAdded = true;
                    _tourPointService.Update(point);
                    GuestsEvent?.Invoke(SelectedTour, point);
                }
            }
        }

        private void DoneTour(Tour selectedTour)
        {
            _messageBoxService.ShowMessage("Tour is done");

            int order = _tourPointService.GetMaxOrder(SelectedTour.Id);
            _tourPointService.Update(_tourPointService.GetByOrder(order));

            SelectedTour.Active = false;
            _tourService.Update(SelectedTour);

        }

    }
}
