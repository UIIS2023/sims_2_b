using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
	
	public class LocationService
	{

		private readonly ILocationRepository _locationRepository;
		private readonly AccommodationReservationRepository accommodationReservationService;
		private readonly TourReservationRepository tourReservationService;
         private readonly AccommodationRepository accommodationService;
		public LocationService()
		{
			_locationRepository = Inject.CreateInstance<ILocationRepository>();
			accommodationReservationService = new AccommodationReservationRepository();
			tourReservationService = new TourReservationRepository();
			accommodationService = new AccommodationRepository();
		}

		

		

		public Location GetById(int id)
		{
			return _locationRepository.GetById(id);
		}
		public List<Location> GetAll()
		{
			return _locationRepository.GetAll();
		}


		public List<String> GetCities(String Country)
		{
			return _locationRepository.GetCities(Country);
		}

		public List<String> GetAllCountries()
		{
			return _locationRepository.GetAllCountries();
		}

		public Location FindLocation(String Country, String City)
		{
			return _locationRepository.FindLocation(Country, City);
		}

        public bool HasUserVisitedLocation(int userId, int locationId)
        {
            var accommodationReservations = accommodationReservationService.GetAll();
            foreach (var reservation in accommodationReservations)
            {
                if (reservation.IdGuest == userId && reservation.IdAccommodation != null)
                {
                    int accommodationId = reservation.IdAccommodation;
                    var accommodation = accommodationService.GetById(accommodationId);

                    if (accommodation != null && accommodation.IdLocation == locationId)
                    {
                        return true;
                    }
                }
            }

            var tourReservations = tourReservationService.GetAll();
            return tourReservations.Any(tr => tr.IdUser == userId && tr.IdLocation == locationId);
        }


        public bool HasUserVisitedLocation(int userId, int locationId, List<Comment> comments, HashSet<int> visitedUserIds)
        {
            foreach (var comment in comments)
            {
                if (!visitedUserIds.Contains(comment.User.Id))
                {
                    visitedUserIds.Add(comment.User.Id);
                    if (HasUserVisitedLocation(comment.User.Id, locationId, comments, visitedUserIds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public bool HasUserVisitedLocation(int userId, int locationId, Comment comment)
        {
            if (comment.User.Id == userId)
            {
                return HasUserVisitedLocation(userId, locationId);
            }

            return false;
        }






        public List<string> GetLocations()
        {
            List<string> locations = new List<string>();
            foreach (Location l in _locationRepository.GetAll())
            {
				locations.Add(l.Country.ToString() + " " + l.City.ToString());
            }
            return locations;
        }
      
		public Location GetLocationByCityandCountry(String City,String Country)
        {
			List<Location> AllLocaitons = _locationRepository.GetAll();
			foreach(Location loc in AllLocaitons)
            {
				if(loc.City.Equals(City) && loc.Country.Equals(Country))
                {
					return loc;
                }
            }
			return null;
        }
    



		public Dictionary<Location, int> CalculateReservationCountByLocation(List<Accommodation> accommodations, List<AccommodationReservation> reservations)
		{
			Dictionary<Location, int> reservationCountByLocation = new Dictionary<Location, int>();

			foreach (var accommodation in accommodations)
			{
				reservationCountByLocation[accommodation.Location] = 0;
			}

			foreach (var reservation in reservations)
			{
				Accommodation accommodation = accommodations.FirstOrDefault(a => a.Id == reservation.IdAccommodation);

				if (accommodation != null)
				{
					reservationCountByLocation[accommodation.Location]++;
				}
			}

			return reservationCountByLocation;
		}

		public Location FindBusiestLocation(Dictionary<Location, int> reservationCountByLocation)
		{
			var busiestLocation = reservationCountByLocation.OrderByDescending(x => x.Value).FirstOrDefault();
			return busiestLocation.Key;
		}


		public List<Location> FindWorstLocations(Dictionary<Location, int> reservationCountByLocation)
		{
			int minReservationCount = reservationCountByLocation.Values.Min();
			List<Location> worstLocations = reservationCountByLocation
				.Where(x => x.Value == minReservationCount)
				.Select(x => x.Key)
				.ToList();
			return worstLocations;
		}

		//lokacije gde imam smestaj
		public List<Location> GetUniqueLocations(List<Accommodation> accommodations)
		{
			
			List<Location> uniqueLocations = new List<Location>();

			foreach (var accommodation in accommodations)
			{
				if (!uniqueLocations.Contains(accommodation.Location))
				{
					uniqueLocations.Add(accommodation.Location);
				}
			}

			return uniqueLocations;
		}

	

	}
}
