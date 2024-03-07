using GalaSoft.MvvmLight.Command;
using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class ChooseAvailableDateViewModel : ViewModelBase
    {
        public DateTime StartInterval { get; set; }
        public DateTime EndInterval { get; set; }
        public TourRequest SelectedRequest { get; set; }

        private string _date;
        public string Date
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }


        private RelayCommand check;
        public RelayCommand CheckAvailabilityCommand
        {
            get => check;
            set
            {
                if (value != check)
                {
                    check = value;
                    OnPropertyChanged();
                }

            }
        }

        public User LoggedInUser { get; set; }

        private readonly TourService _tourService;
        private readonly TourRequestService _tourRequestService;
        private readonly MessageBoxService _messageBoxService;

        public delegate void EventHandler1();
        public event EventHandler1 BackToComplex;

        public Action CloseAction { get; set; }

        public ChooseAvailableDateViewModel(TourRequest request, User user)
        {
            SelectedRequest= request;
            LoggedInUser = user;
            StartInterval = new DateTime(SelectedRequest.NewStartDate.Year, SelectedRequest.NewStartDate.Month, SelectedRequest.NewStartDate.Day);
            EndInterval = new DateTime(SelectedRequest.NewEndDate.Year, SelectedRequest.NewEndDate.Month, SelectedRequest.NewEndDate.Day);

            _tourService = new TourService();
            _tourRequestService = new TourRequestService();
            _messageBoxService = new MessageBoxService();

            CheckAvailabilityCommand = new RelayCommand(Execute_CheckAvailability, CanExecute_Command);
        }

        private bool CanExecute_Command()
        {
            return true;
        }

        private void Execute_CheckAvailability()
        {
            if (!_tourService.IsUserFree(LoggedInUser, DateOnly.Parse(Date)))
            {
                _messageBoxService.ShowMessage("You are not free on this date");
                return;
            }

            if(!_tourRequestService.IsDateFree(SelectedRequest, DateOnly.Parse(Date)))
            {
                _messageBoxService.ShowMessage("Other simple tour of this complex tour is already at the same date");
                return;
            }


           if( _messageBoxService.ShowConfirmationMessage("Date is available, do you want to accept this tour?"))
            {
                SelectedRequest.Status = RequestType.Approved;
                SelectedRequest.IdGuide = LoggedInUser.Id;
                _tourRequestService.Update(SelectedRequest);
                BackToComplex?.Invoke();
                CloseAction();

            }
            else
            {
                BackToComplex?.Invoke();
                CloseAction();
            }
           
        }
    }
}
