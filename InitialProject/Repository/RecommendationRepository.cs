using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class RecommendationRepository: IRecommendationRepository
    {
        public const string FilePath = "../../../Resources/Data/recommendations.csv";

        private readonly Serializer<RecommendationOnAccommodation> _serializer;

        private List<RecommendationOnAccommodation> recommendationOnAccommodations;

        public RecommendationRepository()
        {
            _serializer = new Serializer<RecommendationOnAccommodation>();
            recommendationOnAccommodations = _serializer.FromCSV(FilePath);
        }

        public List<RecommendationOnAccommodation> GetByUser(User user)
        {
            recommendationOnAccommodations = _serializer.FromCSV(FilePath);
            return recommendationOnAccommodations.FindAll(c => c.IdGuest == user.Id);
        }


        public List<RecommendationOnAccommodation> GetAll()
        {
            return recommendationOnAccommodations;
        }

        public RecommendationOnAccommodation Save(RecommendationOnAccommodation recommendation)
        {
            recommendation.Id = NextId();
            recommendationOnAccommodations = _serializer.FromCSV(FilePath);
            recommendationOnAccommodations.Add(recommendation);
            _serializer.ToCSV(FilePath, recommendationOnAccommodations);
            return recommendation;
        }

        public int NextId()
        {

            if (recommendationOnAccommodations.Count < 1)
            {
                return 1;
            }
            return recommendationOnAccommodations.Max(c => c.Id) + 1;
        }

        public void Delete(RecommendationOnAccommodation recommendation)
        {

            RecommendationOnAccommodation founded = recommendationOnAccommodations.Find(a => a.Id == recommendation.Id);
            recommendationOnAccommodations.Remove(founded);
            _serializer.ToCSV(FilePath, recommendationOnAccommodations);
        }

        public RecommendationOnAccommodation Update(RecommendationOnAccommodation recommendation)
        {

            RecommendationOnAccommodation current = recommendationOnAccommodations.Find(a => a.Id == recommendation.Id);
            int index = recommendationOnAccommodations.IndexOf(current);
            recommendationOnAccommodations.Remove(current);
            recommendationOnAccommodations.Insert(index, recommendation);
            _serializer.ToCSV(FilePath, recommendationOnAccommodations);
            return recommendation;
        }

        public RecommendationOnAccommodation GetById(int id)
        {

            return recommendationOnAccommodations.Find(g => g.Id == id);
        }
    }
}
