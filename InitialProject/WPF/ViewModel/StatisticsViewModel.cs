using InitialProject.Applications.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class StatisticsViewModel : ViewModelBase
	{
		public int Year { get; set; }

		public int Month { get; set; }

		public int ReservationNum { get; set; }

		public int CanceledReservationNum { get; set; }

		public int MovedReservationNum { get; set; }

		public int RecommendationNum { get; set; }

		public int TourRequestsCount { get; set; }
        public int PerMonthCount { get; set; }

        public static StatisticsViewModel MapToViewModel(YearlyStatisticsDTO dto)
		{
			return new StatisticsViewModel
			{
				Year = dto.Year,
				ReservationNum = dto.ReservationNum,
				CanceledReservationNum = dto.CanceledReservationNum,
				MovedReservationNum = dto.MovedReservationNum,
				RecommendationNum = dto.RecommendationNum,
			};
		}

		public static StatisticsViewModel MapToViewModel(MonthlyStatisticsDTO dto)
		{
			return new StatisticsViewModel
			{
				Year = dto.Year,
				Month = dto.Month,
				ReservationNum = dto.ReservationNum,
				CanceledReservationNum = dto.CanceledReservationNum,
				MovedReservationNum = dto.MovedReservationNum,
				RecommendationNum = dto.RecommendationNum,
			};
		}
	}
}
