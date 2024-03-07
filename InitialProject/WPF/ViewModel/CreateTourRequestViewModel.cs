using ceTe.DynamicPDF;
using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Action = System.Action;
using ComplexTourRequest = InitialProject.Domain.Model.ComplexTourRequests;

namespace InitialProject.WPF.ViewModel
{
    public class CreateTourRequestViewModel : ViewModelBase
    {
        
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        public User LoggedInUser { get; set; }
        public Action CloseAction { get; set; }
        private int _guestNum;
        public int GuestNum
        {
            get => _guestNum;
            set
            {
                if (value != _guestNum)
                {
                    _guestNum = value;
                    OnPropertyChanged(nameof(GuestNum));
                }
            }
        }
        public static ObservableCollection<String> Countries { get; set; }
        private int requestNumberCopy;
        public ComplexTourRequest complexTourRequest { get; set; }

        public CreateTourRequestViewModel(User user,  ComplexTourRequests cmplxTourRequest)
        {
            LoggedInUser = user;
            _locationService = new LocationService();
            _tourRequestService = new TourRequestService();
            _messageBoxService = new MessageBoxService();
            _complexTourRequestService = new ComplexTourRequestService();
            Countries = new ObservableCollection<String>(_locationService.GetAllCountries());
            Cities = new ObservableCollection<String>();
            
            complexTourRequest = cmplxTourRequest;
            if (complexTourRequest!=null)
            {
                requestNumberCopy =  complexTourRequest.RequestNumber;
            }


            
            SendRequestCommand = new RelayCommand(Execute_SendRequestCommand, CanExecute_SendRequestCommand);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
            NextRequestCommand = new RelayCommand(Execute_NextRequestCommand, CanExecute_NextRequestCommand);
            ViewComplexTourCommand = new RelayCommand(Execute_ViewComplexTourCommand, CanExecute_ViewComplexTourCommand);
        }

