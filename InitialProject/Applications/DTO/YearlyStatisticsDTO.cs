using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.DTO
{
	public class YearlyStatisticsDTO
	{
		public int Year { get; set; }

		public int ReservationNum { get; set; }

		public int CanceledReservationNum { get; set; }

		public int MovedReservationNum { get; set; }

		public int RecommendationNum { get; set; }

		public YearlyStatisticsDTO(int year, int reservationNum, int canceledReservationNum, int movedReservationNum, int recommendationNum)
		{
			Year = year;
			ReservationNum = reservationNum;
			CanceledReservationNum = canceledReservationNum;
			MovedReservationNum = movedReservationNum;
			RecommendationNum = recommendationNum;
		}
	}
}
