using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModel
{
    public class TourRequestsStatisticsViewModel : ViewModelBase
    {

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;

        public static ObservableCollection<int> Years { get; set; }
        public static ObservableCollection<string> Locations { get; set; }
        public static ObservableCollection<string> Languages { get; set; }


        private ObservableCollection<StatisticsViewModel> statistics;
        public ObservableCollection<StatisticsViewModel> Statistics 
        {
            get => statistics;
            set
            {
                if(statistics != value)
                {
                    statistics = value;
                    OnPropertyChanged(nameof(Statistics));
                } 
            }
        }
        private ObservableCollection<StatisticsViewModel> statisticsMonthly;
        public ObservableCollection<StatisticsViewModel> StatisticsMonthly
        {
            get => statisticsMonthly;
            set
            {
                if (statisticsMonthly != value)
                {
                    statisticsMonthly = value;
                    OnPropertyChanged(nameof(statisticsMonthly));
                }
            }
        }

        public string SelectedLocation { get; set; }
        public string SelectedLanguage { get; set; }
        

        private RelayCommand find;
        public RelayCommand FindCommand
        {
            get => find;
            set
            {
                if (find != value)
                {
                    find = value;
                    OnPropertyChanged(nameof(FindCommand));
                }
            }
        }

        private RelayCommand find2;
        public RelayCommand Find2Command
        {
            get => find2;
            set
            {
                if (find2 != value)
                {
                    find2 = value;
                    OnPropertyChanged(nameof(Find2Command));
                }
            }
        }


         private int _selectedYear;
         public int SelectedYear
         {
             get => _selectedYear;
             set
             {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                }
                
             }
         }

        public TourRequestsStatisticsViewModel()
        {
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            Years = new ObservableCollection<int>(_tourRequestService.GetAllYears());
            Locations = new ObservableCollection<string>(_locationService.GetLocations());
            Languages= new ObservableCollection<string>(_tourRequestService.GetLanguages());

            Dictionary<int, int> statistics = _tourRequestService.GetTourRequestsPerYear(SelectedLocation, SelectedLanguage);
            Statistics = new ObservableCollection<StatisticsViewModel>(statistics.Select(s => new StatisticsViewModel { Year = s.Key, TourRequestsCount = s.Value }));

            Dictionary<int, int> statistics2 = _tourRequestService.GetTourRequestsPerMonth(SelectedYear, SelectedLocation, SelectedLanguage);
            StatisticsMonthly = new ObservableCollection<StatisticsViewModel>();


            FindCommand = new RelayCommand(Execute_Find, CanExecute_Command);
            Find2Command = new RelayCommand(Execute_Find2, CanExecute_Command);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_Find(object obj)
        {
            Dictionary<int, int> statistics = _tourRequestService.GetTourRequestsPerYear(SelectedLocation, SelectedLanguage);
            Statistics = new ObservableCollection<StatisticsViewModel>(statistics.Select(s => new StatisticsViewModel { Year = s.Key, TourRequestsCount = s.Value }));
        }

        private void Execute_Find2(object obj)
        {
            Dictionary<int, int> statistics2 = _tourRequestService.GetTourRequestsPerMonth(SelectedYear, SelectedLocation, SelectedLanguage);
            StatisticsMonthly = new ObservableCollection<StatisticsViewModel>(statistics2.Select(s => new StatisticsViewModel { Month = s.Key, PerMonthCount = s.Value }));
        }
    }
}
