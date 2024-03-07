using GalaSoft.MvvmLight.Command;
using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace InitialProject.WPF.ViewModel
{

    public class ComplexTourRequestViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public ICommand CreateComplexTourRequestCommand { get; set; }
        public ICommand AllComplexTourParts { get; set; }

        public delegate void EventHandler1();
        public event EventHandler1 CreateComplexTourRequestEvent;

        public delegate void EventHandler2(ComplexTourRequests SelectedComplexTourRequest);
        public event EventHandler2 AllComplexTourPartsEvent;

        public static ObservableCollection<ComplexTourRequests> ComplexTourRequestsMainList { get; set; }
        public ComplexTourRequests SelectedComplexTourRequest { get; set; }
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly MessageBoxService _messageBoxService;
        private readonly TourRequestService _tourRequestService;
        public ComplexTourRequestViewModel(User user, ComplexTourRequests selectedComplexTourRequest)
        {
            LoggedInUser = user;
            InitializeCommands();
            _complexTourRequestService = new ComplexTourRequestService();
            _tourRequestService = new TourRequestService();
            _messageBoxService = new MessageBoxService();
            SelectedComplexTourRequest = selectedComplexTourRequest;
            ComplexTourRequestsMainList = new ObservableCollection<ComplexTourRequests>(_complexTourRequestService.GetAllByUser(user));

            foreach (ComplexTourRequests complexTourRequest in ComplexTourRequestsMainList)
            {
                SwitchToRejected(complexTourRequest);
                SwitchToAccepted(complexTourRequest);
            }

        }

        private void SwitchToAccepted(ComplexTourRequests complexTourRequest)
        {
            int numAccepted = 0;
            int numRequest = 0;
                foreach (TourRequest tourRequest in _tourRequestService.GetAllTourRequestByComplexRequestId(complexTourRequest.Id))
                {
                    numRequest++;
                    if (tourRequest.Status.Equals(RequestType.Approved))
                    {
                        numAccepted++;
                    }
                }
            if (numAccepted==numRequest)
            {
                complexTourRequest.Status = RequestType.Approved;
                _complexTourRequestService.Update(complexTourRequest);
            }
            
        }

        private void SwitchToRejected(ComplexTourRequests complexTourRequest)
        {
            int numRejected = 0;
            int numRequest = 0;
                foreach (TourRequest tourRequest in _tourRequestService.GetAllTourRequestByComplexRequestId(complexTourRequest.Id))
                {
                    numRequest++;
                    if (tourRequest.Status.Equals(RequestType.Rejected) || tourRequest.Status.Equals(RequestType.RejectedCreated))
                    {
                        numRejected++;
                    }
                }
                if (numRejected==numRequest)
                {
                    complexTourRequest.Status = RequestType.Rejected;
                    _complexTourRequestService.Update(complexTourRequest);
                }
            
        }

        private void InitializeCommands()
        {
            CreateComplexTourRequestCommand = new RelayCommand(Execute_CreateComplexTourRequestCommand, CanExecute_Command);
            AllComplexTourParts = new RelayCommand(Execute_AllComplexTourParts, CanExecute_Command);
        }

        private bool CanExecute_Command()
        {
            return true;
        }

        private void Execute_AllComplexTourParts()
        {
            if(SelectedComplexTourRequest == null)
            {
                _messageBoxService.ShowMessage("You need first to select one complex tour request!");
            }
            else
            {
                AllComplexTourPartsEvent?.Invoke(SelectedComplexTourRequest);
            }
            
        }

        private void Execute_CreateComplexTourRequestCommand()
        {
            CreateComplexTourRequestEvent?.Invoke();
        }
    }
}
