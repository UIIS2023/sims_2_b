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
	internal class GuestReviewService
	{
		private readonly IGuestReviewRepository guestReviewRepository;
		private readonly IOwnerReviewRepository ownerReviewRepository;
		public GuestReviewService()
		{
			guestReviewRepository = Inject.CreateInstance<IGuestReviewRepository>();
			ownerReviewRepository = Inject.CreateInstance<IOwnerReviewRepository>();
		}

		public List<GuestReview> GetAll()
		{
			List<GuestReview> reviews = new List<GuestReview>();
			reviews = guestReviewRepository.GetAll();
			return reviews;
		}

		public GuestReview Save(GuestReview guestReview)
		{
			GuestReview savedGuestReview = guestReviewRepository.Save(guestReview);
			return savedGuestReview;
		}

		public List<GuestReview> GetByUser(User user)
		{
			return guestReviewRepository.GetByUser(user);
		}

		public bool IsElegibleForDisplay(GuestReview guest)
		{
			List<OwnerReview> ownerReviews = ownerReviewRepository.GetAll();

			bool toAdd = false;
			foreach (OwnerReview ownerReview in ownerReviews)
			{
				if (ownerReview.ReservationId == guest.IdReservation)
				{
					toAdd = true;
				}
			}

			return toAdd;
		}

	}
}
