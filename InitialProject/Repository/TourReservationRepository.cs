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
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/toursReservation.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _toursReservation;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _toursReservation = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _toursReservation = _serializer.FromCSV(FilePath);
            _toursReservation.Add(tourReservation);
            _serializer.ToCSV(FilePath, _toursReservation);
            return tourReservation;
        }

        public int NextId()
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            if (_toursReservation.Count < 1)
            {
                return 1;
            }
            return _toursReservation.Max(c => c.Id) + 1;
        }

        public void Delete(TourReservation tourReservation)
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            TourReservation founded = _toursReservation.Find(c => c.Id == tourReservation.Id);
            _toursReservation.Remove(founded);
            _serializer.ToCSV(FilePath, _toursReservation);
        }

        public TourReservation Update(TourReservation tourReservation)
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            TourReservation current = _toursReservation.Find(c => c.Id == tourReservation.Id);
            int index = _toursReservation.IndexOf(current);
            _toursReservation.Remove(current);
            _toursReservation.Insert(index, tourReservation);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _toursReservation);
            return tourReservation;
        }

        public List<TourReservation> GetByUser(User user)
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            return _toursReservation.FindAll(c => c.IdUser == user.Id);
        }

        public List<TourReservation> GetByTour(int idTour)
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            return _toursReservation.FindAll(c => c.IdTour == idTour);
        }

        public TourReservation GetById(int id)
        {
            _toursReservation = _serializer.FromCSV(FilePath);
            return _toursReservation.Find(c => c.Id == id);
        }
    }
}
