using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
	public class OwnerMainWindowViewModel : ViewModelBase
	{
		public static ObservableCollection<Accommodation> Accommodations { get; set; }

		public static ObservableCollection<AccommodationReservation> Reservations { get; set; }

		public static ObservableCollection<AccommodationReservation> AllReservations { get; set; }

		public static ObservableCollection<AccommodationReservation> FilteredReservations { get; set; }

		public static ObservableCollection<OwnerReview> AllReviews { get; set; }

		public static ObservableCollection <OwnerReview> FilteredReviews { get; set; }

		public static ObservableCollection<GuestReview> Reviews { get; set; }

		private readonly AccommodationService accommodationService;

		private readonly AccommodationReservationService accommodationReservationService;

		private readonly GuestReviewService guestReviewService;

		private readonly OwnerReviewService ownerReviewService;

		private readonly UserService userService;
	

		

        public static Accommodation SelectedAccommodation { get; set; }

		public static User LoggedInUser { get; set; }

		public static AccommodationReservation SelectedReservation { get; set; }

		private RelayCommand addAccommodation;
		public RelayCommand AddAccommodation
		{
			get { return addAccommodation; }
			set
			{
				addAccommodation = value;
			}
		}

		private RelayCommand rateGuests;

		public RelayCommand RateGuests
		{
			get { return rateGuests; }
			set
			{
				rateGuests = value;
			}
		}

		private RelayCommand showMore;

		public RelayCommand ShowMore
		{
			get { return showMore; }
			set
			{
				showMore = value;
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

		private RelayCommand moveReservation;

		public RelayCommand MoveReservation
		{
			get { return moveReservation; }
			set
			{
				moveReservation = value;
			}
		}

		public OwnerMainWindowViewModel(User user)
		{
			
			accommodationService = new AccommodationService();
			accommodationReservationService = new AccommodationReservationService();
			guestReviewService = new GuestReviewService();
			ownerReviewService = new OwnerReviewService();
		    userService = new UserService();
			userService.SuperOwner(user);

			InitializeProperties(user);
			InitializeCommands();
			FilterReservations();
			FilterOwnerReviews();
		

		}

		private void InitializeProperties(User user)
		{
			LoggedInUser = user;
		    Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetByUser(LoggedInUser));
			AllReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetAll());
			
			FilteredReservations = new ObservableCollection<AccommodationReservation>();
			AllReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetReviewsByOwnerId(LoggedInUser.Id));
			FilteredReviews = new ObservableCollection<OwnerReview>();
			Reviews = new ObservableCollection<GuestReview>(guestReviewService.GetAll());
		}

		private void InitializeCommands()
		{
			AddAccommodation = new RelayCommand(Execute_AddAccommodation, CanExecute_Command);
			RateGuests = new RelayCommand(Execute_RateGuests, CanExecute_Command);
			ShowMore = new RelayCommand(Execute_ShowMore, CanExecute_Command);
			YourProfile = new RelayCommand(Execute_YourProfile, CanExecute_Command);
			MoveReservation = new RelayCommand(Execute_MoveReservation, CanExecute_Command);
			
		}

		private void Execute_MoveReservation(object sender)
		{
			//ReservationMoving reservationMoving = new ReservationMoving(LoggedInUser);
			//reservationMoving.Show();
		}

		private void Execute_YourProfile(object sender)
		{
			//OwnersProfile ownersProfile = new OwnersProfile(LoggedInUser);
			//ownersProfile.Show();

		}

		private void Execute_AddAccommodation(object sender)
		{
			//CreateAccommodation createAccommodation = new CreateAccommodation(LoggedInUser);
			//createAccommodation.Show();
		}

		private void Execute_RateGuests(object sender)
		{
			if (SelectedReservation == null)
			{
				MessageBox.Show("Potrebno je izbrati gosta kog zelite da ocenite!");
			}
			else
			{
				//RateGuests rateGuests = new RateGuests(LoggedInUser, SelectedReservation);
				//rateGuests.Show();
			}

		}

		private void Execute_ShowMore(object sender)
		{
			//ViewAccommodationGallery viewAccommodationGallery = new ViewAccommodationGallery(SelectedAccommodation);
			//viewAccommodationGallery.Show();
		}

		private bool CanExecute_Command(object parameter)
		{
			return true;
		}
		private void FilterReservations()
		{


			Reservations = new ObservableCollection<AccommodationReservation>((AllReservations.ToList().FindAll(c => c.Accommodation.IdUser == LoggedInUser.Id)));


			DateOnly today = DateOnly.FromDateTime(DateTime.Now);

			foreach (AccommodationReservation res in Reservations)
			{
				if (accommodationReservationService.IsElegibleForReview(today, res) == false) continue;
				FilteredReservations.Add(res);

			}
		}

		private void FilterOwnerReviews()
		{
			foreach(OwnerReview ownerReview in AllReviews)
			{
				if (ownerReviewService.IsElegibleForDisplay(ownerReview))
				{
					FilteredReviews.Add(ownerReview);
				}
			}
		}

		

	
	}
}
