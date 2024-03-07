using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class Guest2AccountViewModel:ViewModelBase
    {
        public User LoggedInUser { get; set; }
        public string ImageSource { get; set; }
        public string UserImageSource { get; set; }
        public ICommand LogOutCommand { get; set; }
        
        private readonly UserService userService;

        public delegate void EventHandler1();

        public event EventHandler1 LogOutEvent;

        public Guest2AccountViewModel(User user) 
        {
            LoggedInUser = user;
            userService = new UserService();
            LogOutCommand =  new RelayCommand(Execute_LogOutCommand, CanExecute_Command);
            SetImagesSource(user);
        }

        private void Execute_LogOutCommand(object obj)
        {

            LogOutEvent?.Invoke();
        }


        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        public void SetImagesSource(User user)
        {
            UserImageSource = userService.GetImageUrlByUserId(user.Id);
        }

    }
}
