using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Image = InitialProject.Domain.Model.Image;
using System.Windows.Input;
using InitialProject.Commands;
using System.Xml.Linq;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using InitialProject.WPF.Converters;

namespace InitialProject.WPF.ViewModel
{
    public class CreateTourViewModel : ViewModelBase
    {
        public Tour tour = new Tour();
        public User LoggedInUser { get; set; }
        private readonly TourService _tourService;
        private readonly TourPointService _tourPointService;
        private readonly ILocationRepository _locationRepository;
        private readonly IImageRepository _imageRepository;

        public static ObservableCollection<String> Countries { get; set; }
        public static List<Image> Images { get; set; }
        public List<string> ImagePaths { get; set; }

        private RelayCommand confirmCreate;
        public RelayCommand CreateTourCommand
        {
            get => confirmCreate;
            set
            {
                if (value != confirmCreate)
                {
                    confirmCreate = value;
                    OnPropertyChanged();
                }

            }
        } 

        private RelayCommand cancel;
        public RelayCommand CancelTourCommand
        {
            get => cancel; 
            set
            {
                if(value != cancel)
                {
                    cancel = value;
                    OnPropertyChanged();
                }

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


        public delegate void EventHandler1();

        public event EventHandler1 EndCreatingEvent;


        public CreateTourViewModel(User user)
        {
            LoggedInUser = user;
            _locationRepository = Inject.CreateInstance<ILocationRepository>();
            _tourService = new TourService();
            _tourPointService = new TourPointService();
            _imageRepository = Inject.CreateInstance<IImageRepository>();
            Countries = new ObservableCollection<String>(_locationRepository.GetAllCountries());
            Cities = new ObservableCollection<String>();
            Images = new List<Image>();
            CreateTourCommand = new RelayCommand(Execute_CreateTour, CanExecute_Command);
            CancelTourCommand = new RelayCommand(Execute_CancelTour, CanExecute_Command);
            AddImagesCommand = new RelayCommand(Execute_AddImages, CanExecute_Command);

        }

       
 private ObservableCollection<String> _cities;
        public ObservableCollection<String> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private String _selectedCity;
        public String SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;

            }
        }

        private String _selectedCountry;
        public String SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    Cities = new ObservableCollection<String>(_locationRepository.GetCities(SelectedCountry));
                    
                    OnPropertyChanged(nameof(Cities));
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        private string _validationResult;
        public string ValidationResult
        {
            get { return _validationResult; }
            set
            {
                _validationResult = value;
                OnPropertyChanged(nameof(ValidationResult));
            }
        }


        private string _validationResult2;
        public string ValidationResult2
        {
            get { return _validationResult2; }
            set
            {
                _validationResult2 = value;
                OnPropertyChanged(nameof(ValidationResult2));
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

        public Tour Tour
        {
            get { return tour; }
            set
            {
                tour = value;
                OnPropertyChanged("Tour");
            }
        }

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
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_CancelTour(object sender)
        {
        }

        private bool IsCityValid()
        {
            if(SelectedCity  == null)
            {
                ValidationResult = "City is required";
                return false;
            }
            ValidationResult = "";
            return true;
        }

        private bool IsTimeValid()
        {
            if (StartTime  == "")
            {
                ValidationResult2 = "Time is required";
                return false;
            }
            ValidationResult2 = "";
            return true;
        }

        private void Execute_CreateTour(object sender)
        {

            Tour.Validate();
            bool validCity = IsCityValid();
            bool validTime = IsTimeValid();

            if (Tour.IsValid && validCity && validTime)
            {
                TimeOnly _startTime = ConvertTime(StartTime);

                Location location = _locationRepository.FindLocation(SelectedCountry, SelectedCity);
                

                Tour newTour = new Tour(Tour.Name, location, Tour.Language, Tour.MaxGuestNum, DateOnly.Parse(Date), _startTime, int.Parse(Tour.DurationS), Tour.MaxGuestNum, false, LoggedInUser.Id, location.Id, false); ;

                Tour savedTour = _tourService.Save(newTour);
                GuideMainWindowViewModel.Tours.Add(newTour);

                CreatePoints(savedTour);
                CreateImages(savedTour);
           
                EndCreatingEvent?.Invoke();

            }
            else
            {
                OnPropertyChanged(nameof(Tour));
            }

        }

        private void Execute_AddImages(object obj)
        { 
            ImagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images");
        }

        private void CreateImages(Tour savedTour)
        {
            foreach (string name in ImagePaths)
            {
                Image newImage = new Image(name, 0, savedTour.Id, 0);
                Image savedImage = _imageRepository.Save(newImage);
                savedTour.Images.Add(savedImage);
            }
        }
        
        private void CreatePoints(Tour savedTour)
        {
            string[] pointsNames = Tour.Points.Split(",");
            int order = 1;
            foreach (string name in pointsNames)
            {
                TourPoint newTourPoint = new TourPoint(name, false, false, order, savedTour.Id);
                TourPoint savedTourPoint = _tourPointService.Save(newTourPoint);
                //savedTour.Points.Add(savedTourPoint);
                order++;
            }

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
