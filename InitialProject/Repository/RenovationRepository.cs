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
	public class RenovationRepository : IRenovationRepository
	{
        public const string FilePath = "../../../Resources/Data/renovations.csv";

        private readonly Serializer<Renovation> _serializer;

        private List<Renovation> _renovations;

        public RenovationRepository()
        {
            _serializer = new Serializer<Renovation>();
            _renovations = _serializer.FromCSV(FilePath);
        }

        public List<Renovation> GetAll()
        {
            return _renovations;
        }

        public Renovation Save(Renovation renovation)
        {
            renovation.Id = NextId();
            _renovations = _serializer.FromCSV(FilePath);
            _renovations.Add(renovation);
            _serializer.ToCSV(FilePath, _renovations);
            return renovation;
        }

        public int NextId()
        {

            if (_renovations.Count < 1)
            {
                return 1;
            }
            return _renovations.Max(c => c.Id) + 1;
        }

        public void Delete(Renovation renovation)
        {

            Renovation founded = _renovations.Find(r => r.Id == renovation.Id);
            _renovations.Remove(founded);
            _serializer.ToCSV(FilePath, _renovations);
        }

        public Renovation Update(Renovation renovation)
        {

            Renovation current = _renovations.Find(r => r.Id == renovation.Id);
            int index = _renovations.IndexOf(current);
            _renovations.Remove(current);
            _renovations.Insert(index, renovation);
            _serializer.ToCSV(FilePath, _renovations);
            return renovation;
        }


        public Renovation GetById(int id)
        {

            return _renovations.Find(r => r.Id == id);
        }


        public List<Renovation> GetByAccommodationId(int accommodationId)
		{
            return _renovations.FindAll(r => r.AccommodationId == accommodationId);
		}

    }
}
