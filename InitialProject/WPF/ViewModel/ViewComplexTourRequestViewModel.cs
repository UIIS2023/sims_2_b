using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class ViewComplexTourRequestViewModel : ViewModelBase
    {

        public static ObservableCollection<ComplexTourRequests> ComplexRequests { get; set; }

        public ComplexTourRequests SelectedRequest { get; set; }
        public User LoggedInUser { get; set; }

        private readonly ComplexTourRequestService _complexService;

        private RelayCommand requestDetails;
        public RelayCommand ViewRequestDetailsCommand
        {
            get => requestDetails;
            set
            {
                if (value != requestDetails)
                {
                    requestDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public delegate void EventHandler1(ComplexTourRequests complexRequest);

        public event EventHandler1 ComplexRequestDetailsEvent;

        public ViewComplexTourRequestViewModel(User user)
        {
            LoggedInUser= user;
            _complexService= new ComplexTourRequestService();
            ComplexRequests = new ObservableCollection<ComplexTourRequests>(_complexService.GetOnHoldAndNotAttended(LoggedInUser));
            ViewRequestDetailsCommand = new RelayCommand(Execute_ViewRequestDetails, CanExecute_Command);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_ViewRequestDetails(object obj)
        {
            ComplexRequestDetailsEvent?.Invoke(SelectedRequest);
        }
    }
}
