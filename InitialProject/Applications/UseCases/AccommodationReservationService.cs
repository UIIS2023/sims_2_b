using InitialProject.Applications.DTO;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
	internal class AccommodationReservationService
	{
		private readonly IAccommodationReservationRepository accommodationReservationRepository;

		private readonly GuestReviewService guestReviewService;

		private readonly AccommodationService accommodationService;

		private readonly UserService userService;

		private readonly ReservationDisplacementRequestService reservationDisplacementRequestService;

		private readonly RecommendationService recommendationOnAccommodationService;



		public AccommodationReservationService()
		{
			userService = new UserService();
			accommodationReservationRepository = Inject.CreateInstance<IAccommodationReservationRepository>();
			accommodationService = new AccommodationService();
			guestReviewService = new GuestReviewService();
			reservationDisplacementRequestService = new ReservationDisplacementRequestService();
			recommendationOnAccommodationService = new RecommendationService();


		}

        public List<AccommodationReservation> GetFilteredReservations(User user)
        {
            List<AccommodationReservation> reservations = GetByOwnerId(user.Id);

            List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (AccommodationReservation res in reservations)
            {
                if (!IsElegibleForReview(today, res)) continue;
                filteredReservations.Add(res);

            }

            return filteredReservations;
        }


        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
		{
			return accommodationReservationRepository.Save(accommodationReservation);
		}

		public void BindData(List<AccommodationReservation> reservations)
		{

			foreach (AccommodationReservation res in reservations)
			{
				res.Guest = userService.GetById(res.IdGuest);
				res.Accommodation = accommodationService.GetById(res.IdAccommodation);
			}

		}

		public void BindParticularData(AccommodationReservation reservation)
		{
			reservation.Guest = userService.GetById(reservation.IdGuest);
			reservation.Accommodation = accommodationService.GetById(reservation.IdAccommodation);
			
		}

		public List<AccommodationReservation> GetAll()
		{
			List<AccommodationReservation> reservations = new List<AccommodationReservation>();
			reservations = accommodationReservationRepository.GetAll();
			if(reservations.Count > 0)
			{
				BindData(reservations);
			}
			
			return reservations;
		}

		public bool IsElegibleForReview(DateOnly today, AccommodationReservation res)
		{
			List<GuestReview> guestReviews = guestReviewService.GetAll(); ;

			bool toAdd = true;
			foreach (GuestReview review in guestReviews)
			{

				if (res.Id == review.IdReservation)
				{
					toAdd = false;
					break;
				}

			}

			return res.EndDate < today && today.DayNumber - res.EndDate.DayNumber <= 5 && toAdd;
		}

		public AccommodationReservation GetById(int id)
		{
			AccommodationReservation reservation = accommodationReservationRepository.GetById(id);
			if(reservation != null)
			{
				BindParticularData(reservation);
			}
			
			return reservation;
		}

		public List<DateOnly> GetAllStartDates(int id)
		{
			List<DateOnly> dates = new List<DateOnly>();
			List<AccommodationReservation> reservations1;
			reservations1 = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
			foreach (AccommodationReservation reservation in reservations1)
			{
				if (reservation.IdAccommodation == id)
				{
					dates.Add(reservation.StartDate);
				}
			}
			return dates;
		}

		public DateOnly startDate(int id)
		{
			List<AccommodationReservation> reservations1;
			reservations1 = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
			foreach (AccommodationReservation a in reservations1)
			{
				if (a.Id== id)
				{
					return a.StartDate;
				}
			}
			throw new ArgumentException("The specified reservation was not found in the collection.");
		}

		public DateOnly endDate(int id)
		{
			List<AccommodationReservation> reservations1;
			reservations1 = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
			foreach (AccommodationReservation a in reservations1)
			{
				if (a.Id == id)
				{
					return a.EndDate;
				}
			}
			throw new ArgumentException("The specified reservation was not found in the collection.");
		}

		public List<DateOnly> GetAllEndDates(int id)
		{
			List<AccommodationReservation> reservations1;
			reservations1 = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
			List<DateOnly> dates = new List<DateOnly>();
			foreach (AccommodationReservation reservation in reservations1)
			{
				if (reservation.IdAccommodation == id)
				{
					dates.Add(reservation.EndDate);
				}
			}
			return dates;
		}

		public List<AccommodationReservation> GetByAccommodationId(int idAccommodation)
		{
			List<AccommodationReservation> reservations = accommodationReservationRepository.GetByAccommodationId(idAccommodation);
			if(reservations.Count != 0)
			{
				BindData(reservations);
			}
			return reservations;
		}

		public void Delete(AccommodationReservation accommodationReservation)
		{

			 accommodationReservationRepository.Delete(accommodationReservation);
		}

		public List<AccommodationReservation> GetOverlappingReservations(int accommodationId, DateOnly NewStartDate, DateOnly NewEndDate, List<AccommodationReservation> reservations)
		{
			List<AccommodationReservation> overlappingReservations = new List<AccommodationReservation>();
			 overlappingReservations = reservations.Where(r => r.IdAccommodation == accommodationId && r.EndDate >= NewStartDate && r.StartDate <= NewEndDate).ToList();

			return overlappingReservations;
		}

		public AccommodationReservation Update(AccommodationReservation accommodationReservation)
		{
			return accommodationReservationRepository.Update(accommodationReservation);
		}


		public List<AccommodationReservation> GetByOwnerId(int id)
		{
			List<AccommodationReservation> reservations = new List<AccommodationReservation>();
			List<AccommodationReservation> AllReservations = accommodationReservationRepository.GetAll();
			BindData(AllReservations);

			foreach(AccommodationReservation r in AllReservations)
			{
				if (r.Accommodation.IdUser == id && r.IsCanceled==false)
				{
					reservations.Add(r);
				}
			}

			return reservations;
		}

		public List<AccommodationReservation> GetByUser(User user)

		{
			List<AccommodationReservation> reservations = new List<AccommodationReservation>();
			reservations = accommodationReservationRepository.GetByUser(user);
			if (reservations.Count > 0)
			{
				BindData(reservations);
			}

			return reservations;
		}

		public List<int> GetYearsForAccommodation(int accommodationId)
		{
			List<int> years = new List<int>();

			List<AccommodationReservation> reservations = accommodationReservationRepository.GetByAccommodationId(accommodationId);
			if(reservations.Count > 0)
			{
				BindData(reservations);
			}
			
			foreach(AccommodationReservation r in reservations)
			{
				if (!years.Contains(r.StartDate.Year))
				{
					years.Add(r.StartDate.Year);
				}
				
			}
			return years;
		}

		private List<AccommodationReservation> GetReservationsByYear(int year, int accommodationId)
		{
			List<AccommodationReservation> reservations = new List<AccommodationReservation>();
			List<AccommodationReservation> allreservations = accommodationReservationRepository.GetAll();
			if(allreservations.Count > 0)
			{
				BindData(allreservations);
			}
			
			foreach(AccommodationReservation reservation in allreservations)
			{
				if(reservation.IdAccommodation==accommodationId && reservation.StartDate.Year==year )
				{
					reservations.Add(reservation);
				}
			}

			return reservations;
		}

		private List<AccommodationReservation> GetCancelledReservationsByYear(int year, int accommodationId)
		{
			List<AccommodationReservation> reservations = accommodationReservationRepository.GetAll();
			if (reservations.Count > 0)
			{
				BindData(reservations);
			}
			return reservations.FindAll(r => r.IdAccommodation == accommodationId && r.StartDate.Year == year && r.IsCanceled == true);
		}

	    private int GetNumberOfReservationByYear(int year, int accommodationId)

		{
			int count = 0;

			List<AccommodationReservation> reservations = accommodationReservationRepository.GetByAccommodationId(accommodationId);
			if (reservations.Count > 0)
			{
				BindData(reservations);
			}

			foreach(AccommodationReservation r in reservations)
			{
				if(year == r.StartDate.Year && !r.IsCanceled)
				{
					count++;
				}
			}

			return count;
		}

		private int GetNumberOfReservationsByMonth(int month, int year, int accommodationId)
		{
			int count = 0;
			
			List<AccommodationReservation> reservations = GetReservationsByYear(year, accommodationId);

			foreach(AccommodationReservation r in reservations)
			{
				if (r.StartDate.Month == month)
				{
					count++;
				}
			}

			return count++;
		}

		private int GetNumberOfCancelReservationByYear(int year, int accommodationId)
		{
			int count = 0;

			List<AccommodationReservation> reservations = accommodationReservationRepository.GetByAccommodationId(accommodationId);
			if (reservations.Count > 0)
			{
				BindData(reservations);
			}

			foreach (AccommodationReservation r in reservations)
			{
				if (year == r.StartDate.Year && r.IsCanceled)
				{
					count++;
				}
			}

			return count;
		}

		private int GetNumberOfCancelledReservationsByMonth(int month, int year, int accommodationId)
		{
			int count = 0;

			List<AccommodationReservation> reservations = GetCancelledReservationsByYear(year, accommodationId);

			foreach (AccommodationReservation r in reservations)
			{
				if (r.StartDate.Month == month)
				{
					count++;
				}
			}

			return count++;
		}
		private List<int> GetMonthsByYear(int year, int accommodationId)
		{
			List<int> months = new List<int>();

			List<AccommodationReservation> reservation = GetReservationsByYear(year, accommodationId);

			foreach(AccommodationReservation r in reservation)
			{
				if (!months.Contains(r.StartDate.Month))
				{
					months.Add(r.StartDate.Month);
				}
			}

			return months;
		}

		public List<YearlyStatisticsDTO> GetYearlyStatistics(int accommodationId)
		{
			List<YearlyStatisticsDTO> statistics = new List<YearlyStatisticsDTO>();

			List<int> years = GetYearsForAccommodation(accommodationId);

			foreach (int year in years)
			{
				int reservations = GetNumberOfReservationByYear(year, accommodationId);
				int cancelReservations = GetNumberOfCancelReservationByYear(year, accommodationId);
				int movedReservations = reservationDisplacementRequestService.GetNumberOfRequestsByYear(year, accommodationId);
				int recommendations = recommendationOnAccommodationService.GetNumberOfRecommendationsByYear(year, accommodationId);

				statistics.Add(new YearlyStatisticsDTO(year, reservations, cancelReservations, movedReservations, recommendations));

			}

			return statistics;
		}

		public List<MonthlyStatisticsDTO> GetMonthlyStatistics(int year, int accommodationId)
		{
			List<MonthlyStatisticsDTO> statistics = new List<MonthlyStatisticsDTO>();

			List<int> months = GetMonthsByYear(year, accommodationId);

			foreach(int month in months)
			{
				int reservations = GetNumberOfReservationsByMonth(month, year, accommodationId);
				int cancelledReservations = GetNumberOfCancelledReservationsByMonth(month, year, accommodationId);
				int movedReservations = reservationDisplacementRequestService.GetNumberOfRequestsByMonth(month, year, accommodationId);
				int recommendations = recommendationOnAccommodationService.GetNumberOfRecommendationsByMonth(month, year, accommodationId);

				statistics.Add(new MonthlyStatisticsDTO(year, month, reservations, cancelledReservations, movedReservations, recommendations));
			}

			return statistics;
		}

		public int GetBusiestmonth(int accommodationId, int year)
		{
			List<int> months = GetMonthsByYear(year, accommodationId);

			List<DateOnly> reservedDays = GetReservedDays(accommodationId);

			int MaxDays = 0;
			int MaxMonth = 0;
		

			foreach(int month in months)
			{
				int reservationsInMonth = reservedDays.Count(d => d.Month == month);

				if (reservationsInMonth > MaxDays)
				{
					MaxDays = reservationsInMonth;
					MaxMonth = month;
					
				}
				
			}

			return MaxMonth;
		}

		public int GetBusiestYear(int accommodationId)
		{
			List<AccommodationReservation> reservations = GetByAccommodationId(accommodationId);

			int MaxDays = 0;
			int MaxYear = 0;

			Dictionary<int, int> yearTotals = new Dictionary<int, int>();

			foreach (var reservation in reservations)
			{
				int year = reservation.StartDate.Year;
				int daysBooked = reservation.DaysNum;

				if (yearTotals.ContainsKey(year))
				{
					yearTotals[year] += daysBooked;
				}
				else
				{
					yearTotals[year] = daysBooked;
				}

				if(yearTotals[year]> MaxDays)
				{
					MaxDays = yearTotals[year];
					MaxYear = year;
				}

				
			}
			return MaxYear;
		}

		public List<DateOnly>  GetReservedDays(int accommodationId)
		{
			List<AccommodationReservation> reservations = GetByAccommodationId(accommodationId);

			List<DateOnly> dates = new List<DateOnly>();

			foreach(AccommodationReservation r in reservations)
			{
				for(var date = r.StartDate; date <= r.EndDate; date = date.AddDays(1))
				{
					dates.Add(date);
				}
			}

			return dates;
		}

		private List<DateOnly> GetReservedDatesForAllAcc(List<AccommodationReservation> reservations)
		{
			List<DateOnly> reservedDates = new List<DateOnly>();

			foreach (AccommodationReservation reservation in reservations)
			{
				for (DateOnly date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
				{
					reservedDates.Add(date);
				}
			}

			return reservedDates;
		}

		public bool IsDateReserved( List<DateOnly> reservedDates, DateOnly startDate, int daysNumber)
		{
			for(int i = 0; i<daysNumber; i++)
			{
				DateOnly date = startDate.AddDays(i);

				if (reservedDates.Contains(date))
				{
					return true;
				}
			}

			return false;
		}

		
		public List<ReservationPeriodDTO> GetAvailableAccommodations(DateOnly startDate, DateOnly endDate, int daysNum, int numGuests)
		{
			List<ReservationPeriodDTO> availableAccommodations = new List<ReservationPeriodDTO>();
			List<Accommodation> accommodations = accommodationService.GetAll();

			if (startDate == DateOnly.FromDateTime(DateTime.Today) && endDate == DateOnly.FromDateTime(DateTime.Today))
			{
				// Case when the guest doesn't enter a specific date range
				foreach (Accommodation accommodation in accommodations)
				{
					if (accommodation.MaxGuestNum >= numGuests)
					{
						List<AccommodationReservation> reservedAccommodations = GetByAccommodationId(accommodation.Id);
						List<DateOnly> reservedDates = GetReservedDatesForAllAcc(reservedAccommodations);

						bool isAvailable = true;
						DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today); // Starting from today

						while (currentDate <= DateOnly.FromDateTime(DateTime.Today.AddDays(20))) // Searching for available accommodations within a year range
						{
							if (!IsDateReserved(reservedDates, currentDate, daysNum))
							{
								ReservationPeriodDTO reservationPeriod = new ReservationPeriodDTO(
									currentDate, currentDate.AddDays(daysNum - 1), accommodation.Name,accommodation.Location.City,accommodation.Location.Country);
								availableAccommodations.Add(reservationPeriod);
								break; // Break the loop after adding the reservation period
							}

							currentDate = currentDate.AddDays(1);
						}
					}
				}
			}
			else
			{
				// Case when the guest enters a specific date range
				foreach (Accommodation accommodation in accommodations)
				{
					if (accommodation.MaxGuestNum >= numGuests)
					{
						List<AccommodationReservation> reservedAccommodations = GetByAccommodationId(accommodation.Id);
						List<DateOnly> reservedDates = GetReservedDatesForAllAcc(reservedAccommodations);

						bool isAvailable = true;
						DateOnly currentDate = startDate;

						while (currentDate.AddDays(daysNum - 1) <= endDate)
						{
							if (!IsDateReserved(reservedDates, currentDate, daysNum))
							{
								ReservationPeriodDTO reservationPeriod = new ReservationPeriodDTO(
									currentDate, currentDate.AddDays(daysNum - 1), accommodation.Name, accommodation.Location.City, accommodation.Location.Country);
								availableAccommodations.Add(reservationPeriod);
								break; // Break the loop after adding the reservation period
							}

							currentDate = currentDate.AddDays(1);
						}
					}
				}
			}

			return availableAccommodations;
		}

		public List<Accommodation> GetAccommodationsByUser(User user)
		{
			List<Accommodation> accommodations = new List<Accommodation>();
			List<AccommodationReservation> reservations = GetByUser(user);

			foreach(AccommodationReservation r in reservations)
			{
				accommodations.Add(r.Accommodation);
			}
			return accommodations;
		}

		public List<AccommodationReservation> GetReservationsForReport(User user, Accommodation accommodation, DateOnly startDate, DateOnly endDate)
		{
			List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();

			List<AccommodationReservation> usersReservations = GetByOwnerId(user.Id);

			foreach(AccommodationReservation r in usersReservations)
			{
				if(r.Accommodation==accommodation && r.StartDate >= startDate && r.EndDate <= endDate)
				{
					filteredReservations.Add(r);
				}
			}

			return filteredReservations;
		}




	}
}

