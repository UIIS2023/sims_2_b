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
    public class TourAttendanceRepository : ITourAttendanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourattendances.csv";

        private readonly Serializer<TourAttendance> _serializer;

        private List<TourAttendance> _attendances;

        public TourAttendanceRepository()
        {
            _serializer = new Serializer<TourAttendance>();
            _attendances = _serializer.FromCSV(FilePath);
        }

        public List<TourAttendance> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourAttendance> GetAllByGuide(User user)
        {
            return _attendances.FindAll(c => c.IdGuide == user.Id);
        }

        public TourAttendance Save(TourAttendance tourpoint)
        {
            tourpoint.Id = NextId();
            _attendances = _serializer.FromCSV(FilePath);
            _attendances.Add(tourpoint);
            _serializer.ToCSV(FilePath, _attendances);
            return tourpoint;
        }

        public int NextId()
        {
            _attendances = _serializer.FromCSV(FilePath);
            if (_attendances.Count < 1)
            {
                return 1;
            }
            return _attendances.Max(c => c.Id) + 1;
        }

        public void Delete(TourAttendance tourattendance)
        {
            _attendances = _serializer.FromCSV(FilePath);
            TourAttendance founded = _attendances.Find(c => c.Id == tourattendance.Id);
            _attendances.Remove(founded);
            _serializer.ToCSV(FilePath, _attendances);
        }

        public TourAttendance Update(TourAttendance tourattendance)
        {
            _attendances = _serializer.FromCSV(FilePath);
            TourAttendance current = _attendances.Find(c => c.Id == tourattendance.Id);
            int index = _attendances.IndexOf(current);
            _attendances.Remove(current);
            _attendances.Insert(index, tourattendance);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _attendances);
            return tourattendance;
        }

        public TourAttendance GetById(int id)
        {
            return _attendances.Find(c => c.Id == id);
        }
    }
}
