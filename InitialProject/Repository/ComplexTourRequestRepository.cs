using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/complextourrequests.csv";

        private readonly Serializer<ComplexTourRequests> _serializer;

        private List<ComplexTourRequests> _complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _serializer = new Serializer<ComplexTourRequests>();
            _complexTourRequests = _serializer.FromCSV(FilePath);
        }

        public void Delete(ComplexTourRequests complexTourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequests founded = _complexTourRequests.Find(c => c.Id == complexTourRequest.Id);
            _complexTourRequests.Remove(founded);
            _serializer.ToCSV(FilePath, _complexTourRequests);
        }

        public List<ComplexTourRequests> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ComplexTourRequests GetById(int id)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            return _complexTourRequests.Find(c => c.Id == id);
        }

        public int NextId()
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            if (_complexTourRequests.Count < 1)
            {
                return 1;
            }
            return _complexTourRequests.Max(c => c.Id) + 1;
        }

        public ComplexTourRequests Save(ComplexTourRequests complexTourRequest)
        {
            complexTourRequest.Id = NextId();
            _complexTourRequests = _serializer.FromCSV(FilePath);
            _complexTourRequests.Add(complexTourRequest);
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return complexTourRequest;
        }

        public ComplexTourRequests Update(ComplexTourRequests complexTourRequest)
        {
            _complexTourRequests = _serializer.FromCSV(FilePath);
            ComplexTourRequests current = _complexTourRequests.Find(c => c.Id == complexTourRequest.Id);
            int index = _complexTourRequests.IndexOf(current);
            _complexTourRequests.Remove(current);
            _complexTourRequests.Insert(index, complexTourRequest);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _complexTourRequests);
            return complexTourRequest;
        }

    }
}
