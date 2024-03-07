using InitialProject.Commands;
using InitialProject.Applications.DTO;
using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class Whenever_WhereverViewModel: BindableBase

    {
        public Action CloseAction { get; set; }
        private DateOnly startDate1;
        private DateOnly endDate1;
        public List<DateOnly> reservedDates { get; set; }

        private readonly IMessageBoxService _messageBoxService;

        public User LoggedInUser { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        private readonly AccommodationReservationService accommodationReservationService;
        private readonly AccommodationService accommodationService;

        public Whenever_WhereverViewModel(User user, Accommodation acc)
        {
            LoggedInUser = user;
            accommodationReservationService = new AccommodationReservationService();
            InitializeCommands();
            startDate = DateTime.Today;
            endDate = DateTime.Today;
            SelectedAccommodation = acc;
            _messageBoxService = new MessageBoxService();
            accommodationService = new AccommodationService();

        }
        private RelayCommand cancelCreate;
        public RelayCommand CancelCreate
        {
            get { return cancelCreate; }
            set
            {
                cancelCreate = value;
            }
        }
        private RelayCommand checkAcc;
        public RelayCommand CheckAccommodation
        {
            get { return checkAcc; }
            set
            {
                checkAcc = value;
            }
        }
        private RelayCommand reserveAcc;
        public RelayCommand ReserveAccommodation
        {
            get { return reserveAcc; }
            set
            {
                reserveAcc = value;
            }
        }
        private void InitializeCommands()
        {
            CancelCreate = new RelayCommand(Execute_CancelCreate, CanExecute_Command);
            CheckAccommodation = new RelayCommand(Execute_CheckAccommodation, CanExecute_Command);
            // CheckAvailableDate = new RelayCommand(Execute_CheckAvailableDate, CanExecute_Command);
            ReserveAccommodation = new RelayCommand(Execute_ReserveAccommodation, CanExecute_Command);
        }

        private void Execute_ReserveAccommodation(object obj)
        {
            if (selectedPeriod != null)
            {
                SelectedAccommodation=accommodationService.GetAccommodationByName(selectedPeriod.Name);
                AccommodationReservation newReservation = new(LoggedInUser, LoggedInUser.Id, SelectedAccommodation, SelectedAccommodation.Id, SelectedPeriod.StartDate, SelectedPeriod.EndDate, int.Parse(TxtDaysNum), false);

                AccommodationReservation savedReservation = accommodationReservationService.Save(newReservation);
                Guest1MainWindowViewModel.AccommodationsReservationList.Add(savedReservation);
                AvailablePeriods.Remove(selectedPeriod);

                CloseAction();
            }
            else
            {
                _messageBoxService.ShowMessage("Morate prvo selektovati smestaj koji zelite da rezervisete!");
            }

        }

        private void Execute_CheckAccommodation(object obj)
        {
            startDate1 = DateOnly.FromDateTime(startDate); 

            endDate1 = DateOnly.FromDateTime(endDate);
            AvailablePeriods = new ObservableCollection<ReservationPeriodDTO>(accommodationReservationService.GetAvailableAccommodations(startDate1, endDate1, int.Parse(TxtDaysNum),int.Parse(TxtNumGuest)));


        }

        private void Execute_CancelCreate(object obj)
        {

            CloseAction();


        }

        private ObservableCollection<ReservationPeriodDTO> availablePeriods;
        public ObservableCollection<ReservationPeriodDTO> AvailablePeriods
        {
            get => availablePeriods;
            set
            {
                if (value != availablePeriods)
                {
                    availablePeriods = value;
                    OnPropertyChanged(nameof(AvailablePeriods));
                }
            }
        }
        private string _txtGuestNum { get; set; }
        public string TxtNumGuest
        {
            get { return _txtGuestNum; }
            set
            {
                if (_txtGuestNum != value)
                {
                    _txtGuestNum = value;
                    OnPropertyChanged("_txtGuestNum");
                }
            }
        }
        private string _txtReservationNum { get; set; }
        public string TxtDaysNum
        {
            get { return _txtReservationNum; }
            set
            {
                if (_txtReservationNum != value)
                {
                    _txtReservationNum = value;
                    OnPropertyChanged("_txtReservationNum");
                }
            }
        }


        private DateOnly _startDate;
        private DateOnly _endDate;


        public DateTime startDate
        {
            get => _startDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != _startDate.ToDateTime(TimeOnly.MinValue))
                {
                    _startDate = DateOnly.FromDateTime(value.Date);
                    OnPropertyChanged(nameof(startDate));
                }
            }
        }

        public DateTime endDate
        {
            get => _endDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != _endDate.ToDateTime(TimeOnly.MinValue))
                {
                    _endDate = DateOnly.FromDateTime(value.Date);
                    OnPropertyChanged(nameof(endDate));
                }
            }
        }


        private ReservationPeriodDTO selectedPeriod;
        public ReservationPeriodDTO SelectedPeriod
        {
            get => selectedPeriod;
            set
            {
                if (value != selectedPeriod)
                {
                    selectedPeriod = value;
                    OnPropertyChanged(nameof(SelectedPeriod));
                }
            }
        }




        private string _minDaysReservation { get; set; }
        public string DaysNum
        {
            get => _minDaysReservation;
            set
            {
                if (value != _minDaysReservation)
                {
                    _minDaysReservation = value;
                    OnPropertyChanged(nameof(DaysNum));
                }
            }
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
    }
}
