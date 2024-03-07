using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class OwnersProfileViewModel : ViewModelBase
	{
		public User LoggedInUser { get; set; }

		public double AverageGrade { get; set; }

		public int GradeNum { get; set; }

		public string ImageSource { get; set; }

		public string UserImageSource { get; set; }

		private readonly OwnerReviewService ownerReviewService;

		private readonly UserService userService;

		List<OwnerReview> ownerReviews;


		public OwnersProfileViewModel(User user)
		{
			
			ownerReviewService = new OwnerReviewService();

			userService = new UserService();

			InitializeProperties(user);

			userService.SuperOwner(user);

			SetImagesSource(user);

		}

		private void InitializeProperties(User user)
		{
			LoggedInUser = user;

			ownerReviews = ownerReviewService.GetReviewsByOwnerId(user.Id);

			AverageGrade = Math.Round(userService.AverageGrade(ownerReviews), 2);

			GradeNum = ownerReviews.Count();
		}

		public void SetImagesSource(User user)
		{
			if (user.IsSuper == true)
			{
				ImageSource = "../../Resources/Images/true.png";
			}
			else
			{
				ImageSource = "../../Resources/Images/delete.png";
			}

			UserImageSource = userService.GetImageUrlByUserId(user.Id);
		}
	}
}
