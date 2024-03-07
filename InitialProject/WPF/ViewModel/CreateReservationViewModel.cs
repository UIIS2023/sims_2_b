using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.Validations;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class CreateReservationViewModel : BindableBase
    {
        public Accommodation SelectedAccommodation;
        public AccommodationReservation accommodationReservation = new AccommodationReservation();
        public AccommodationReservation accommodationRes { get; set; }
        public AccommodationReservation reservation { get; set; }
        public List<DateOnly> StartDates;
        public List<DateOnly> EndDates;
        public List<DateOnly> BetweenDates;
        private DateOnly startDate1;
        private DateOnly endDate1;
        private int minReservation = 0;
        private readonly AccommodationReservationService accommodationReservationService;
        public Action CloseAction { get; set; }
        private readonly IMessageBoxService _messageBoxService;
        public SuperGuest _superGuest { get; set; }
        private readonly SuperGuestService superGuestService;


        public User LoggedInUser { get; set; }




        public CreateReservationViewModel(Accommodation selectedAccommodation, User user, AccommodationReservation selectedReservation, IMessageBoxService messageBoxService)
        {
            LoggedInUser = user;
            accommodationReservationService = new AccommodationReservationService();
            _messageBoxService = messageBoxService;
            accommodationRes = selectedReservation;
            InitializeProperties(selectedAccommodation, user, accommodationRes);
            superGuestService = new SuperGuestService();
            _superGuest = superGuestService.GetSuperGuest(LoggedInUser.Id);
            InitializeCommands();


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

        private RelayCommand checkAvailableDate;
        public RelayCommand CheckAvailableDate
        {
            get { return checkAvailableDate; }
            set
            {
                checkAvailableDate = value;
            }
        }

        private RelayCommand checkGuestNumber;
        public RelayCommand CheckGuestNumber
        {
            get { return checkGuestNumber; }
            set
            {
                checkGuestNumber = value;
            }
        }

        private RelayCommand reserveAccommodation;
        public RelayCommand ReserveAccommodation
        {
            get { return reserveAccommodation; }
            set
            {
                reserveAccommodation = value;
            }
        }

        private void InitializeCommands()
        {
            CancelCreate = new RelayCommand(Execute_CancelCreate, CanExecute_Command);
            CheckGuestNumber = new RelayCommand(Execute_CheckGuestNumber, CanExecute_Command);
            CheckAvailableDate = new RelayCommand(Execute_CheckAvailableDate, CanExecute_Command);
            ReserveAccommodation = new RelayCommand(Execute_ReserveAccommodation, CanExecute_Command);
        }

        private void Execute_ReserveAccommodation(object obj)
        {
            StartDates.Clear();
            EndDates.Clear();
            BetweenDates.Clear();


            AccommodationReservation newReservation = new(LoggedInUser, LoggedInUser.Id, SelectedAccommodation, SelectedAccommodation.Id, AccommodationReservations.StartDate, AccommodationReservations.EndDate, int.Parse(TxtDaysNum),false);

            AccommodationReservation savedReservation = accommodationReservationService.Save(newReservation);
            Guest1MainWindowViewModel.AccommodationsReservationList.Add(savedReservation);



            
            Guest1ProfilViewModel.bonusPoints = Guest1ProfilViewModel.bonusPoints - 1;

            if (LoggedInUser.IsSuper)
            {
                _superGuest.Bonus = (Guest1ProfilViewModel.bonusPoints).ToString();
                superGuestService.Update(_superGuest);
            }





            CloseAction();


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

        private void Execute_CheckAvailableDate(object obj)
        {
           
            AccommodationReservations.StartDate = DateOnly.FromDateTime(startDate);
            AccommodationReservations.EndDate = DateOnly.FromDateTime(endDate);
            AccommodationReservations.Validate();
            if (AccommodationReservations.IsValid)
            {

                if (!(int.TryParse(TxtDaysNum, out minReservation) || (TxtDaysNum.Equals(""))))
                {

                    return;
                }

                if ((minReservation - SelectedAccommodation.MinReservationDays) >= 0)
                {

                }
                else
                {
                    _messageBoxService.ShowMessage("Prekrsili ste broj minimalnih dana za rezervaciju.");
                    return;

                }



                StartDates.Clear();
                EndDates.Clear();


                // 1-30jula 5  startDate1=1 endDate1=30 jul
                // 1 , 4 , 10
                // 6 , 12, 17

                startDate1 = DateOnly.FromDateTime(startDate); // ovo sam radila da bih mogla da poredim sa datumima iz csva koji su upisani kao date only

                endDate1 = DateOnly.FromDateTime(endDate);

                StartDates = accommodationReservationService.GetAllStartDates(SelectedAccommodation.Id);  // svi pocetni iz csv
                EndDates = accommodationReservationService.GetAllEndDates(SelectedAccommodation.Id); //svi krajnji iz csv
                List<DateOnly> freeDates = GetFreeDates(startDate1, endDate1, StartDates, EndDates, minReservation);

                BetweenDates.Clear();
                for (DateOnly date = startDate1; date <= endDate1; date = date.AddDays(1))
                {
                    BetweenDates.Add(date);
                } // svi datumi izmedju pocetnog i krajnjeg


                GetDateByCondition(freeDates);


            }
            
        }

        private void GetDateByCondition(List<DateOnly> freeDates)
        {
            if (freeDates.Count == 0 && StartDates.Count != 0 && EndDates.Count != 0)
            {
                IsEnabledGuestNumber = true;
                _messageBoxService.ShowMessage("Nema slobodnih datuma u vasem opsegu. Alternative su: ");
                AlternativeDates(endDate1, StartDates, EndDates, minReservation);
                return;
            }

            else if (freeDates.Count == 0 && StartDates.Count == 0)
            {
                IsEnabledGuestNumber = true;
                _messageBoxService.ShowMessage("Mozete izabrati koji god zelite datum u ovom opsegu");
            }
            else
            {

                IsEnabledGuestNumber = true;
                string message = "";
                foreach (DateOnly item in freeDates)
                {
                    message += item + "\n";
                }

                _messageBoxService.ShowMessage(message, "Free Dates");

                freeDates.Clear();
            }
        }

        public void AlternativeDates(DateOnly endDate, List<DateOnly> startDates, List<DateOnly> endDates, int numDays)
        {
            List<DateOnly> alternativeFreeDates = new List<DateOnly>();
            alternativeFreeDates = GetFreeDates(endDate, endDate.AddDays(30), startDates, endDates, numDays);
            string message = "";
            int i = 0;
            foreach (DateOnly item in alternativeFreeDates)
            {
                if (i < numDays)
                {
                    message += item + "\n";
                    i++;
                }
                else
                {
                    _messageBoxService.ShowMessage(message, "Alternative days");
                    return;
                }
            }


        }

        public List<DateOnly> GetFreeDates(DateOnly startDate, DateOnly endDate, List<DateOnly> startDates, List<DateOnly> endDates, int numDays)
        {
            List<DateOnly> freeDates = new List<DateOnly>();
            if (startDates.Count != 0)
            {
                DateOnly earliestStartDate = startDate < startDates.Min() ? startDate : startDates.Min();
                DateOnly latestEndDate = endDate > endDates.Max() ? endDate : endDates.Max();

                for (DateOnly currentDate = earliestStartDate; currentDate <= latestEndDate; currentDate = currentDate.AddDays(1))
                {
                    bool isBooked = false;

                    // Skip the current date if it falls before the startDate or after the endDate
                    if (currentDate < startDate || currentDate > endDate)
                        continue;

                    isBooked = GetSequenceFreeDates(startDates, endDates, numDays, currentDate, isBooked);

                    if (!isBooked)
                    {
                        freeDates.Add(currentDate);
                    }
                }
            }
            return freeDates;
        }


        private static bool GetSequenceFreeDates(List<DateOnly> startDates, List<DateOnly> endDates, int numDays, DateOnly currentDate, bool isBooked)
        {
            for (int i = 0; i < startDates.Count; i++)
            {
                DateOnly bookedStartDate = startDates[i];
                DateOnly bookedEndDate = endDates[i];

                if (currentDate >= bookedStartDate && currentDate <= bookedEndDate)
                {
                    isBooked = true;
                    break;
                }

                if (currentDate.AddDays(numDays) >= bookedStartDate && currentDate.AddDays(numDays) <= bookedEndDate)
                {
                    isBooked = true;
                    break;
                }
            }

            return isBooked;
        }


        private void Execute_CheckGuestNumber(object obj)
        {
            int maxGuest = 0;
            if (!(int.TryParse(TxtNumGuest, out maxGuest) || (TxtNumGuest.Equals(""))))
            {
                return;
            }

            if ((maxGuest - SelectedAccommodation.MaxGuestNum) <= 0 && minReservation == BetweenDates.Count())
            {
                BlockedButton = true;
            }
            else
            {
                BlockedButton = false;
                _messageBoxService.ShowMessage("Prekrsili ste maksimalan broj gostiju za rezervaciju ili pokusavate da rezervisete razlicit broj dana od navedenog!");
            }
        }

        private void Execute_CancelCreate(object obj)
        {

            CloseAction();


        }

        private void InitializeProperties(Accommodation selectedAccommodation, User user, AccommodationReservation selectedReservation)
        {
            StartDates = new List<DateOnly>();
            EndDates = new List<DateOnly>();
            BetweenDates = new List<DateOnly>();
            SelectedAccommodation = selectedAccommodation;
            LoggedInUser = user;
            IsEnabledGuestNumber = false;
            BlockedButton = false;
            startDate = DateTime.Today;
            endDate = DateTime.Today;
           

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

        private bool _isenabledGuestNumber { get; set; }
        public bool IsEnabledGuestNumber
        {
            get => _isenabledGuestNumber;
            set
            {
                if (value != _isenabledGuestNumber)
                {
                    _isenabledGuestNumber = value;
                    OnPropertyChanged(nameof(IsEnabledGuestNumber));
                }
            }
        }

        private bool _blockedButton { get; set; }
        public bool BlockedButton
        {
            get => _blockedButton;
            set
            {
                if (value != _blockedButton)
                {
                    _blockedButton = value;
                    OnPropertyChanged(nameof(BlockedButton));
                }
            }
        }
        public AccommodationReservation AccommodationReservations
        {
            get { return accommodationReservation; }
            set
            {
                accommodationReservation = value;
                OnPropertyChanged("AccommodationReservations");
            }
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
    }
}