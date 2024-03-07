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
    public class ChangeReservationDateViewModel: BindableBase
    {
        public Action CloseAction;

        public User LogedUser;

        public List<DateOnly> SelectedDates;

        public DateOnly startDate1;

        public DateOnly endDate1;

        private readonly IMessageBoxService _messageBoxService;
        public AccommodationReservation SelectedReservation { get; set; }

        public ReservationDisplacementRequest SelectedRequest;

        public ReservationDisplacementRequest ReservationDisplacementRequest=new ReservationDisplacementRequest();
        private readonly ReservationDisplacementRequestService reservationDisplacementRequest;
        
        public ChangeReservationDateViewModel(User user,AccommodationReservation reservation,ReservationDisplacementRequest request, IMessageBoxService messageBoxService)
        {
            LogedUser = user;
            SelectedReservation = reservation;
            SelectedRequest= request;
            SelectedDates= new List<DateOnly>();
            reservationDisplacementRequest = new ReservationDisplacementRequestService();
            startDate = DateTime.Today;
            endDate = DateTime.Today;
            _messageBoxService = messageBoxService;
            InitializeCommands();

        }

        private void InitializeCommands()
        {
            CancelChange = new RelayCommand(Execute_CancelChange, CanExecute_Command);
            SendRequest = new RelayCommand(Execute_SendRequest, CanExecute_Command);
            
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

        private void Execute_SendRequest(object obj)
        {
            if (SelectedReservation != null)
            {
                ReservationDisplacement.NewStartDate = DateOnly.FromDateTime(startDate);
                ReservationDisplacement.NewEndDate = DateOnly.FromDateTime(endDate);
                ReservationDisplacement.Validate();
            if (ReservationDisplacement.IsValid)
                {
                    FillSelectedDatesList();
                    if (SelectedReservation.DaysNum == SelectedDates.Count)

                    {
                        string? comment = null;
                        ReservationDisplacementRequest newReservation = new ReservationDisplacementRequest(SelectedReservation, SelectedReservation.Id, default(RequestType), ReservationDisplacement.NewStartDate, ReservationDisplacement.NewEndDate, LogedUser.Id, comment);
                        ReservationDisplacementRequest savedReservation = reservationDisplacementRequest.Save(newReservation);
                        Guest1MainWindowViewModel.RequestsList.Add(savedReservation);
                        CloseAction();
                    }
                    else
                    {
                        _messageBoxService.ShowMessage("Morate uneti " + SelectedReservation.DaysNum + " dana!");
                    }
                }
            }
           
            
        }
         private void FillSelectedDatesList()
        {
            SelectedDates.Clear();
            startDate1 = DateOnly.FromDateTime(startDate);
            endDate1 = DateOnly.FromDateTime(endDate);

            for (DateOnly date = startDate1; date <= endDate1; date = date.AddDays(1))
            {
                SelectedDates.Add(date);
            }
        }
        private void Execute_CancelChange(object obj)
        {
            CloseAction();
        }

        private RelayCommand cancelChange;
        public RelayCommand CancelChange
        {
            get { return cancelChange; }
            set
            {
                cancelChange = value;
            }
        }

        private RelayCommand sendRequest;
        public RelayCommand SendRequest
        {
            get { return sendRequest; }
            set
            {
                sendRequest = value;
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


        

        public ReservationDisplacementRequest ReservationDisplacement
        {
            get { return ReservationDisplacementRequest; }
            set
            {
                ReservationDisplacementRequest = value;
                OnPropertyChanged("ReservationDisplacement");
            }
        }


    }
}
