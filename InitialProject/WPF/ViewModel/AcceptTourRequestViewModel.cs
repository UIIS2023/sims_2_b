using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class AcceptTourRequestViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public static ObservableCollection<TourRequest> Requests { get; set; }
        public static ObservableCollection<TourRequest> RequestsCopyList { get; set; }

        public TourRequest SelectedRequest { get; set; }

        private readonly TourRequestService _tourRequestService;


        private RelayCommand filter;
        public RelayCommand FilterCommand
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

        private RelayCommand accept;
        public RelayCommand AcceptCommand
        {
            get => accept;
            set
            {
                if (value != accept)
                {
                    accept = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand statistics;
        public RelayCommand StatisticsCommand
        {
            get => statistics;
            set
            {
                if (value != statistics)
                {
                    statistics = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private RelayCommand locationBased;
        public RelayCommand CreateBasedOnLocation
        {
            get => locationBased;
            set
            {
                if (value != locationBased)
                {
                    locationBased = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand languageBased;
        public RelayCommand CreateBasedOnLanguage
        {
            get => languageBased;
            set
            {
                if (value != languageBased)
                {
                    languageBased = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand reset;
        public RelayCommand ResetCommand
        {
            get => reset;
            set
            {
                if (value != reset)
                {
                    reset = value;
                    OnPropertyChanged();
                }
            }
        }



        public delegate void EventHandler1();
        public delegate void EventHandler2(TourRequest request);
        public delegate void EventHandler3();
        public delegate void EventHandler4();
        public delegate void EventHandler5(string location);
        public delegate void EventHandler6(string language);

        public event EventHandler1 FilterEvent;
        public event EventHandler2 AcceptEvent;
        public event EventHandler3 StatistcisEvent;
        public event EventHandler4 CreateRequestEvent;
        public event EventHandler5 CreateOnLocation;
        public event EventHandler6 CreateOnLanguage;

        public string TopLocation { get; set; }
        public string TopLanguage { get; set; }


        public AcceptTourRequestViewModel(User user)
        {
            LoggedInUser = user;
            _tourRequestService= new TourRequestService();
            Requests = new ObservableCollection<TourRequest>(_tourRequestService.GetAllUnaccepted());
            RequestsCopyList = new ObservableCollection<TourRequest>(_tourRequestService.GetAllUnaccepted());

            FilterCommand = new RelayCommand(Execute_Filter, CanExecute_Command);
            AcceptCommand = new RelayCommand(Execute_Accept, CanExecute_Command);
            StatisticsCommand = new RelayCommand(Execute_Statistics, CanExecute_Command);
            CreateRequestCommand = new RelayCommand(Execute_CreateRequest, CanExecute_Command);
            CreateBasedOnLocation = new RelayCommand(Execute_CreateLocationBased, CanExecute_Command);
            CreateBasedOnLanguage = new RelayCommand(Execute_CreateLanguageBased, CanExecute_Command);
            ResetCommand = new RelayCommand(Execute_Reset, CanExecute_Command);
            TopLocation = _tourRequestService.GetTopLocation();
            TopLanguage = _tourRequestService.GetTopLanguage();
        }

        private void Execute_Reset(object obj)
        {
            Requests.Clear();
            foreach(TourRequest tr in _tourRequestService.GetAll())
            {
                Requests.Add(tr);
            }
        }

        private void Execute_CreateLanguageBased(object obj)
        {
            CreateOnLanguage?.Invoke(TopLanguage);
        }

        private void Execute_CreateLocationBased(object obj)
        {
            CreateOnLocation?.Invoke(TopLocation);
        }

        private void Execute_CreateRequest(object obj)
        {
            CreateRequestEvent?.Invoke();
        }

        private void Execute_Statistics(object obj)
        {
            StatistcisEvent?.Invoke();
        }

        private void Execute_Accept(object obj)
        {
            AcceptEvent?.Invoke(SelectedRequest);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_Filter(object obj)
        {
            FilterEvent?.Invoke();
        }
    }
}
