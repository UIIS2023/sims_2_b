using InitialProject.Applications.DTO;
using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Validations;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace InitialProject.WPF.ViewModel
{
	public class RenovationViewModel : BindableBase
	{
		public static User LoggedInUser { get; set; }

        public List<DateOnly> reservedDates { get; set; }

        public List<DateOnly> renovationDates { get; set; }


		public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public static ObservableCollection<Renovation> AllRenovations { get; set; }

        public Renovation renovation = new Renovation();

		private readonly AccommodationService accommodationService;

        private readonly RenovationService renovationService;

        private readonly AccommodationReservationService accommodationReservationService;

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
            cfg.DisplayOptions.Width = 350;
        });


        private ObservableCollection<RenovationPeriodDTO> availablePeriods;
        public ObservableCollection<RenovationPeriodDTO> AvailablePeriods
        {
            get => availablePeriods;
            set
            {
                if (value != availablePeriods)
                {
                    availablePeriods = value;
                    OnPropertyChanged(nameof(AvailablePeriods));
                }
            }
        }

        

        private DateTime? _startDate;
        public DateTime? startDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(startDate));
                }
            }
        }

        private DateTime? _endDate;
        public DateTime? endDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(endDate));
                }
            }
        }
      
    

        private Accommodation selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get => selectedAccommodation;
            set
            {
                if (value != selectedAccommodation)
                {
                    selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private RelayCommand checkCommand;
        public RelayCommand CheckCommand
        {
            get { return checkCommand; }
            set
            {
                checkCommand = value;
            }
        }

        private RelayCommand confirmCommand;
        public RelayCommand ConfirmCommand
        {
            get { return confirmCommand; }
            set
            {
                confirmCommand = value;
            }
        }

        private RelayCommand cancelRenovation;
        public RelayCommand CancelRenovation
        {
            get { return cancelRenovation; }
            set
            {
                cancelRenovation = value;
            }
        }

        private RenovationPeriodDTO selectedPeriod;
        public RenovationPeriodDTO SelectedPeriod
        {
            get => selectedPeriod;
            set
            {
                if (value != selectedPeriod)
                {
                    selectedPeriod = value;
                    OnPropertyChanged(nameof(SelectedPeriod));
                }
            }
        }

        private Renovation selectedRenovation;
        public Renovation SelectedRenovation
        {
            get => selectedRenovation;
            set
            {
                if (value != selectedRenovation)
                {
                    selectedRenovation = value;
                    OnPropertyChanged(nameof(SelectedRenovation));
                }
            }
        }


        public Renovation Renovations
        {
            get { return renovation; }
            set
            {
                renovation = value;
                OnPropertyChanged("Renovations");
            }
        }
        public RenovationViewModel(User owner)
		{
            LoggedInUser = owner;
			accommodationService = new AccommodationService();
            renovationService = new RenovationService();
            accommodationReservationService = new AccommodationReservationService();
            InitializeProperties();
            InitializeCommands();
			
		}

        public void InitializeProperties()
		{
           Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAll());
            AllRenovations = new ObservableCollection<Renovation>(renovationService.GetAll());
            renovationService.SetAbilityForCancel(AllRenovations);
            renovationService.SetRenovationStatus(AllRenovations);
            Default();
        }

        public void InitializeCommands()
		{
            CheckCommand = new RelayCommand(Execute_CheckCommand, CanExecute);
            ConfirmCommand = new RelayCommand(Execute_ConfirmCommand, CanExecute);
            CancelRenovation = new RelayCommand(Execute_CancelCommand, CanExecute);
		}


        private void Execute_CancelCommand(object parameter)
        {
            renovationService.Delete(SelectedRenovation);
            AllRenovations.Remove(SelectedRenovation);
        }

        

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void Execute_ConfirmCommand(object sender)
        {
            Renovation newRenovation = new Renovation(SelectedPeriod.StartDate, SelectedPeriod.EndDate, Renovations.Duration, Description, SelectedAccommodation.Id, SelectedAccommodation,true,true);
            Renovation savedRenovation = renovationService.Save(newRenovation);
            AllRenovations.Add(savedRenovation);
            var options = new MessageOptions { FontSize = 30 };
            notifier.ShowSuccess("You have successfully scheduled a renovation ", options);
            Default();
            AvailablePeriods.Clear();

        }

        private void Default()
		{
            SelectedAccommodation = Accommodations.Any() ? Accommodations[0] : null;
            Description = "";
            Renovations.Duration = 0;
            startDate = null;
            endDate = null;
            SelectedPeriod = null;
          
        }

        private void Execute_CheckCommand(object sender)
        {
            

            Renovations.StartDate = DateOnly.FromDateTime(startDate ?? DateTime.MinValue);
            Renovations.EndDate = DateOnly.FromDateTime(endDate ?? DateTime.MinValue);
            Renovations.Validate();


			if (Renovations.IsValid)
            {
                reservedDates = new List<DateOnly>(accommodationReservationService.GetReservedDays(SelectedAccommodation.Id));
                renovationDates = new List<DateOnly>(renovationService.GetRenovationDates(SelectedAccommodation.Id));
                AvailablePeriods = new ObservableCollection<RenovationPeriodDTO>(renovationService.GetAvailableDatesForRenovation(renovationDates, reservedDates, DateOnly.FromDateTime(startDate ?? DateTime.MinValue ), DateOnly.FromDateTime(endDate ?? DateTime.MinValue ), Renovations.Duration));
            }
			

        }
    }
}
