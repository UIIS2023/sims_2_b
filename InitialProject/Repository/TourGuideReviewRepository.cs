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
    public class TourGuideReviewRepository : ITourGuideReviewRepository
    {
        public const string FilePath = "../../../Resources/Data/tourguidereviews.csv";

        private readonly Serializer<TourGuideReview> _serializer;

        private List<TourGuideReview> _tourGuideReviews;

        public TourGuideReviewRepository()
        {
            _serializer = new Serializer<TourGuideReview>();
            _tourGuideReviews = _serializer.FromCSV(FilePath);
        }
        public void Delete(TourGuideReview tourGuideReview)
        {
            _tourGuideReviews = _serializer.FromCSV(FilePath);
            TourGuideReview founded = _tourGuideReviews.Find(a => a.Id == tourGuideReview.Id);
            _tourGuideReviews.Remove(founded);
            _serializer.ToCSV(FilePath, _tourGuideReviews);
        }

        public List<TourGuideReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourGuideReview> GetAllByUser(User user)
        {
            return _tourGuideReviews.FindAll(g => g.IdGuide == user.Id);
        }

        public TourGuideReview GetById(int id)
        {
            _tourGuideReviews = _serializer.FromCSV(FilePath);
            return _tourGuideReviews.Find(g => g.Id == id);
        }

        public int NextId()
        {
            _tourGuideReviews = _serializer.FromCSV(FilePath);
            if (_tourGuideReviews.Count < 1)
            {
                return 1;
            }
            return _tourGuideReviews.Max(c => c.Id) + 1;
        }

        public TourGuideReview Save(TourGuideReview tourGuideReview)
        {
            tourGuideReview.Id = NextId();
            _tourGuideReviews = _serializer.FromCSV(FilePath);
            _tourGuideReviews.Add(tourGuideReview);
            _serializer.ToCSV(FilePath, _tourGuideReviews);
            return tourGuideReview;
        }

        public TourGuideReview Update(TourGuideReview tourGuideReview)
        {
            _tourGuideReviews = _serializer.FromCSV(FilePath);
            TourGuideReview current = _tourGuideReviews.Find(a => a.Id == tourGuideReview.Id);
            int index = _tourGuideReviews.IndexOf(current);
            _tourGuideReviews.Remove(current);
            _tourGuideReviews.Insert(index, tourGuideReview);
            _serializer.ToCSV(FilePath, _tourGuideReviews);
            return tourGuideReview;
        }
    }
}
