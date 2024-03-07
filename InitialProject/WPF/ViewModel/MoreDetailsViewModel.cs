using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class MoreDetailsViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public TourRequest SelectedTourRequest { get; set; }
        public static User User { get; set; }
        public ICommand BackCommand { get; set; }
       
        public MoreDetailsViewModel(User user, TourRequest tourRequest) 
        {
            User = user;
            SelectedTourRequest = tourRequest;
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecute_Command);
        }

        private void Execute_BackCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }
    }
}
