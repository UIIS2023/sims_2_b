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
    public class ReservationDisplacementRequestRepository: IReservationDisplacementRequestRepository
    {
        public const string FilePath = "../../../Resources/Data/requestes.csv";

        private readonly Serializer<ReservationDisplacementRequest> _serializer;

        private List<ReservationDisplacementRequest> _requestes;

        public ReservationDisplacementRequestRepository()
        {
            _serializer = new Serializer<ReservationDisplacementRequest>();
            _requestes = _serializer.FromCSV(FilePath);
        }

        public List<ReservationDisplacementRequest> GetByUser(User user)
        {
            return _requestes.FindAll(a => a.IdUser == user.Id);
        }
        public List<ReservationDisplacementRequest> GetAll()
        {
            return _requestes;
        }

        public ReservationDisplacementRequest Save(ReservationDisplacementRequest request)
        {
            request.Id = NextId();
            _requestes = _serializer.FromCSV(FilePath);
            _requestes.Add(request);
            _serializer.ToCSV(FilePath, _requestes);
            return request;
        }

        public int NextId()
        {

            if (_requestes.Count < 1)
            {
                return 1;
            }
            return _requestes.Max(c => c.Id) + 1;
        }
       

        public void Delete(ReservationDisplacementRequest request)
        {

            ReservationDisplacementRequest founded = _requestes.Find(r => r.Id == request.Id);
            _requestes.Remove(founded);
            _serializer.ToCSV(FilePath, _requestes);
        }

        public ReservationDisplacementRequest Update(ReservationDisplacementRequest request)
        {

            ReservationDisplacementRequest current = _requestes.Find(r => r.Id == request.Id);
            int index = _requestes.IndexOf(current);
            _requestes.Remove(current);
            _requestes.Insert(index, request);
            _serializer.ToCSV(FilePath, _requestes);
            return request;
        }

        public ReservationDisplacementRequest GetById(int id)
        {

            return _requestes.Find(r => r.Id == id);
        }

      
    }
}
