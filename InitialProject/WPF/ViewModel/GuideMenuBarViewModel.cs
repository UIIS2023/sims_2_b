using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static InitialProject.WPF.ViewModel.AccommodationUCViewModel;

namespace InitialProject.WPF.ViewModel
{
    public class GuideMenuBarViewModel : ViewModelBase
    {
        public User LoggedInUser;


        private RelayCommand main_page;
        public RelayCommand MainPageCommand
        {
            get => main_page;
            set
            {
                if (value != main_page)
                {
                    main_page = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand upcoming_tours;
        public RelayCommand UpcomingToursCommand
        {
            get => upcoming_tours;
            set
            {
                if (value != upcoming_tours)
                {
                    upcoming_tours = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand create;
        public RelayCommand CreateTourCommand
        {
            get => create;
            set
            {
                if (value != create)
                {
                    create = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand tourTrack;
        public RelayCommand TourTrackingCommand
        {
            get => tourTrack;
            set
            {
                if (value != tourTrack)
                {
                    tourTrack = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand mostVisited;
        public RelayCommand MostVisitedCommand
        {
            get => mostVisited;
            set
            {
                if (value != mostVisited)
                {
                    mostVisited = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand finishedTours;
        public RelayCommand FinishedToursCommand
        {
            get => finishedTours;
            set
            {
                if (value != finishedTours)
                {
                    finishedTours = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand request;
        public RelayCommand RequestCommand
        {
            get => request;
            set
            {
                if (value != request)
                {
                    request = value;
                    OnPropertyChanged();
                }

            }
        }
        private RelayCommand complexrequest;
        public RelayCommand ComplexRequestCommand
        {
            get => complexrequest;
            set
            {
                if (value != complexrequest)
                {
                    complexrequest = value;
                    OnPropertyChanged();
                }

            }
        }

        public delegate void EventHandler1();
        public delegate void EventHandler2();
        public delegate void EventHandler3();
        public delegate void EventHandler4();
        public delegate void EventHandler5();
        public delegate void EventHandler6();
        public delegate void EventHandler7();
        public delegate void EventHandler8();

        public event EventHandler1 MainPageEvent;
        public event EventHandler2 UpcomingToursEvent;
        public event EventHandler3 CreateTourEvent;
        public event EventHandler4 TourTrackingEvent;
        public event EventHandler5 MostVisitedEvent;
        public event EventHandler6 FinishedToursEvent;
        public event EventHandler7 RequestEvent;
        public event EventHandler8 ComplexRequestEvent;

        public GuideMenuBarViewModel(User user)
        {

            LoggedInUser = user;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            MainPageCommand = new RelayCommand(Execute_MainPage, CanExecute_Command);
            UpcomingToursCommand = new RelayCommand(Execute_UpComingTours, CanExecute_Command);
            CreateTourCommand = new RelayCommand(Execute_CreateTour, CanExecute_Command);
            TourTrackingCommand = new RelayCommand(Execute_TourTracking, CanExecute_Command);
            MostVisitedCommand = new RelayCommand(Execute_MostVisited, CanExecute_Command);
            FinishedToursCommand = new RelayCommand(Execute_FinishedTours, CanExecute_Command);
            RequestCommand = new RelayCommand(Execute_Request,CanExecute_Command);
            ComplexRequestCommand = new RelayCommand(Execute_ComplexRequest, CanExecute_Command);
        }

        private void Execute_ComplexRequest(object obj)
        {
            ComplexRequestEvent?.Invoke();
        }

        private void Execute_Request(object obj)
        {
            RequestEvent?.Invoke();
        }

        private void Execute_FinishedTours(object obj)
        {
            FinishedToursEvent?.Invoke();
        }

        private void Execute_MostVisited(object obj)
        {
            MostVisitedEvent?.Invoke();
        }

        private void Execute_TourTracking(object obj)
        {
            TourTrackingEvent?.Invoke();
        }

        private void Execute_CreateTour(object obj)
        {
            CreateTourEvent?.Invoke();
        }

        private void Execute_UpComingTours(object obj)
        {
            UpcomingToursEvent?.Invoke();
        }

        private void Execute_MainPage(object obj)
        {
            MainPageEvent?.Invoke();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
