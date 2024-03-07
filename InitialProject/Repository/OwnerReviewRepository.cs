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
	public class OwnerReviewRepository : IOwnerReviewRepository
	{
        public const string FilePath = "../../../Resources/Data/ownerreviews.csv";

        private readonly Serializer<OwnerReview> _serializer;

        private List<OwnerReview> _reviews;

        public OwnerReviewRepository()
        {
            _serializer = new Serializer<OwnerReview>();
            _reviews = _serializer.FromCSV(FilePath);
        }

        public List<OwnerReview> GetAll()
        {
            return _reviews;
        }

        public OwnerReview Save(OwnerReview ownerReview)
        {
            ownerReview.Id = NextId();
            _reviews = _serializer.FromCSV(FilePath);
            _reviews.Add(ownerReview);
            _serializer.ToCSV(FilePath, _reviews);
            return ownerReview;
        }

        public int NextId()
        {
            
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(c => c.Id) + 1;
        }
        public List<OwnerReview> GetByUser(User user)
        {
            _reviews = _serializer.FromCSV(FilePath);
            return _reviews.FindAll(c => c.IdUser == user.Id);
        }

        public void Delete(OwnerReview ownerReview)
        {
            
            OwnerReview founded = _reviews.Find(r => r.Id == ownerReview.Id);
            _reviews.Remove(founded);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public OwnerReview Update(OwnerReview ownerReview)
        {
            
            OwnerReview current = _reviews.Find(r => r.Id == ownerReview.Id);
            int index = _reviews.IndexOf(current);
            _reviews.Remove(current);
            _reviews.Insert(index, ownerReview);
            _serializer.ToCSV(FilePath, _reviews);
            return ownerReview;
        }

        public OwnerReview GetById(int id)
        {
            
            return _reviews.Find(r => r.Id == id);
        }

       

       
    }
}
