using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class CreateAccommodationViewModel : ViewModelBase
	{
		private readonly AccommodationService accommodationService;

		private readonly LocationService locationService;

		private readonly ImageRepository _imageRepository;

		public static ObservableCollection<String> Countries { get; set; }

		public Accommodation accommodation = new Accommodation();

		public static User LoggedInUser { get; set; }

		public static List<Image> Images { get; set; }
		public List<string> ImagePaths { get; set; }

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

		public Accommodation Accommodation
		{
			get { return accommodation; }
			set
			{
				accommodation = value;
				OnPropertyChanged("Accommodation");
			}
		}


		private ObservableCollection<String> _cities;
		public ObservableCollection<String> Cities
		{
			get { return _cities; }
			set
			{
				_cities = value;
				OnPropertyChanged(nameof(Cities));
			}
		}


		private ObservableCollection<AccommodationType> _accommodationTypes;
		public ObservableCollection<AccommodationType> AccommodationTypes
		{
			get { return _accommodationTypes; }
			set
			{
				_accommodationTypes = value;
				OnPropertyChanged(nameof(AccommodationTypes));
			}
		}



		private bool _isCityEnabled;
		public bool IsCityEnabled
		{
			get { return _isCityEnabled; }
			set
			{
				_isCityEnabled = value;
				OnPropertyChanged(nameof(IsCityEnabled));
			}
		}


		private AccommodationType _selectedType;
		public AccommodationType SelectedType
		{
			get { return _selectedType; }
			set
			{
				_selectedType = value;
				OnPropertyChanged(nameof(SelectedType));
			}
		}


		private String _selectedCity;
		public String SelectedCity
		{
			get { return _selectedCity; }
			set
			{
				_selectedCity = value;
				OnPropertyChanged(nameof(SelectedCity));
				
			}
		}

		private String _selectedCountry;
		public String SelectedCountry
		{
			get { return _selectedCountry; }
			set
			{
				if (_selectedCountry != value)
				{
					_selectedCountry = value;
					Cities = new ObservableCollection<String>(locationService.GetCities(SelectedCountry));
					SelectedCity = Cities[0];
					if (Cities.Count == 0)
					{
						IsCityEnabled = false;
					}
					else
					{
						IsCityEnabled = true;
					}
					OnPropertyChanged(nameof(Cities));
					OnPropertyChanged(nameof(SelectedCountry));
					OnPropertyChanged(nameof(IsCityEnabled));
				}
			}
		}


		

		private RelayCommand confirmCreate;
		public RelayCommand ConfirmCreate
		{
			get { return confirmCreate; }
			set
			{
				confirmCreate = value;
			}
		}

		private RelayCommand addImages;
		public RelayCommand AddImages
		{
			get { return addImages; }
			set
			{
				addImages = value;
			}
		}

		public CreateAccommodationViewModel(User user)
		{
			
			LoggedInUser = user;
			accommodationService = new AccommodationService();
			locationService = new LocationService();
			_imageRepository = new ImageRepository();
			InitializeProperties();
			InitializeCommands();


		}

		private void InitializeProperties()
		{
			Countries = new ObservableCollection<String>(locationService.GetAllCountries());
			Cities = new ObservableCollection<String>();
			AccommodationTypes = new ObservableCollection<AccommodationType>();
			AccommodationTypes.Add(AccommodationType.Apartment);
			AccommodationTypes.Add(AccommodationType.House);
			AccommodationTypes.Add(AccommodationType.Cottage);
			Images = new List<Image>();
			MaxGuestNum = 1;
			MinResevationDays = 1;
			DaysBeforeCancel = 1;
		}


		private void InitializeCommands()
		{
			
			ConfirmCreate = new RelayCommand(Execute_ConfirmCreate, CanExecute_Command);
			AddImages = new RelayCommand(Execute_AddImages, CanExecute_Command);
		}


		private void Execute_AddImages(object sender)
		{
			ImagePaths = FileDialogService.GetImagePaths("Resources\\Images\\Accommodations", "/Resources/Images");
		}

		private bool CanExecute_Command(object parameter)
		{
			return true;
		}

		private void Execute_ConfirmCreate(object sender)
		{
			Accommodation.Validate();

			if (Accommodation.IsValid)
			{
				Location location = locationService.FindLocation(SelectedCountry, SelectedCity);
				Accommodation Accommodation1 = new Accommodation(accommodation.Name, location.Id, location,  AccommodationType,MaxGuestNum, MinResevationDays, DaysBeforeCancel, LoggedInUser.Id);
				Accommodation savedAccommodation = accommodationService.Save(Accommodation1);

				CreateImages(savedAccommodation);
				AccommodationUCViewModel.Accommodations.Add(savedAccommodation);

				var options = new MessageOptions { FontSize = 30 };
				notifier.ShowSuccess("Successfully created", options);
				Refresh();
			}
			else
			{
				OnPropertyChanged(nameof(Accommodation));
			}
			
		}

		private void CreateImages(Accommodation accommodation)
		{
			foreach(string name in ImagePaths)
			{
				Image newImage = new Image(name, accommodation.Id, 0, 0);
				Image savedImage = _imageRepository.Save(newImage);
				//accommodation.Images.Add(savedImage);
			}
		}
		public void Refresh()
		{
			Accommodation.Name = null;
			SelectedCountry = Countries.Any() ? Countries[0] : null;
			SelectedCity = Cities[0];
			SelectedType = AccommodationTypes[0];
			MaxGuestNum = 1;
			MinResevationDays = 1;
			DaysBeforeCancel = 1;

		}


		private AccommodationType _accommodationType;
		public AccommodationType AccommodationType
		{
			get => _accommodationType;
			set
			{
				if (value != _accommodationType)
				{
					_accommodationType = value;
					OnPropertyChanged();
				}
			}
		}


		private int _maxGuestNum;
		public int MaxGuestNum
		{
			get => _maxGuestNum;
			set
			{
				if (value != _maxGuestNum)
				{
					_maxGuestNum = value;
					OnPropertyChanged();
				}
			}
		}


		private int _minDaysReservation;
		public int MinResevationDays
		{
			get => _minDaysReservation;
			set
			{
				if (value != _minDaysReservation)
				{
					_minDaysReservation = value;
					OnPropertyChanged();
				}
			}
		}


		private int _daysBeforeCancel;
		public int DaysBeforeCancel
		{
			get => _daysBeforeCancel;
			set
			{
				if (value != _daysBeforeCancel)
				{
					_daysBeforeCancel = value;
					OnPropertyChanged();
				}
			}
		}


		private string _imageUrl;
		public string ImageUrl
		{
			get => _imageUrl;
			set
			{
				if (value != _imageUrl)
				{
					_imageUrl = value;
					OnPropertyChanged();
				}
			}
		}

		

		

		
	}
}
