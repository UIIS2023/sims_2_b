using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
    public class ChooseRequestDateViewModel : ViewModelBase
    {

        private RelayCommand createRequest;
        public RelayCommand CreateRequestCommand
        {
            get => createRequest;
            set
            {
                if (value != createRequest)
                {
                    createRequest = value;
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

        public Tour Tour
        {
            get { return tour; }
            set
            {
                tour = value;
                OnPropertyChanged("Tour");
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

        private string _date;
        public string Date
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }


        public TourRequest SelectedRequest { get; set; }
        private readonly TourService _tourService;
        private readonly TourPointService _tourPointService;
        private readonly TourRequestService _tourRequestService;
        private readonly MessageBoxService _messageBoxService;
        private readonly NotificationService _notificationService;
        private readonly IImageRepository _imageRepository;
        public User LoggedInUser { get; set; }
        public Tour tour = new Tour();

        public List<string> ImagePaths { get; set; }

        public DateTime StartInterval { get; set; }
        public DateTime EndInterval { get; set; }

        public delegate void EventHandler1();
        public event EventHandler1 EndCreatingRequestEvent;

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


        public ChooseRequestDateViewModel(User user, TourRequest tourRequest)
        {
            LoggedInUser= user;
            CreateRequestCommand = new RelayCommand(Execute_CreateRequest, CanExecute_Command);
            AddImagesCommand = new RelayCommand(Execute_AddImages, CanExecute_Command);
            SelectedRequest = tourRequest;
            _tourService = new TourService();
            _tourPointService = new TourPointService();
            _tourRequestService = new TourRequestService();
            _messageBoxService = new MessageBoxService();
            _notificationService = new NotificationService();
            _imageRepository = Inject.CreateInstance<IImageRepository>();

            StartInterval = new DateTime(SelectedRequest.NewStartDate.Year, SelectedRequest.NewStartDate.Month, SelectedRequest.NewStartDate.Day);
            EndInterval = new DateTime(SelectedRequest.NewEndDate.Year, SelectedRequest.NewEndDate.Month, SelectedRequest.NewEndDate.Day);
        }


        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_AddImages(object obj)
        {
            ImagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Tours", "/Resources/Images");
        }

        private bool IsTimeValid()
        {
            if (StartTime == "")
            {
                ValidationResult2 = "Time is required";
                return false;
            }
            ValidationResult2 = "";
            return true;
        }

        private void Execute_CreateRequest(object obj)
        {
            Tour.Location = SelectedRequest.Location;
            Tour.Language = SelectedRequest.TourLanguage;
            Tour.Descripiton = SelectedRequest.Description;

            //Tour.Validate();
            //bool validTime = IsTimeValid();

            //if (Tour.IsValid && validTime)
            //{

            if (!_tourService.IsUserFree(LoggedInUser, DateOnly.Parse(Date)))
            {
                _messageBoxService.ShowMessage("You are not available at this date, try new date");
                return;
            }
                
                TimeOnly _startTime = ConvertTime(StartTime);
                Tour newTour = new Tour(Tour.Name, Tour.Location, Tour.Language, Tour.MaxGuestNum, DateOnly.Parse(Date), _startTime, Duration, Tour.MaxGuestNum, false, LoggedInUser.Id, Tour.Location.Id, false); ;

                Tour savedTour = _tourService.Save(newTour);
                GuideMainWindowViewModel.Tours.Add(newTour);

                SelectedRequest.Status = RequestType.Approved;
                SelectedRequest.NewStartDate = DateOnly.Parse(Date);
                _tourRequestService.Update(SelectedRequest);
                AcceptTourRequestViewModel.Requests.Remove(SelectedRequest);
                AcceptTourRequestViewModel.RequestsCopyList.Remove(SelectedRequest);

                CreatePoints(savedTour);
                CreateImages(savedTour);

                EndCreatingRequestEvent?.Invoke();
                CreateNotification(savedTour);
           //}
           //else
           // {
           //     OnPropertyChanged(nameof(Tour));
           // }

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

        public void CreateNotification(Tour tour)
        {
            _notificationService.GenerateNotifications(SelectedRequest, tour);
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

