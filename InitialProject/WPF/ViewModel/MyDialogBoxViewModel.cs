using InitialProject.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
	internal class MyDialogBoxViewModel : ViewModelBase
	{
        private string _userInput;
        public string UserInput
        {
            get { return _userInput; }
            set { _userInput = value; OnPropertyChanged(); }
        }

        public RelayCommand OKCommand { get; }

       

        public MyDialogBoxViewModel()
        {
            OKCommand = new RelayCommand(Execute_OK, CanExecute_Command);
        }

        private void Execute_OK(object sender)
        {
            CloseDialogWithResult(true);
        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
        }



        private void CloseDialogWithResult(bool result)
        {
           
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (window != null)
            {
                window.DialogResult = result;
                window.Close();
            }
        }
    }
}