        private bool CanExecute_SendRequestCommand(object arg)
        {
            if(complexTourRequest==null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanExecute_ViewComplexTourCommand(object arg)
        {
            if (requestNumberCopy <= 0 && complexTourRequest!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool CanExecute_NextRequestCommand(object arg)
        {
            if(requestNumberCopy <= 0 && complexTourRequest ==null)
            {
                return false;
            }
            return true;
        }
        private void Execute_ViewComplexTourCommand(object obj)
        {
            ComplexTourRequestParts complexTourRequestParts = new ComplexTourRequestParts(LoggedInUser, complexTourRequest);
            complexTourRequestParts.Show();
            CloseAction();
        }

        private void Execute_NextRequestCommand(object obj)
        {
            if (requestNumberCopy <= 0)
            {
                _messageBoxService.ShowMessage("You created all simple tour requests for this complex tour!");
                ((RelayCommand)NextRequestCommand).RaiseCanExecuteChanged();
            }
            else
            {
                TourRequests.NewStartDate = DateOnly.Parse(startDate);
                TourRequests.NewEndDate = DateOnly.Parse(endDate);
                TourRequests.Validate();

                bool cityValid = IsCityValid();
                bool countryValid = IsCountryValid();

                if (TourRequests.IsValid && cityValid && countryValid)
                {
                    CreateNextTourRequestValid();
                }
                else
                {
                    OnPropertyChanged(nameof(TourRequests));
                }
            }
            
        }

        private void CreateNextTourRequestValid()
        {
            Location location = _locationService.FindLocation(SelectedCountry, SelectedCity);

            TourRequest newTourRequest = new TourRequest(location, LoggedInUser.Id, TourRequests.TourLanguage, GuestNum, TourRequests.NewStartDate, TourRequests.NewEndDate, location.Id, TourRequests.Description, complexTourRequest.Id);


            requestNumberCopy--;
            TourRequest savedTour = _tourRequestService.Save(newTourRequest);
            TourRequestsViewModel.TourRequestsMainList.Add(savedTour);
            
            int help = complexTourRequest.RequestNumber - requestNumberCopy;
            string message = "You created " + help + "/"+complexTourRequest.RequestNumber + " simple tour requests";
            string title = "Tracking number of tour requests!";
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBox.Show(message, title, buttons);
            /*
            TourRequests.TourLanguage = "";
            TourRequests.GuestNum = 0;
            TourRequests.Description = "";
            TourRequests.NewStartDate= default;
            TourRequests.NewEndDate= default;*/
            ((RelayCommand)NextRequestCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ViewComplexTourCommand).RaiseCanExecuteChanged();
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
                    Cities = new ObservableCollection<String>(_locationService.GetCities(SelectedCountry));
                    IsCityEnabled = true;
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


        public TourRequest tourRequest = new TourRequest();

        public TourRequest TourRequests
        {
            get { return tourRequest; }
            set
            {
                tourRequest = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private RelayCommand nextRequestCommand;
        public RelayCommand NextRequestCommand
        {
            get => nextRequestCommand;
            set
            {
                if (value != nextRequestCommand)
                {
                    nextRequestCommand = value;
                    TourRequests.TourLanguage = string.Empty;
                    TourRequests.NewEndDate = default;
                    TourRequests.NewStartDate = default;
                    GuestNum = 0;
                    OnPropertyChanged(nameof(TourRequests));
                    
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get => cancelCommand;
            set
            {
                if (value != cancelCommand)
                {
                    cancelCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand viewComplexTourCommand;
        public RelayCommand ViewComplexTourCommand
        {
            get => viewComplexTourCommand;
            set
            {
                if (value != viewComplexTourCommand)
                {
                    viewComplexTourCommand = value;
                    OnPropertyChanged();
                }

            }
        }

        private RelayCommand sendRequestCommand;
        public RelayCommand SendRequestCommand
        {
            get => sendRequestCommand;
            set
            {
                if (value != sendRequestCommand)
                {
                    sendRequestCommand = value;
                    OnPropertyChanged();
                }

            }
        }


        private string _startDate;
        private string _endDate;

        public string startDate
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

        public string endDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public User User { get; }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_CancelCommand(object obj)
        {
            if (complexTourRequest!=null)
            {
                foreach(TourRequest tourRequest in _tourRequestService.GetAll())
                {
                    if(tourRequest.IdComplexTour == complexTourRequest.Id)
                    {
                        _tourRequestService.Delete(tourRequest);
                        
                    }
                }

                TourRequestsViewModel.TourRequestsMainList.Clear();
                foreach(TourRequest tourRequest in _tourRequestService.GetAll())
                {
                    TourRequestsViewModel.TourRequestsMainList.Add(tourRequest);
                }

                _complexTourRequestService.Delete(complexTourRequest);
                ComplexTourRequestViewModel.ComplexTourRequestsMainList.Clear();
                foreach (ComplexTourRequests complexTourRequest in _complexTourRequestService.GetAll())
                {
                    ComplexTourRequestViewModel.ComplexTourRequestsMainList.Add(complexTourRequest);
                }
            }
            CloseAction();
        }

        private bool IsCityValid()
        {
            if (SelectedCity  == null)
            {
                ValidationResult = "City is required";
                return false;
            }
            ValidationResult = "";
            return true;
        }

        private bool IsCountryValid()
        {
            if (SelectedCountry  == null)
            {
                ValidationResult2 = "Country is required";
                return false;
            }
            ValidationResult2 = "";
            return true;
        }

        private void Execute_SendRequestCommand(object obj)
        {
            TourRequests.NewStartDate = DateOnly.Parse(startDate);
            TourRequests.NewEndDate = DateOnly.Parse(endDate);
            TourRequests.Validate();

            bool cityValid = IsCityValid();
            bool countryValid = IsCountryValid();

            if (TourRequests.IsValid && cityValid && countryValid)
            {
                CreateTourRequestValid();
            }
            else
            {
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private void CreateTourRequestValid()
        {
            Location location = _locationService.FindLocation(SelectedCountry, SelectedCity);

            TourRequest newTourRequest = new TourRequest(location, LoggedInUser.Id, TourRequests.TourLanguage, GuestNum, TourRequests.NewStartDate, TourRequests.NewEndDate, location.Id, TourRequests.Description, 0);



             TourRequest savedTour = _tourRequestService.Save(newTourRequest);
             TourRequestsViewModel.TourRequestsMainList.Add(savedTour);

            CloseAction();
        }
    }
}
