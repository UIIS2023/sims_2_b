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
    public class ComplexTourRequestPartsViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public ICommand CreateComplexTourRequestCommand { get; set; }
        public ICommand AllComplexTourCommand { get; set; }
        public Action CloseAction { get; set; }
        public static ObservableCollection<TourRequest> ComplexTourPartsMainList { get; set; }
        public ComplexTourRequests complexTourRequests { get; set; }
        public readonly TourRequestService _tourRequestService;
        public ComplexTourRequestPartsViewModel(User user, ComplexTourRequests cmplxTourReguest) 
        {
            LoggedInUser = user;
            complexTourRequests = cmplxTourReguest;
            _tourRequestService = new TourRequestService();
            ComplexTourPartsMainList = new ObservableCollection<TourRequest>(_tourRequestService.GetAllTourRequestByComplexRequestId(complexTourRequests.Id));
            CreateComplexTourRequestCommand = new RelayCommand(Execute_CreateComplexTourRequestCommand, CanExecute_Command);
            AllComplexTourCommand = new RelayCommand(Execute_AllComplexTourCommand, CanExecute_Command);

        }

        private void Execute_AllComplexTourCommand()
        {
            CloseAction();
        }

        private bool CanExecute_Command()
        {
            return true;
        }


        private void Execute_CreateComplexTourRequestCommand()
        {
            CreateComplexTourRequest createComplexTourRequest = new CreateComplexTourRequest(LoggedInUser, complexTourRequests);
            createComplexTourRequest.Show();
            CloseAction();
        }
    }
}
