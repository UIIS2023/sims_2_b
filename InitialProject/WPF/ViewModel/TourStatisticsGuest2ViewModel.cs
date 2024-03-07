using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModel
{
    public class TourStatisticsGuest2ViewModel : ViewModelBase
    {
        public static User LoggedInUser { get; set; }
        public static string TopGuestNum { get; set; }
        public static double TopAcceptedRequests { get; set; }
        public static double TopRejectedRequests { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public static ObservableCollection<int> Years { get; set; }
        public static ObservableCollection<string> Languages { get; set; }
        public static ObservableCollection<String> Countries { get; set; }
        private readonly TourRequestService _tourRequestService;
        private readonly LocationRepository _locationRepository;

        private SeriesCollection _generalPie;

        public SeriesCollection GeneralPie
        {
            get => _generalPie;
            set
            {
                if (_generalPie != value)
                {
                    _generalPie = value;
                    OnPropertyChanged("GeneralPie");
                }
            }
        }
        private SeriesCollection _selectedYearPie;

        public SeriesCollection SelectedYearPie
        {
            get => _selectedYearPie;
            set
            {
                if (_selectedYearPie != value)
                {
                    _selectedYearPie = value;
                    OnPropertyChanged("SelectedYearPie");
                }
            }
        }

        private SeriesCollection _locationPie;
        public SeriesCollection LocationPie
        {
            get => _locationPie;
            set
            {
                if (_locationPie != value)
                {
                    _locationPie = value;
                    OnPropertyChanged("LocationPie");
                }
            }
        }

        private SeriesCollection _languagePie;

        public SeriesCollection LanguagePie
        {
            get => _languagePie;
            set
            {
                if (_languagePie != value)
                {
                    _languagePie = value;
                    OnPropertyChanged("LanguagePie");
                }
            }
        }


        public TourStatisticsGuest2ViewModel(User user)
        {
            LoggedInUser = user;
            _tourRequestService = new TourRequestService();
            _locationRepository = new LocationRepository();
            InitializeProperties();
            BindLocation();
        }

        private void InitializeProperties()
        {
            Years = new ObservableCollection<int>(_tourRequestService.GetAllYears());
            Languages = new ObservableCollection<string>(_tourRequestService.GetLanguages());
            TourRequests = new List<TourRequest>(_tourRequestService.GetAll());
            Countries = new ObservableCollection<String>(_locationRepository.GetAllCountries());
            Cities = new ObservableCollection<String>();
            TopGuestNum = _tourRequestService.GetTopGuestNumGeneral().ToString();
            TopAcceptedRequests = _tourRequestService.GetTopAcceptedRequests();
            TopRejectedRequests = _tourRequestService.GetTopRejectedRequests();
            TopYearGuestNum = TopGuestNum;

            GeneralPie = new SeriesCollection();
            CreateGeneralPie();

            SelectedYearPie = new SeriesCollection();
            CreateSelectedYearPie(Years[0]);

            LocationPie = new SeriesCollection();
            CreateLocationPie();

            LanguagePie = new SeriesCollection();
            CreateLanguagePie();
        }

      

        private void CreateSelectedYearPie(int selectedYear)
        {
            SelectedYearPie = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Accepted request",
                    Values = new ChartValues<double> { _tourRequestService.GetTopYearAcceptedRequests(selectedYear) },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb6c1")),
                    LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
                    },
                    new PieSeries
                    {
                        Title = "Rejected request",
                    Values = new ChartValues<double> { _tourRequestService.GetTopYearRejectedRequests(selectedYear) },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff69b4")),
                    LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
                    }
                };
            
        }

        private void BindLocation()
        {
            foreach (Tour tour in ToursViewModel.ToursCopyList)
            {
                tour.Location = _locationRepository.GetById(tour.IdLocation);
            }
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


        private String _selectedYear;
        public String SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    CreateSelectedYearPie(int.Parse(SelectedYear));
                    //TopYearGuestNum = _tourRequestService.GetTopGuestByYear(int.Parse(SelectedYear)).ToString();
                    OnPropertyChanged(nameof(SelectedYear));
                    //OnPropertyChanged(nameof(TopYearGuestNum));
                }
            }
        }

        

        private String _selectedYearDown;
        public String SelectedYearDown
        {
            get { return _selectedYearDown; }
            set
            {
                if (_selectedYearDown != value)
                {
                    _selectedYearDown = value;
                    TopYearGuestNum = _tourRequestService.GetTopGuestByYear(int.Parse(SelectedYearDown)).ToString();
                    OnPropertyChanged(nameof(SelectedYearDown));
                    OnPropertyChanged(nameof(TopYearGuestNum));
                }
            }
        }


        private string _topYearGuestNum;
        public string TopYearGuestNum
        {
            get { return _topYearGuestNum; }
            set
            {
                if (_topYearGuestNum != value)
                {
                    _topYearGuestNum = value;

                    OnPropertyChanged(nameof(TopYearGuestNum));

                }
            }
        }

        
        private void CreateGeneralPie()
        {
            GeneralPie.Add(new PieSeries
            {
                Title = "Accepted requests",
                Values = new ChartValues<double> { TopAcceptedRequests },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffb6c1")),
                LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
            });
            GeneralPie.Add(new PieSeries
            {
                Title = "Rejected requests",
                Values = new ChartValues<double> { TopRejectedRequests },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff69b4")),
                LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
            });
        }

        private void CreateLanguagePie()
        {
            var colors = new List<Color>
                {
                    Colors.DeepPink,
                    Colors.HotPink,
                    Colors.LightPink,
                    Colors.Pink,
                };

            var languageIndex = 0;
            foreach (var language in Languages)
            {
                var fillBrush = new SolidColorBrush(colors[languageIndex % colors.Count]);

                LanguagePie.Add(new PieSeries
                {
                    Title = language,
                    Values = new ChartValues<double> { _tourRequestService.GetLanguageGuestNum(language) },
                    DataLabels = true,
                    Fill = fillBrush,
                    LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
                });

                languageIndex++;
            }
        }

        private void CreateLocationPie()
        {
            var colors = new List<Color>
                {
                    Colors.DeepPink,
                    Colors.HotPink,
                    Colors.LightPink,
                    Colors.DarkOrchid,
                    Colors.MediumPurple,
                    Colors.Purple,
                    Colors.PeachPuff,
                    Colors.DarkOrchid,
                };

            var locationIndex = 0;
            var locationSeries = new SeriesCollection();

            foreach (var country in Countries)
            {
                foreach (var city in _locationRepository.GetCities(country))
                {
                    var fillBrush = new SolidColorBrush(colors[locationIndex % colors.Count]);

                    locationSeries.Add(new PieSeries
                    {
                        Title = country + "," + city,
                        Values = new ChartValues<double> { _tourRequestService.GetLocationGuestNum(country, city) },
                        DataLabels = true,
                        Fill = fillBrush,
                        LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
                    });

                    locationIndex++;
                }
            }

            LocationPie = locationSeries;
        }



    }
}
