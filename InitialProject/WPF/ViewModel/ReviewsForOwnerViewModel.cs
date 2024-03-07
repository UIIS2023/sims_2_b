using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class ReviewsForOwnerViewModel
	{
		public static ObservableCollection<OwnerReview> AllReviews { get; set; }

		public static ObservableCollection<OwnerReview> FilteredReviews { get; set; }


		private readonly OwnerReviewService ownerReviewService;

		public static User LoggedInUser { get; set; }
		public ReviewsForOwnerViewModel(User owner)
		{
			LoggedInUser = owner;

			ownerReviewService = new OwnerReviewService();

			InitializeProperties(owner);
			FilterOwnerReviews();

		}

		public void InitializeProperties(User owner)
		{
			AllReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetReviewsByOwnerId(LoggedInUser.Id));
			FilteredReviews = new ObservableCollection<OwnerReview>();
		}

		private void FilterOwnerReviews()
		{
			foreach (OwnerReview ownerReview in AllReviews)
			{
				if (ownerReviewService.IsElegibleForDisplay(ownerReview))
				{
					FilteredReviews.Add(ownerReview);
				}
			}
		}
	}
}
