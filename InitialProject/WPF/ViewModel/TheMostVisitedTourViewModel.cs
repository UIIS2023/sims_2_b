using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class TheMostVisitedTourViewModel : ViewModelBase
    {
        public Tour TopTour { get; set; }
        public Tour TopYearTour { get; set; }
        public List<TourAttendance> ToursAttendances { get; set; }
        public List<Tour> Tours { get; set; }
        public static ObservableCollection<int> Years { get; set; }
        public User LoggedInUser { get; set; }
        public string TopTourPath { get; set; }

        private readonly ITourAttendanceRepository _tourAttendanceRepository;
        private readonly TourService _tourService;
        private readonly IImageRepository _imageRepository;

        public TheMostVisitedTourViewModel(User user)
        {
            _tourAttendanceRepository = Inject.CreateInstance<ITourAttendanceRepository>();
            _tourService = new TourService();
            _imageRepository= Inject.CreateInstance<IImageRepository>();
            LoggedInUser= user;
            InitializeProperties();
             
        }


        private String _selectedYear;
        public String SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    TopYearTour = _tourService.GetTopYearTour(LoggedInUser, int.Parse(SelectedYear));
                    OnPropertyChanged(nameof(TopYearTour));
                    OnPropertyChanged(nameof(SelectedYear));
                }
            }
        }

        void InitializeProperties()
        {
            ToursAttendances = new List<TourAttendance>(_tourAttendanceRepository.GetAllByGuide(LoggedInUser));
            Tours = new List<Tour>(_tourService.GetAllByUser(LoggedInUser));
            TopTour = _tourService.GetTopTour(LoggedInUser);
            TopTourPath = _imageRepository.GetUrlByTourId(TopTour.Id)[0];
            Years = new ObservableCollection<int>(_tourService.GetAllYears());
            TopYearTour = TopTour;
        }


    }
}
