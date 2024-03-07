using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public interface IMessageBoxService
    {
        void ShowMessage(string message, string title = "");
        bool ShowConfirmationMessage(string message, string title = "");

    }
}
