using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModel
{
	internal class MenuWindowViewModel : ViewModelBase
	{
		public User LoggedInUser { get; set; }

		public UserControl CurrentUserControl { get; set; }

		private RelayCommand openAccommodations;
		public RelayCommand OpenAccommodations
		{
			get { return openAccommodations; }
			set
			{
				openAccommodations = value;
			}
		}

		private RelayCommand openReservations;
		public RelayCommand OpenReservations
		{
			get { return openReservations; }
			set
			{
				openReservations = value;
			}
		}

		private RelayCommand openReviews;
		public RelayCommand OpenReviews
		{
			get { return openReviews; }
			set
			{
				openReviews = value;
			}
		}

		private RelayCommand openRenovations;
		public RelayCommand OpenRenovations
		{
			get { return openRenovations; }
			set
			{
				openRenovations = value;
			}
		}

		private RelayCommand openForum;
		public RelayCommand OpenForum
		{
			get { return openForum; }
			set
			{
				openForum  = value;
			}
		}

		private RelayCommand yourProfile;
		public RelayCommand YourProfile
		{
			get { return yourProfile; }
			set
			{
				yourProfile = value;
			}
		}

		private RelayCommand openTutorial;
		public RelayCommand OpenTutorial
		{
			get { return openTutorial; }
			set
			{
				openTutorial = value;
			}
		}

		private RelayCommand viewNotifications;
		public RelayCommand ViewNotifications
		{
			get { return viewNotifications; }
			set
			{
				viewNotifications = value;
			}
		}
		public MenuWindowViewModel(User owner)
		{
			LoggedInUser=owner;
			var accommodationUCViewModel = new AccommodationUCViewModel(owner);
			CurrentUserControl = new AccommodationUC(owner, accommodationUCViewModel);

			accommodationUCViewModel.AddEvent += OnAdd;
			accommodationUCViewModel.StatisticsEvent += selectedAccommodation => {
				var selectedAccommodationViewModel = new StatisticsForAccommodationViewModel(selectedAccommodation, LoggedInUser);
				CurrentUserControl.Content = new StatisticsForAccommodation(selectedAccommodationViewModel);
			};
			InitializeCommands();
		}

		public void InitializeCommands()
		{
			OpenAccommodations = new RelayCommand(Execute_OpenAccommodations, CanExecute);
			OpenReservations = new RelayCommand(Execute_OpenReservations, CanExecute);
			OpenReviews = new RelayCommand(Execute_OpenReviews, CanExecute);
			OpenRenovations = new RelayCommand(Execute_OpenRenovations, CanExecute);
			OpenForum = new RelayCommand(Execute_OpenForum, CanExecute);
			YourProfile = new RelayCommand(Execute_YourProfile, CanExecute);
			ViewNotifications = new RelayCommand(Execute_Notifications, CanExecute);
			OpenTutorial = new RelayCommand(Execute_OpenTutorial, CanExecute);
		}

		private void Execute_OpenTutorial(object sender)
		{
			CurrentUserControl.Content = new Tutorial();
		}

		private void Execute_YourProfile(object sender)
		{
			var ownersProfileViewModel = new OwnersProfileViewModel(LoggedInUser);
			CurrentUserControl.Content = new OwnersProfile(LoggedInUser, ownersProfileViewModel);
		}

		private bool CanExecute(object parameter)
		{
			return true;
		}

		private void Execute_OpenAccommodations(object sender)
		{
			var accommodationUCViewModel = new AccommodationUCViewModel(LoggedInUser);
			CurrentUserControl.Content = new AccommodationUC(LoggedInUser, accommodationUCViewModel);

			accommodationUCViewModel.AddEvent += OnAdd;
			accommodationUCViewModel.StatisticsEvent += selectedAccommodation =>
			{
				var selectedAccommodationViewModel = new StatisticsForAccommodationViewModel(selectedAccommodation, LoggedInUser);
				CurrentUserControl.Content = new StatisticsForAccommodation(selectedAccommodationViewModel);

			};
				accommodationUCViewModel.ViewMoreEvent += selectedAccommodation => {
					
					CurrentUserControl.Content = new ViewAccommodationGallery(selectedAccommodation);
				};
		}




		private void OnAdd()
		{
			var createAccommodationViewModel = new CreateAccommodationViewModel(LoggedInUser);
			CurrentUserControl.Content = new CreateAccommodation(LoggedInUser, createAccommodationViewModel);
		}

		private void Execute_OpenReservations(object sender)
		{
			var reservationMoving = new ReservationMovingViewModel(LoggedInUser);
			CurrentUserControl.Content = new ReservationMoving(LoggedInUser, reservationMoving);
		}

		private void Execute_OpenReviews(object sender)
		{
			var reviewsForGuestsViewModel = new ReviewsForGuestsUCViewModel(LoggedInUser);
			CurrentUserControl.Content= new ReviewsForGuestsUC(LoggedInUser, reviewsForGuestsViewModel);

			reviewsForGuestsViewModel.Grades += OnGrades;
		}

		private void OnGrades()
		{
			var reviewsForOwnerViewModel = new ReviewsForOwnerViewModel(LoggedInUser);
			CurrentUserControl.Content = new ReviewsForOwner(LoggedInUser, reviewsForOwnerViewModel);

		}

		private void Execute_Notifications(object sender)
		{
			var ownerNotificationsViewModel = new OwnerNotificationsViewModel(LoggedInUser);
			CurrentUserControl.Content = new OwnerNotifications(LoggedInUser, ownerNotificationsViewModel);

			ownerNotificationsViewModel.RateGuests += OnRateGuests;
			ownerNotificationsViewModel.CheckRequests += OnCheckRequests;
			ownerNotificationsViewModel.CheckForum += OnCheckForum;
		}


		private void OnRateGuests()
		{
			var reviewsForGuestsViewModel = new ReviewsForGuestsUCViewModel(LoggedInUser);
			CurrentUserControl.Content = new ReviewsForGuestsUC(LoggedInUser, reviewsForGuestsViewModel);
		}


		private void OnCheckRequests()
		{
			var reservationMoving = new ReservationMovingViewModel(LoggedInUser);
			CurrentUserControl.Content = new ReservationMoving(LoggedInUser, reservationMoving);
		}

		private void OnCheckForum()
		{
			var ownerForum = new OwnerForumViewModel(LoggedInUser);
			CurrentUserControl.Content = new OwnerForum(LoggedInUser, ownerForum);
		}



		private void Execute_OpenRenovations(object sender)
		{
			var renovationViewModel = new RenovationViewModel(LoggedInUser);
			CurrentUserControl.Content = new RenovationUC(LoggedInUser, renovationViewModel);
		}

		private void Execute_OpenForum(object sender)
		{
			var ownerForumViewModel = new OwnerForumViewModel(LoggedInUser);
			CurrentUserControl.Content= new OwnerForum(LoggedInUser, ownerForumViewModel);
		}
	}
}
