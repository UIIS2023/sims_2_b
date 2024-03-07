using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.DTO
{
	public class MonthlyStatisticsDTO
	{
		public int Year { get; set; }

		public int Month { get; set; }

		public int ReservationNum { get; set; }

		public int CanceledReservationNum { get; set; }

		public int MovedReservationNum { get; set; }

		public int RecommendationNum { get; set; }

		public MonthlyStatisticsDTO(int year,int month, int reservationNum, int canceledReservationNum, int movedReservationNum, int recommendationNum)
		{
			Year = year;
			Month = month;
			ReservationNum = reservationNum;
			CanceledReservationNum = canceledReservationNum;
			MovedReservationNum = movedReservationNum;
			RecommendationNum = recommendationNum;
		}
	}
}
