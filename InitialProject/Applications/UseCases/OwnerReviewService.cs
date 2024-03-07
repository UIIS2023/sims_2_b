using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
	public class OwnerReviewService
	{
		private readonly IOwnerReviewRepository ownerReviewRepository;

		private readonly GuestReviewService guestReviewService;

		private readonly AccommodationReservationService accommodationReservationService;



		public OwnerReviewService()
		{
		   
			ownerReviewRepository = Inject.CreateInstance<IOwnerReviewRepository>();
			guestReviewService=new GuestReviewService();

		}

		public List<OwnerReview> GetByUser(User user)
		{
			return  ownerReviewRepository.GetByUser(user);
		}

		public OwnerReview Save(OwnerReview ownerReview)
		{
			return ownerReviewRepository.Save(ownerReview);
		}
		public bool IsElegibleForDisplay(OwnerReview ownerReview)
		{
			List<GuestReview> guestReviews = guestReviewService.GetAll();

			bool toAdd=false;
			foreach (GuestReview guestReview in guestReviews)
			{
				if(guestReview.IdReservation == ownerReview.ReservationId)
				{
					toAdd=true;
				}
			}
			
			return toAdd;
		}

		private void BindData(List<OwnerReview> ownerReviews)
		{
			AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
			foreach (OwnerReview ownerReview in ownerReviews)
			{
				ownerReview.Reservation = accommodationReservationService.GetById(ownerReview.ReservationId);
				
			}
		}

		public void BindParticularData(OwnerReview review)
		{
			AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
			review.Reservation = accommodationReservationService.GetById(review.ReservationId);
		}

		public List<OwnerReview> GetReviewsByOwnerId(int id)
		{
			List<OwnerReview> reviews = new List<OwnerReview>();
			List<OwnerReview> ownerReviews = ownerReviewRepository.GetAll();
			BindData(ownerReviews);

			foreach (OwnerReview ownerReview in ownerReviews)
			{
				if (ownerReview.Reservation.Accommodation.IdUser == id)
				{
					reviews.Add(ownerReview);
				}
			}
			return reviews;
		}

		

		public List<OwnerReview> GetAll()
		{
			/*List<OwnerReview> ownerReviews = ownerReviewRepository.GetAll();
			if (ownerReviews.Count > 0)
			{
				BindData(ownerReviews);
			}
			
			return ownerReviews;*/
			return ownerReviewRepository.GetAll();
		}

		public OwnerReview GetById(int id)
		{

			OwnerReview review = ownerReviewRepository.GetById(id);
			if(review != null)
			{
				BindParticularData(review);
			}

			return review;
		}

	}
}
