using InitialProject.Applications.DTO;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
	public class RenovationService 
	{
		private readonly IRenovationRepository renovationRepository;

		private readonly AccommodationReservationService accommodationReservationService; 

		private readonly AccommodationService accommodationService;
		

		public RenovationService()
		{
			renovationRepository = Inject.CreateInstance<IRenovationRepository>();
			accommodationReservationService = new AccommodationReservationService();
			accommodationService = new AccommodationService();
		}

		public List<Renovation> GetByAccommodationId(int accommodationId)
		{
			return renovationRepository.GetByAccommodationId(accommodationId);
		}

		public List<DateOnly> GetRenovationDates(int accommodationId)
		{
			List<Renovation> renovations = GetByAccommodationId(accommodationId);

			List<DateOnly> dates = new List<DateOnly>();

			foreach (Renovation r in renovations)
			{
				for (var date = r.StartDate; date <= r.EndDate; date = date.AddDays(1))
				{
					dates.Add(date);
				}
			}

			return dates;
		}

		private bool IsDateInRenovationPeriod(List<DateOnly> renovationDates, DateOnly startDate, int daysNumber)
		{
			for(int i=0; i<daysNumber; i++)
			{
				DateOnly date = startDate.AddDays(i);
				if (renovationDates.Contains(date))
				{
					return true;
				}
			}

			return false;
		}

		private bool IsDateAvailable(List<DateOnly> renovationDates, List<DateOnly> reservedDates, DateOnly startDate, int daysNum)
		{
			if(!IsDateInRenovationPeriod(renovationDates,startDate,daysNum) && !accommodationReservationService.IsDateReserved(reservedDates, startDate, daysNum))
			{
				return true;
			}

			return false;
		}

		public List<RenovationPeriodDTO> GetAvailableDatesForRenovation(List<DateOnly> renovationDates, List<DateOnly> reservedDates, DateOnly startDate, DateOnly endDate, int daysNum)
		{
			List<RenovationPeriodDTO> dates = new List<RenovationPeriodDTO>();

			while(startDate <= endDate.AddDays(-daysNum + 1))
			{
				if (IsDateAvailable(renovationDates, reservedDates, startDate, daysNum))
				{
					RenovationPeriodDTO renovationPeriod = new RenovationPeriodDTO(startDate, startDate.AddDays(daysNum - 1));
					dates.Add(renovationPeriod);
				}

				startDate = startDate.AddDays(1);
			}

			return dates;
		}

		public Renovation Save(Renovation renovation)
		{
			return renovationRepository.Save(renovation);
		}

		private void BindData(List<Renovation> renovations)
		{
			foreach(Renovation r in renovations)
			{
				r.Accommodation = accommodationService.GetById(r.AccommodationId);
			}
		}

		public List<Renovation> GetAll()
		{
			List<Renovation> renovations = renovationRepository.GetAll();
			if (renovations.Count > 0)
			{
				BindData(renovations);
			}

			return renovations;
		}

		public void SetAbilityForCancel(ObservableCollection<Renovation> renovations)
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			

			foreach (Renovation r in renovations)
			{
				if(r.StartDate.DayNumber - today.DayNumber <= 5 || r.EndDate<today)
				{
					r.IsEnabledForCancel = false;
					renovationRepository.Update(r);
				}
				else
				{
					r.IsEnabledForCancel = true;
					renovationRepository.Update(r);
				}
			}
		}

		public void Delete(Renovation renovation)
		{
			renovationRepository.Delete(renovation);
		}

		public void SetRenovationStatus(ObservableCollection<Renovation> renovations)
		{
			foreach(Renovation r in renovations)
			{
				if(r.EndDate < DateOnly.FromDateTime(DateTime.Now))
				{
					r.IsRenovated = true;
					renovationRepository.Update(r);
				}
				else
				{
					r.IsRenovated = false;
					renovationRepository.Update(r);
				}
			}
		}
	}
}
