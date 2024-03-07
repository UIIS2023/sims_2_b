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
	internal class UserService
	{
		private readonly IUserRepository userRepository;

		private readonly OwnerReviewService ownerReviewService;

		public UserService()
		{
			userRepository = Inject.CreateInstance<IUserRepository>();
			ownerReviewService = new OwnerReviewService();
		}
		

		public void SuperOwner(User owner)
		{
			List<OwnerReview> ownerReviews = ownerReviewService.GetReviewsByOwnerId(owner.Id);

			double avg = AverageGrade(ownerReviews);

			if (avg > 9.5 && ownerReviews.Count >= 3)
			{
				owner.IsSuper = true;
				userRepository.Update(owner);
			}
			else
			{
				owner.IsSuper = false;
				userRepository.Update(owner);
			}


		}

        public User GetByUsername(string username)
        {

			return userRepository.GetByUsername(username);
        }

		public User GetById(int id)
		{ 
			return userRepository.GetById(id);
		}


        public double AverageGrade(List<OwnerReview> ownerReviews)
		{
			int sum = 0;

			double avg;

			foreach (OwnerReview ownerReview in ownerReviews)
			{
				sum += ownerReview.OwnerCorrectness + ownerReview.CleanlinessGrade;
				
			}

			avg = (double)sum / ownerReviews.Count;
			return avg;
		}

		public string GetImageUrlByUserId(int id)
		{
			return userRepository.GetImageUrlByUserId(id);
		}


        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

		public User Update(User user)
		{
			return userRepository.Update(user);
		}

		public void DeleteUser(User user)
		{
			userRepository.Delete(user);
		}
    }
}
