using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class RequestFilterViewModel : ViewModelBase
    {
        private readonly ILocationRepository _locationRepository;


        private RelayCommand filter;
        public RelayCommand FilterResultCommand
        {
            get => filter;
            set
            {
                if (value != filter)
                {
                    filter = value;
                    OnPropertyChanged();
                }

            }
        }

        public static ObservableCollection<String> Countries { get; set; }

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

        private bool _isCityEnabled;
        public bool IsCityEnabled
        {
            get { return _isCityEnabled; }
            set
            {
                _isCityEnabled = value;
                OnPropertyChanged(nameof(IsCityEnabled));
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
                    if (Cities.Count == 0)
                    {
                        IsCityEnabled = false;
                    }
                    else
                    {
                        IsCityEnabled = true;
                    }
                    OnPropertyChanged(nameof(Cities));
                    OnPropertyChanged(nameof(SelectedCountry));
                    OnPropertyChanged(nameof(IsCityEnabled));
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _maxGuestNum;
        public string MaxGuestNum 
        {
            get => _maxGuestNum;
            set
            {
                if (value != _maxGuestNum)
                {
                    _maxGuestNum = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _startDate;
        public string StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }


        private string _endDate;
        public string EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }


        public delegate void EventHandler1();
        public event EventHandler1 DoneFilteringEvent;

        public RequestFilterViewModel() 
        {
            FilterResultCommand = new RelayCommand(Execute_FilterResult, CanExecute_Command);
            _locationRepository = Inject.CreateInstance<ILocationRepository>();
            Countries = new ObservableCollection<String>(_locationRepository.GetAllCountries());
            Cities = new ObservableCollection<String>();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_FilterResult(object obj)
        {
            AcceptTourRequestViewModel.Requests.Clear();

            foreach(TourRequest request in AcceptTourRequestViewModel.RequestsCopyList)
            {
                Comparison( request);
            }
            DoneFilteringEvent?.Invoke();
        }

        private void Comparison(TourRequest request)
        {
            if ((request.Location.Country == SelectedCountry || SelectedCountry == null) && (request.Location.City == SelectedCity || SelectedCity == null) && 
                request.TourLanguage.ToLower().Contains(Language) && (MaxGuestNum == null || request.GuestNum - int.Parse(MaxGuestNum) >= 0) && DatesComparison(request))
            {
                AcceptTourRequestViewModel.Requests.Add(request);
            }
        }

        private bool DatesComparison(TourRequest request)
        {
            if(StartDate != null || EndDate != null)
            {
                DateTime start = DateTime.Parse(StartDate);
                DateTime end = DateTime.Parse(EndDate);
                if (request.NewStartDate.CompareTo(DateOnly.FromDateTime(start.Date)) >= 0 && request.NewEndDate.CompareTo(DateOnly.FromDateTime(end.Date)) <= 0)
                {
                    return true;
                }
                return false;
            }
            return true; 
        }
    }
}
