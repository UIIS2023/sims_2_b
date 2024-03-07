using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Repository
{
    internal class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return _locations;
        }


      
        public Location Save(Location location)
        {
            if (!IsSaved(location))
            {
                location.Id = NextId();
                _locations = _serializer.FromCSV(FilePath);
                _locations.Add(location);
                _serializer.ToCSV(FilePath, _locations);
            }
            return location;
        }

        public bool IsSaved(Location location)
        {
            
            Location current = _locations.Find(c => c.City == location.City);
            if (current != null)
                return true;
            else
                return false;
        }


        public int NextId()
        {
            
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(c => c.Id) + 1;
        }

        public void Delete(Location location)
        {
         
            Location founded = _locations.Find(c => c.Id == location.Id);
            _locations.Remove(founded);
            _serializer.ToCSV(FilePath, _locations);
        }

        public Location GetByCity(string city)
        {
            
            return _locations.Find(c => c.City == city);
        }

        public Location Update(Location location)
        {
           
            Location current = _locations.Find(c => c.Id == location.Id);
            int index = _locations.IndexOf(current);
            _locations.Remove(current);
            _locations.Insert(index, location);       
            _serializer.ToCSV(FilePath, _locations);
            return location;
        }


        public Location FindLocation(String Country, String City)

        {
            foreach (Location location in _locations)
            {
                if (location.Country == Country && location.City == City)
                    return location;
            }
            return null;
        }

        public List<String> GetAllCountries()
        {
            List<String> countries = new List<String>();

            foreach (Location location in _locations)
            {
                if (!countries.Contains(location.Country))
                    countries.Add(location.Country);
            }
            return countries;
        }

        public List<String> GetCities(String Country)
        {
            List<String> cities = new List<string>();

            foreach (Location location in _locations)
            {
                if (location.Country == Country)
                {
                    cities.Add(location.City);
                }
            }
            return cities;
        }



        public Location GetById(int id)
        {
            foreach (Location location in _locations)
            {
                if (location.Id == id)
                {
                    return location;
                }
            }
            return null;
        }

    }
}

