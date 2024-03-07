using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using InitialProject.Commands;

namespace InitialProject.WPF.ViewModel
{
    public class AddDateViewModel : ViewModelBase
    {
        public Tour SelectedTour { get; set; }
        private readonly TourService _tourService;


        private string _startDate;
        public string Date
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _startTime;
        public string StartTime
        {
            get => _startTime;
            set
            {
                if (value != _startTime)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addTour;
        public RelayCommand AddDateCommand
        {
            get => _addTour;
            set
            {
                if (value != _addTour)
                {
                    _addTour = value;
                    OnPropertyChanged();
                }
            }
        }


        public delegate void EventHandler1();

        public event EventHandler1 AddEvent;


        public AddDateViewModel(Tour tour)
        {
            SelectedTour = tour;
            _tourService = new TourService();
            AddDateCommand = new RelayCommand(Execute_AddDate, CanExecute_Command);

        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_AddDate(object obj)
        {
            TimeOnly _startTime = ConvertTime(StartTime);
            Tour newTour = new Tour(SelectedTour.Name, SelectedTour.Location, SelectedTour.Language, SelectedTour.MaxGuestNum, DateOnly.Parse(Date), _startTime, SelectedTour.Duration, SelectedTour.MaxGuestNum, false, SelectedTour.IdUser, SelectedTour.IdLocation, false);
            Tour savedTour = _tourService.Save(newTour);
            GuideMainWindowViewModel.Tours.Add(savedTour);
            AddEvent?.Invoke();
        }

        public TimeOnly ConvertTime(string times)
        {
            StartTimes time = (StartTimes)Enum.Parse(typeof(StartTimes), times);
            TimeOnly startTime;
            switch (time)
            {
                case StartTimes._8AM:
                    startTime = new TimeOnly(8, 0);
                    break;
                case StartTimes._10AM:
                    startTime = new TimeOnly(10, 0);
                    break;
                case StartTimes._12PM:
                    startTime = new TimeOnly(12, 0);
                    break;
                case StartTimes._2PM:
                    startTime = new TimeOnly(14, 0);
                    break;
                case StartTimes._4PM:
                    startTime = new TimeOnly(16, 0);
                    break;
                case StartTimes._6PM:
                    startTime = new TimeOnly(18, 0);

                    break;
            }
            return startTime;
        }


    }
}
