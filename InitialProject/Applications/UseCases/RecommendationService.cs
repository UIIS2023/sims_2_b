using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class RecommendationService
    {
        private readonly IRecommendationRepository recommendationRepository;

        private readonly OwnerReviewService ownerReviewService;

        public RecommendationService()
        {
            recommendationRepository = Inject.CreateInstance<IRecommendationRepository>();
            ownerReviewService = new OwnerReviewService();
            
        }

        public List<RecommendationOnAccommodation> GetByUser(User user)
        {
           return recommendationRepository.GetByUser(user);
        }

        public List<RecommendationOnAccommodation> GetAll()
        {
            return recommendationRepository.GetAll();
        }

        public RecommendationOnAccommodation Save(RecommendationOnAccommodation recommendation)
        {
            return recommendationRepository.Save(recommendation);
        }

        public void Delete(RecommendationOnAccommodation recommendation)
        {
             recommendationRepository.Delete(recommendation);
        }

        public RecommendationOnAccommodation Update(RecommendationOnAccommodation recommendation)
        {

            return recommendationRepository.Update(recommendation);
        }

        public RecommendationOnAccommodation GetById(int id)
        {

            return recommendationRepository.GetById(id);
        }

        private void BindData(List<RecommendationOnAccommodation> recommendations)
		{
            foreach(RecommendationOnAccommodation r in recommendations)
			{
                r.OwnerReview = ownerReviewService.GetById(r.IdOwnerReview);
			}
		}

        public List<RecommendationOnAccommodation> GetByAccommodationId(int accommodationId)
		{
            List<RecommendationOnAccommodation> recommendations = new List<RecommendationOnAccommodation>();
            List<RecommendationOnAccommodation> allRecommendations = recommendationRepository.GetAll();
            if(allRecommendations.Count > 0)
			{
                BindData(allRecommendations);
			}

            foreach(RecommendationOnAccommodation r in allRecommendations)
			{
                if(r.OwnerReview.Reservation.IdAccommodation == accommodationId)
				{
                    recommendations.Add(r);
				}
			}

            return recommendations;

		}

        public int GetNumberOfRecommendationsByYear(int year, int accommodationId)
		{
            int count = 0;

            List<RecommendationOnAccommodation> recommendations = GetByAccommodationId(accommodationId);

            foreach(RecommendationOnAccommodation r in recommendations)
			{
				if (r.OwnerReview.Reservation.StartDate.Year == year)
				{
                    count++;
				}
			}

            return count;
		}

        public int GetNumberOfRecommendationsByMonth(int month,int year, int accommodationId)
        {
            int count = 0;

            List<RecommendationOnAccommodation> recommendations = GetByAccommodationId(accommodationId);

            foreach (RecommendationOnAccommodation r in recommendations)
            {
                if (r.OwnerReview.Reservation.StartDate.Year == year && r.OwnerReview.Reservation.StartDate.Month==month)
                {
                    count++;
                }
            }

            return count;
        }

    }
}
