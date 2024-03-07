using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
   
    public class GuideMainWindowViewModel :ViewModelBase
    {
        public Tour SelectedTour { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourService _tourService;
        private readonly MessageBoxService _messageBoxService;

        public static ObservableCollection<Tour> Tours { get; set; }

        private RelayCommand create;
        public RelayCommand CreateCommand
        {
            get { return create; }
            set
            {
                create = value;
            }
        }

        private RelayCommand tracking;
        public RelayCommand TourTrackingCommand
        {
            get { return tracking; }
            set
            {
                tracking = value;
            }
        }

        private RelayCommand multiply;
        public RelayCommand MultiplyCommand
        {
            get { return multiply; }
            set
            {
                multiply = value;
            }
        }

        private RelayCommand viewGallery;
        public RelayCommand ViewGalleryCommand
        {
            get { return viewGallery; }
            set
            {
                viewGallery = value;
            }
        }


        private RelayCommand cancel;
        public RelayCommand CancelCommand
        {
            get { return cancel; }
            set
            {
                cancel = value;
            }
        }

        public delegate void EventHandler1(Tour tour);
        public delegate void EventHandler2(Tour tour);
        public event EventHandler1 MultiplyEvent;
        public event EventHandler2 ViewGalleryEvent;


        public GuideMainWindowViewModel(User user)
        {
            LoggedInUser = user;
            _tourService = new TourService();
            _messageBoxService = new MessageBoxService();
            Tours = new ObservableCollection<Tour>(_tourService.GetUpcomingToursByUser(LoggedInUser));

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            MultiplyCommand = new RelayCommand(Execute_Multiply, CanExecute_Command);
            ViewGalleryCommand = new RelayCommand(Execute_ViewGallery, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_Cancel, CanExecute_Command);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_Multiply(object obj)
        {
            if (SelectedTour != null)
            {
                MultiplyEvent?.Invoke(SelectedTour);
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a tour which you want to multiply");
            }

        }

        private void Execute_ViewGallery(object obj)
        {
            if(SelectedTour != null)
            {
                ViewGalleryEvent?.Invoke(SelectedTour);
            }
            else
            {
                _messageBoxService.ShowMessage("To view tour gallery, please choose a tour");
            }
        }

        private void Execute_Cancel(object obj)
        {
            if (_tourService.IsCancellationPossible(SelectedTour))
            {
                _tourService.CancelTour(SelectedTour);
                Tours.Remove(SelectedTour);
            }
            else
            {
                _messageBoxService.ShowMessage("You can't cancel tour less then 48h before");
            }
        }

    }
}
