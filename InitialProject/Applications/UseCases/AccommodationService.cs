using InitialProject.Applications.DTO;
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
	internal class AccommodationService
	{
		private readonly IAccommodationRepository _accommodationRepository;


		public AccommodationService()
		{
			_accommodationRepository = Inject.CreateInstance<IAccommodationRepository>();

		}
		public List<Accommodation> GetByUser(User user)

		{
			List<Accommodation> accommodations = new List<Accommodation>();
			accommodations = _accommodationRepository.GetByUser(user);
			if (accommodations.Count > 0)
			{
				BindData(accommodations);
			}

			return accommodations;
		}

		public Accommodation GetById(int id)
		{
			Accommodation accommodation = _accommodationRepository.GetById(id);
			if (accommodation != null)
			{
				BindParticularData(accommodation);
			}

			return accommodation;

		}

		public Accommodation Save(Accommodation accommodation)
		{
			Accommodation savedAccommodation = _accommodationRepository.Save(accommodation);
			if (savedAccommodation != null)
			{
				BindParticularData(savedAccommodation);
			}

			return savedAccommodation;
		}

		private void BindData(List<Accommodation> accommodations)
		{
			LocationService locationService = new LocationService();

			foreach (Accommodation accommodation in accommodations)
			{
				accommodation.Location = locationService.GetById(accommodation.IdLocation);
			}

		}

		private void BindParticularData(Accommodation accommodation)
		{
			LocationService locationService = new LocationService();
			accommodation.Location = locationService.GetById(accommodation.IdLocation);
		}

		public List<Accommodation> GetAll()
		{
			return _accommodationRepository.GetAll();
		}
		public Accommodation GetAccommodationByName(String name)
		{
			List<Accommodation> accommodations = _accommodationRepository.GetAll();
			foreach (Accommodation a in accommodations)
			{
				if (a.Name == name)
				{
					return a;
				}
			}
			return null;
		}


		public List<string> GetAccommodationNames()
		{
			return _accommodationRepository.GetAccommodationNames();
		}


		public void Delete(Accommodation accommodation)
		{

			_accommodationRepository.Delete(accommodation);	
		}
	}	
}
