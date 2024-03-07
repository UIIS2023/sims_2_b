using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Image = InitialProject.Domain.Model.Image;

namespace InitialProject.WPF.ViewModel
{
    public class RateTourViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public TourAttendance SelectedAttendedTour { get; set; }
        public static User User { get; set; }
        private readonly TourGuideReviewRepository tourGuideReviewRepository;
        private readonly ImageRepository _imageRepository;
        private readonly TourAttendanceService _tourAttendenceService;
        public static List<Image> Images { get; set; }
        public List<string> ImagePaths { get; set; }
        public TourGuideReview tourGuideReview = new TourGuideReview();

        public ICommand KnoledgeRadiosCommand { get; set; }
        public ICommand LanguageRadiosCommand { get; set; }
        public ICommand InterestingRadiosCommand { get; set; }



        public RateTourViewModel(User user, TourAttendance tourAttendance)
        {
            SelectedAttendedTour = tourAttendance;
            User = user;
            tourGuideReviewRepository = new TourGuideReviewRepository();
            _imageRepository = new ImageRepository();
            _tourAttendenceService = new TourAttendanceService();
            Images = new List<Image>();
            InitializeCommands();
        }


        private bool _radioButton1IsChecked;
        public bool RadioButton1IsChecked
        {
            get { return _radioButton1IsChecked; }
            set
            {
                _radioButton1IsChecked = value;
                OnPropertyChanged(nameof(RadioButton1IsChecked));
            }
        }


        private bool _radioButton2IsChecked;
        public bool RadioButton2IsChecked
        {
            get { return _radioButton2IsChecked; }
            set
            {
                _radioButton2IsChecked = value;
                OnPropertyChanged(nameof(RadioButton2IsChecked));
            }
        }

        private bool _radioButton3IsChecked;
        public bool RadioButton3IsChecked
        {
            get { return _radioButton3IsChecked; }
            set
            {
                _radioButton3IsChecked = value;
                OnPropertyChanged(nameof(RadioButton3IsChecked));
            }
        }

        private bool _radioButton4IsChecked;
        public bool RadioButton4IsChecked
        {
            get { return _radioButton4IsChecked; }
            set
            {
                _radioButton4IsChecked = value;
                OnPropertyChanged(nameof(RadioButton4IsChecked));
            }
        }

        private bool _radioButton5IsChecked;
        public bool RadioButton5IsChecked
        {
            get { return _radioButton5IsChecked; }
            set
            {
                _radioButton5IsChecked = value;
                OnPropertyChanged(nameof(RadioButton5IsChecked));
            }
        }


        private bool _radioButton6IsChecked;
        public bool RadioButton6IsChecked
        {
            get { return _radioButton6IsChecked; }
            set
            {
                _radioButton6IsChecked = value;
                OnPropertyChanged(nameof(RadioButton6IsChecked));
            }
        }

        private bool _radioButton7IsChecked;
        public bool RadioButton7IsChecked
        {
            get { return _radioButton7IsChecked; }
            set
            {
                _radioButton7IsChecked = value;
                OnPropertyChanged(nameof(RadioButton7IsChecked));
            }
        }

        private bool _radioButton8IsChecked;
        public bool RadioButton8IsChecked
        {
            get { return _radioButton8IsChecked; }
            set
            {
                _radioButton8IsChecked = value;
                OnPropertyChanged(nameof(RadioButton8IsChecked));
            }
        }

        private bool _radioButton9IsChecked;
        public bool RadioButton9IsChecked
        {
            get { return _radioButton9IsChecked; }
            set
            {
                _radioButton9IsChecked = value;
                OnPropertyChanged(nameof(RadioButton9IsChecked));
            }
        }

        private bool _radioButton10IsChecked;
        public bool RadioButton10IsChecked
        {
            get { return _radioButton10IsChecked; }
            set
            {
                _radioButton10IsChecked = value;
                OnPropertyChanged(nameof(RadioButton10IsChecked));
            }
        }

        private bool _radioButton11IsChecked;
        public bool RadioButton11IsChecked
        {
            get { return _radioButton11IsChecked; }
            set
            {
                _radioButton11IsChecked = value;
                OnPropertyChanged(nameof(RadioButton11IsChecked));
            }
        }

        private bool _radioButton12IsChecked;
        public bool RadioButton12IsChecked
        {
            get { return _radioButton12IsChecked; }
            set
            {
                _radioButton12IsChecked = value;
                OnPropertyChanged(nameof(RadioButton12IsChecked));
            }
        }

        private bool _radioButton13IsChecked;
        public bool RadioButton13IsChecked
        {
            get { return _radioButton13IsChecked; }
            set
            {
                _radioButton13IsChecked = value;
                OnPropertyChanged(nameof(RadioButton13IsChecked));
            }
        }

        private bool _radioButton14IsChecked;
        public bool RadioButton14IsChecked
        {
            get { return _radioButton14IsChecked; }
            set
            {
                _radioButton14IsChecked = value;
                OnPropertyChanged(nameof(RadioButton14IsChecked));
            }
        }

        private bool _radioButton15IsChecked;
        public bool RadioButton15IsChecked
        {
            get { return _radioButton15IsChecked; }
            set
            {
                _radioButton15IsChecked = value;
                OnPropertyChanged(nameof(RadioButton15IsChecked));
            }
        }


