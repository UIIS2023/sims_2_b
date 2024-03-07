using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Repository
{
    internal class TourPointRepository : ITourPointRepository
    {
        private const string FilePath = "../../../Resources/Data/tourpoints.csv";

        private readonly Serializer<TourPoint> _serializer;

        private List<TourPoint> _tourpoints;

        public TourPointRepository()
        {
            _serializer = new Serializer<TourPoint>();
            _tourpoints = _serializer.FromCSV(FilePath);
        }

        public List<TourPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourPoint Save(TourPoint tourpoint)
        {
            tourpoint.Id = NextId();
            _tourpoints = _serializer.FromCSV(FilePath);
            _tourpoints.Add(tourpoint);
            _serializer.ToCSV(FilePath, _tourpoints);
            return tourpoint;
        }

        public int NextId()
        {
            _tourpoints = _serializer.FromCSV(FilePath);
            if (_tourpoints.Count < 1)
            {
                return 1;
            }
            return _tourpoints.Max(c => c.Id) + 1;
        }

        public void Delete(TourPoint tourpoint)
        {
            _tourpoints = _serializer.FromCSV(FilePath);
            TourPoint founded = _tourpoints.Find(c => c.Id == tourpoint.Id);
            _tourpoints.Remove(founded);
            _serializer.ToCSV(FilePath, _tourpoints);
        }

        public TourPoint Update(TourPoint tourpoint)
        {
            _tourpoints = _serializer.FromCSV(FilePath);
            TourPoint current = _tourpoints.Find(c => c.Id == tourpoint.Id);
            int index = _tourpoints.IndexOf(current);
            _tourpoints.Remove(current);
            _tourpoints.Insert(index, tourpoint);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourpoints);
            return tourpoint;
        }

        public List<TourPoint> GetAllByTourId(int idTour)
        {
            _tourpoints = _serializer.FromCSV(FilePath);
            return _tourpoints.FindAll(c => c.IdTour == idTour);
        }

        public TourPoint GetById(int id)
        {
            return _tourpoints.Find(c=>c.Id == id);
        }
 
        public int GetTourPointIdByTourPointName(string name)
        {
            foreach(TourPoint tourPoint in _tourpoints) 
            {
                if(tourPoint.Name == name)
                {
                    return tourPoint.Id;
                }
                    
            }
            return 0;
        }
    }
}
