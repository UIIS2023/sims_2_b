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
    public class CreateComplexTourRequestViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public Action CloseAction { get; set; }
        public ComplexTourRequests complexTourRequest { get; set; }
        public ICommand CreateSimpleRequestCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private readonly ComplexTourRequestService _complexTourRequestService;

        private int requestNumber { get; set; }
        public int RequestNumber
        {
            get { return requestNumber; }
            set
            {
                if (requestNumber != value)
                {
                    requestNumber = value;
                    OnPropertyChanged(nameof(RequestNumber));
                    ((RelayCommand)CreateSimpleRequestCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public CreateComplexTourRequestViewModel(User user, ComplexTourRequests cmplxTourRequests)
        {
            LoggedInUser = user;
            _complexTourRequestService = new ComplexTourRequestService();
            complexTourRequest = cmplxTourRequests;
            CreateSimpleRequestCommand = new RelayCommand(Execute_CreateSimpleRequestCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_AllComplexRequestCommand, CanExecute_AllComplexRequestCommand);
        }

        private bool CanExecute_AllComplexRequestCommand()
        {
            return true;
        }


        private bool CanExecute_Command()
        {
            if (RequestNumber > 0)
            {
                return true;
            }

            return false;
        }



        private void Execute_AllComplexRequestCommand()
        {
            CloseAction();
        }

        private void Execute_CreateSimpleRequestCommand()
        {
            ComplexTourRequests newComplexTourRequests = new ComplexTourRequests(requestNumber, RequestType.OnHold, LoggedInUser.Id);
            ComplexTourRequests savedComplexTourRequest = _complexTourRequestService.Save(newComplexTourRequests);
            ComplexTourRequestViewModel.ComplexTourRequestsMainList.Add(savedComplexTourRequest);

            CreateTourRequest createTourRequest = new CreateTourRequest(LoggedInUser, RequestNumber, savedComplexTourRequest);
            createTourRequest.Show();

            CloseAction();
        }
    }
}
