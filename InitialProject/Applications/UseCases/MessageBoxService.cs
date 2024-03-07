using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Applications.UseCases
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessage(string message, string title = "")
        {
            MessageBox.Show(message, title);
        }

        public  bool ShowConfirmationMessage(string message, string title = "")
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }

}
