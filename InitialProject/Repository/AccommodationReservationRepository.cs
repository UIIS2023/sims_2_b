using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using InitialProject.View;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {

        private const string FilePath = "../../../Resources/Data/accommodationreservations.csv";


        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;



        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);


        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

       

        public string GetNameById(int id)
        {
            
            foreach (Accommodation accommodation in Guest1MainWindowViewModel.AccommodationsMainList)
            {
                if (accommodation.Id == id)
                {
                    return accommodation.Name;
                }
                return null;
            }
            return null;
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public int NextId()
        {
            
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }



        public void Delete(AccommodationReservation accommodationReservation)
        {
            
            AccommodationReservation founded = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            _accommodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);       
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public List<AccommodationReservation> GetByUser(User user)

        { 
            return _accommodationReservations.FindAll(a => a.IdGuest == user.Id && a.IsCanceled==false);
        }
        

        public AccommodationReservation GetById(int id)
		{
            
            return _accommodationReservations.Find(a => a.Id == id);
        }

        public List<AccommodationReservation> GetByAccommodationId(int idAccommodation)
		{
            return _accommodationReservations.FindAll(a => a.IdAccommodation == idAccommodation);
		}

      
    }
}