        public TourGuideReview TourGuideReviews
        {
            get { return tourGuideReview; }
            set
            {
                tourGuideReview = value;
                OnPropertyChanged("TourGuideReviews");
            }
        }

        private RelayCommand addImages;
        public RelayCommand AddImagesCommand
        {
            get => addImages;
            set
            {
                if (value != addImages)
                {
                    addImages = value;
                    OnPropertyChanged();
                }

            }
        }

        private void InitializeCommands()
        {
            SubmitCommand = new RelayCommand(Execute_SubmitCommand, CanExecute_Command);
            GiveUpRatingCommand =  new RelayCommand(Execute_GiveUpRatingCommand, CanExecute_Command);
            KnoledgeRadiosCommand = new RelayCommand(Execute_KnoledgeRadiosCommand, CanExecute_Command);
            LanguageRadiosCommand = new RelayCommand(Execute_LanguageRadiosCommand, CanExecute_Command);
            InterestingRadiosCommand = new RelayCommand(Execute_InterestingRadiosCommand, CanExecute_Command);
            AddImagesCommand = new RelayCommand(Execute_AddImages, CanExecute_Command);
        }

        private void Execute_AddImages(object obj)
        {
            ImagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images");
        }

        private void Execute_InterestingRadiosCommand(object obj)
        {
            if (RadioButton11IsChecked == true)
            {
                TourGuideReviews.InterestingTour = 1;
            }
            else if (RadioButton12IsChecked == true)
            {
                TourGuideReviews.InterestingTour = 2;
            }
            else if (RadioButton13IsChecked == true)
            {
                TourGuideReviews.InterestingTour = 3;
            }
            else if (RadioButton14IsChecked == true)
            {
                TourGuideReviews.InterestingTour = 4;
            }
            else if (RadioButton15IsChecked == true)
            {
                TourGuideReviews.InterestingTour = 5;
            }
        }

        private void Execute_LanguageRadiosCommand(object obj)
        {
            if (RadioButton6IsChecked == true)
            {
                TourGuideReviews.GuideLanguage = 1;
            }
            else if (RadioButton7IsChecked == true)
            {
                TourGuideReviews.GuideLanguage = 2;
            }
            else if (RadioButton8IsChecked == true)
            {
                TourGuideReviews.GuideLanguage = 3;
            }
            else if (RadioButton9IsChecked == true)
            {
                TourGuideReviews.GuideLanguage = 4;
            }
            else if (RadioButton10IsChecked == true)
            {
                TourGuideReviews.GuideLanguage = 5;
            }
        }

        private void Execute_KnoledgeRadiosCommand(object obj)
        {
            if (RadioButton1IsChecked == true)
            {
                TourGuideReviews.GuideKnowledge = 1;
            }
            else if(RadioButton2IsChecked == true)
            {
                TourGuideReviews.GuideKnowledge = 2;
            }
            else if (RadioButton3IsChecked == true)
            {
                TourGuideReviews.GuideKnowledge = 3;
            }
            else if (RadioButton4IsChecked == true)
            {
                TourGuideReviews.GuideKnowledge = 4;
            }
            else if (RadioButton5IsChecked == true)
            {
                TourGuideReviews.GuideKnowledge = 5;
            }
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_GiveUpRatingCommand(object obj)
        {
            CloseAction();
        }

        private void Execute_SubmitCommand(object obj)
        {

            TourGuideReviews.Validate();

            if (TourGuideReviews.IsValid)
            {
                // Create a new OwnerReview object with the validated values and save it

                SelectedAttendedTour.Rated = true;
                _tourAttendenceService.Update(SelectedAttendedTour);
                TourGuideReview newTourGuideReview = new TourGuideReview(User.Id, SelectedAttendedTour.IdGuide, SelectedAttendedTour.IdTourPoint, TourGuideReviews.GuideKnowledge, TourGuideReviews.GuideLanguage, TourGuideReviews.InterestingTour, TourGuideReviews.Comment, SelectedAttendedTour.IdTour);
                TourGuideReview savedTourGuideRewiew = tourGuideReviewRepository.Save(newTourGuideReview);

                CreateImages(savedTourGuideRewiew);


                CloseAction();
            }
            else
            {
                // Update the view with the validation errors
                OnPropertyChanged(nameof(TourGuideReviews));
            }
        }

        private void CreateImages(TourGuideReview savedTourGuideRewiew)
        {
            foreach (string name in ImagePaths)
            {
                Image newImage = new Image(name, 0, savedTourGuideRewiew.Id, 0);
                Image savedImage = _imageRepository.Save(newImage);
                savedTourGuideRewiew.Images.Add(savedImage);
            }
        }

        private RelayCommand submitCommand;
        public RelayCommand SubmitCommand
        {
            get { return submitCommand; }
            set
            {
                submitCommand = value;
            }
        }


        private RelayCommand giveUpRatingCommand;
        public RelayCommand GiveUpRatingCommand
        {
            get { return giveUpRatingCommand; }
            set
            {
                giveUpRatingCommand = value;
            }
        }

    }
}
