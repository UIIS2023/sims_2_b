using InitialProject.Applications.UseCases;
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

namespace InitialProject.WPF.ViewModel
{
    class GuideFrameViewModel : ViewModelBase
    {
        private RelayCommand menu;
        public RelayCommand MenuBarCommand
       {
            get => menu;
            set
            {
                if (value != menu)
                {
                    menu = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand back;
        public RelayCommand BackCommand
        {
            get => back;
            set
            {
                if (value != back)
                {
                    back = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand home;
        public RelayCommand HomeCommand
        {
            get => home;
            set
            {
                if (value != home)
                {
                    home = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand demo;
        public RelayCommand DemoCommand
        {
            get => demo;
            set
            {
                if (value != demo)
                {
                    demo = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand user;
        public RelayCommand UserCommand
        {
            get => user;
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged();
                }

            }
        }

        public User LoggedInUser { get; set; }
        private Page frame;
        public  Page FrameContent 
        {
              get { return frame; }
              set
              {
                  if (frame != value)
                  {
                    frame = value;
                    OnPropertyChanged(nameof(FrameContent));
                  }
              }
          }


        public TourService tourService;
        private readonly AcceptTourRequestViewModel acceptTourRequestViewModel;
        private readonly GuideMainWindowViewModel upcomingVm;
        private readonly GuideHomePageViewModel profileVm;

        public GuideFrameViewModel(User user)
        {
            LoggedInUser= user;

            profileVm = new GuideHomePageViewModel(LoggedInUser);
            acceptTourRequestViewModel = new AcceptTourRequestViewModel(LoggedInUser);
            upcomingVm = new GuideMainWindowViewModel(LoggedInUser);

            FrameContent = new GuideHomePage(profileVm);
            profileVm.DetailsEvent += OnUpcomingTours;
            profileVm.RatingsEvent += OnRatings;
            profileVm.TopEvent += OnMostVisited;
            tourService = new TourService();

            InitializeCommands();
        }


        private void InitializeCommands()
        {
            MenuBarCommand = new RelayCommand(Execute_MenuBar, CanExecute_Command);
            BackCommand = new RelayCommand(Execute_Back, CanExecute_Command);
            HomeCommand = new RelayCommand(Execute_Home, CanExecute_Command);
            UserCommand = new RelayCommand(Execute_User, CanExecute_Command);
            DemoCommand = new RelayCommand(Execute_Demo, CanExecute_Command);
        }

        private void Execute_User(object obj)
        {
            GuideProfileViewModel guideProfileVm = new GuideProfileViewModel(LoggedInUser);
            FrameContent = new GuideProfile(guideProfileVm);

            guideProfileVm.RatingsEvent += OnRatings;
        }

        private void OnRatings()
        {
            GuideRatingsViewModel ratingsViewModel = new GuideRatingsViewModel(LoggedInUser);
            FrameContent = new GuideRatings(ratingsViewModel);
        }


        private void OnComplexRequest()
        {
            ViewComplexTourRequestViewModel complexViewModel = new ViewComplexTourRequestViewModel(LoggedInUser);
            FrameContent = new ViewComplexTourRequest(complexViewModel);

            complexViewModel.ComplexRequestDetailsEvent += OnComplexRequestDetails;
        }

        private void OnComplexRequestDetails(ComplexTourRequests complexRequest)
        {
            ViewOneComplexRequestViewModel oneComplexViewModel = new ViewOneComplexRequestViewModel(LoggedInUser, complexRequest.Id);
            FrameContent = new ViewOneComplexRequest(oneComplexViewModel);

            oneComplexViewModel.ChosenSimpleREvent += OnDateChoose;
        }

        private void OnDateChoose(TourRequest request)
        {
            ChooseAvailableDateViewModel chooseVm = new ChooseAvailableDateViewModel(request, LoggedInUser);
            ChooseAvailableDate chooseAvailableDate = new ChooseAvailableDate(chooseVm);
            chooseAvailableDate.Show();

            chooseVm.BackToComplex += OnComplexRequest;
        }

        private void Execute_Demo(object obj)
        {
            //
        }

        private void Execute_Home(object obj)
        {
            //GuideHomePageViewModel homeVm = new GuideHomePageViewModel(LoggedInUser);
            FrameContent = new GuideHomePage(profileVm);

            profileVm.DetailsEvent += OnUpcomingTours;
            profileVm.RatingsEvent += OnRatings;
            profileVm.TopEvent += OnMostVisited;
        }

        private void Execute_Back(object obj)
        {
           //
        }

        private void Execute_MenuBar(object obj)
        {
            var guideMenuBarVm = new GuideMenuBarViewModel(LoggedInUser);
            FrameContent = new GuideMenuBar(guideMenuBarVm);
           
            guideMenuBarVm.CreateTourEvent += OnCreate;
            guideMenuBarVm.TourTrackingEvent += OnTourTracking;
            guideMenuBarVm.UpcomingToursEvent += OnUpcomingTours;
            guideMenuBarVm.FinishedToursEvent += OnFinishedTours;
            guideMenuBarVm.MainPageEvent += OnMainPage;
            guideMenuBarVm.MostVisitedEvent += OnMostVisited;
            guideMenuBarVm.RequestEvent += OnRequest;
            guideMenuBarVm.ComplexRequestEvent += OnComplexRequest;
        }

        private void OnRequest()
        {
            FrameContent = new AcceptTourRequest(acceptTourRequestViewModel);
            acceptTourRequestViewModel.FilterEvent += OnFilter;
            acceptTourRequestViewModel.AcceptEvent += OnAccept;
            acceptTourRequestViewModel.StatistcisEvent += OnRequestStatistics;
            acceptTourRequestViewModel.CreateRequestEvent += OnCreateRequest;
            acceptTourRequestViewModel.CreateOnLanguage += OnLanguage;
            acceptTourRequestViewModel.CreateOnLocation += OnLocation;
        }

        private void OnLanguage(string language)
        {
            CreateTourByLanguageViewModel createTourByLanguageViewModel = new CreateTourByLanguageViewModel(LoggedInUser);
            FrameContent = new CreateTourByLanguage(createTourByLanguageViewModel);
            createTourByLanguageViewModel.EndCreatingEvent += OnUpcomingTours; 
        }

        private void OnLocation(string location)
        {
            CreateTourByLocationViewModel createTourByLocationViewModel = new CreateTourByLocationViewModel(LoggedInUser);
            FrameContent = new CreateTourByLocation(createTourByLocationViewModel);
            createTourByLocationViewModel.EndCreatingEvent += OnUpcomingTours;
        }

        private void OnCreateRequest()
        {
            TourRequestsStatisticsViewModel requestsStatisticsViewModel = new TourRequestsStatisticsViewModel();
            FrameContent = new TourRequestsStatistics(requestsStatisticsViewModel);
        }

        private void OnRequestStatistics()
        {
            TourRequestsStatisticsViewModel requestsStatisticsViewModel = new TourRequestsStatisticsViewModel();
            FrameContent = new TourRequestsStatistics(requestsStatisticsViewModel);
        }

        private void OnAccept(TourRequest request)
        {
            ChooseRequestDateViewModel chooseRequestDateViewModel = new ChooseRequestDateViewModel(LoggedInUser, request);
            FrameContent = new ChooseRequestDate(chooseRequestDateViewModel);

            chooseRequestDateViewModel.EndCreatingRequestEvent += OnRequest;
        }

        private void OnFilter()
        {
            RequestFilterViewModel requestVm = new RequestFilterViewModel();
            FrameContent = new RequestFilter(requestVm);

            requestVm.DoneFilteringEvent += OnRequest;
        }
        private void OnCreate()
        {
            CreateTourViewModel createTourViewModel = new CreateTourViewModel(LoggedInUser);
            FrameContent = new CreateTour(createTourViewModel);

            createTourViewModel.EndCreatingEvent += OnUpcomingTours;
        }

        private void OnTourTracking()
        {
            TourTrackingViewModel tourTrackingVm = new TourTrackingViewModel(LoggedInUser);
            FrameContent = new TourTracking(tourTrackingVm);
            tourTrackingVm.TourPointsEvent += OnTourPoints;
        }

        private void OnTourPoints(Tour tour)
        {
            TourPointsViewModel tourPointsVm = new TourPointsViewModel(tour);
            FrameContent = new TourPoints(tourPointsVm);

            tourPointsVm.GuestsEvent += OnGuests;
        }

        private void OnGuests(Tour tour, TourPoint point)
        {
            FrameContent = new TourGuests(tour,point);
        }

        private void OnUpcomingTours()
        {
            FrameContent = new GuideMainWindow(upcomingVm);

            upcomingVm.MultiplyEvent += OnMultiply;
            upcomingVm.ViewGalleryEvent += OnViewGallery;
        }

        private void OnViewGallery(Tour tour)
        {
            FrameContent = new ViewTourGalleryGuide(tour);
        }

        private void OnMultiply(Tour tour)
        {
            AddDateViewModel addDateViewModel = new AddDateViewModel(tour);
            FrameContent = new AddDate(addDateViewModel);

            addDateViewModel.AddEvent += OnUpcomingTours;
        }

        private void OnFinishedTours()
        {
            FinishedToursViewModel finishedVm = new FinishedToursViewModel(LoggedInUser);
            FrameContent= new FinishedTours(finishedVm);

            finishedVm.StatisticsEvent += OnStatistics;
        }

        private void OnStatistics(Tour tour)
        {
            FrameContent = new TourStatistics(tour);
        }

        private void OnMainPage()
        {
            FrameContent = new GuideHomePage(profileVm);

            profileVm.DetailsEvent += OnUpcomingTours;
            profileVm.RatingsEvent += OnRatings;
            profileVm.TopEvent += OnMostVisited;
        }

        private void OnMostVisited()
        {
            FrameContent = new TheMostVisitedTour(LoggedInUser);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
