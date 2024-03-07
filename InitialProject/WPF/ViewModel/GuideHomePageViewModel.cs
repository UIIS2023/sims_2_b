using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModel
{
    public class GuideHomePageViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public Tour TopTour { get; set; }
        public int NumOfUpcomingTours { get; set; }
        public string AvarageGrade { get; set; }


        private RelayCommand logOut;
        public RelayCommand LogOutCommand
        {
            get => logOut;
            set
            {
                if (value != logOut)
                {
                    logOut = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand upcoming;
        public RelayCommand UpcomingCommand
        {
            get => upcoming;
            set
            {
                if (value != upcoming)
                {
                    upcoming = value;
                    OnPropertyChanged(nameof(UpcomingCommand));
                }
            }
        }

        private RelayCommand allRatings;
        public RelayCommand AllRatingsCommand
        {
            get => allRatings;
            set
            {
                if (value != allRatings)
                {
                    allRatings = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand top;
        public RelayCommand TopCommand
        {
            get => top;
            set
            {
                if (value != top)
                {
                    top = value;
                    OnPropertyChanged();
                }
            }
        }

        public delegate void EventHandler1();
        public delegate void EventHandler2();
        public delegate void EventHandler3();

        public event EventHandler1 RatingsEvent;
        public event EventHandler2 DetailsEvent;
        public event EventHandler3 TopEvent;

        private readonly TourService _tourService;
        private readonly TourGuideReviewsService _tourGuideReviewService;

        public GuideHomePageViewModel(User user)
         {
            LoggedInUser = user;
            _tourService = new TourService();
            _tourGuideReviewService = new TourGuideReviewsService();
            InitializeCommands();
           
            InitializeProperties();
        }

        private void InitializeCommands()
        {
            LogOutCommand = new RelayCommand(Execute_LogOut, CanExecute_Command);
            UpcomingCommand = new RelayCommand(Execute_Upcoming, CanExecute_Command);
            AllRatingsCommand = new RelayCommand(Execute_AllRatings, CanExecute_Command);
            TopCommand = new RelayCommand(Execute_Top, CanExecute_Command);
        }

        private void Execute_Top(object obj)
        {
            TopEvent?.Invoke();
        }

        private void Execute_AllRatings(object obj)
        {
            RatingsEvent?.Invoke();
        }

        private void InitializeProperties()
        {
            TopTour = _tourService.GetTopTour(LoggedInUser);
            NumOfUpcomingTours = _tourService.GetNumOfUpcomingTours(LoggedInUser);
            AvarageGrade = _tourGuideReviewService.GetAvarageGrade(LoggedInUser).ToString("F2");
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_LogOut(object obj)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show(); 

            foreach (Window window in Application.Current.Windows)
            {
                if (window is GuideFrame)
                {
                    window.Close();
                }
            }
        }

        private void Execute_Upcoming(object obj)
        {
            DetailsEvent?.Invoke();
        }

    }
}
