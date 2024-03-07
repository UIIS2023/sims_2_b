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
using System.Windows.Media;

namespace InitialProject.WPF.ViewModel
{
    public class GuideProfileViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public string AvarageGrade { get; set; }

        private RelayCommand demission;
        public RelayCommand DemissionCommand
        {
            get => demission;
            set
            {
                if (value != demission)
                {
                    demission = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand ratings;
        public RelayCommand YourRatingsCommand
        {
            get => ratings;
            set
            {
                if (value != ratings)
                {
                    ratings = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public delegate void EventHandler1();
        public event EventHandler1 RatingsEvent;

        private readonly TourGuideReviewsService _tourGuideReviews;
        private readonly TourService _tourService;
        private readonly UserService _userService;
        private readonly MessageBoxService _messageBoxService;

        public string ImageSource { get; set; }
        public string UserImageSource { get; set; }


        public GuideProfileViewModel(User user)
        {
            _tourGuideReviews = new TourGuideReviewsService();
            _tourService = new TourService();
            _userService = new UserService();
            _messageBoxService = new MessageBoxService();
            LoggedInUser = user;
            AvarageGrade = _tourGuideReviews.GetAvarageGrade(user).ToString("F2");
            InitializeCommands();
            SetImagesSource(user);
        }

        private void InitializeCommands()
        {
            DemissionCommand = new RelayCommand(Execute_Demission, CanExecute_Command);
            YourRatingsCommand = new RelayCommand(Execute_YourRatings, CanExecute_Command);
            LogOutCommand = new RelayCommand(Execute_LogOut, CanExecute_Command);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_LogOut(object obj)
        {
            SignInForm signInForm= new SignInForm();
            signInForm.Show();


            foreach(Window window in Application.Current.Windows)
            {
                if(window is GuideFrame)
                {
                    window.Close();
                }
            }
        }

        private void Execute_YourRatings(object obj)
        {
            RatingsEvent?.Invoke();
        }

        private void Execute_Demission(object obj)
        {
            _tourService.DelayGuideUpcomingTours(LoggedInUser);
            _messageBoxService.ShowMessage("We will log out you from the sistem, all the best!");
            //_userService.DeleteUser(LoggedInUser);

            foreach (Window window in Application.Current.Windows)
            {
                if (window is GuideFrame)
                {
                    window.Close();
                }
            }
        }

        public void SetImagesSource(User user)
        {
            if (user.IsSuper == true)
            {
                ImageSource = "../../Resources/Images/true.png";
            }
            else
            {
                ImageSource = "../../Resources/Images/delete.png";
            }

        }
    }
}
