using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.DTO
{
    public class ReservationPeriodDTO
    {
		public DateOnly StartDate { get; set; }

		public String Name { get; set; }
		public DateOnly EndDate { get; set; }

		public String City { get; set; }
		public String Country { get; set; }

		public ReservationPeriodDTO(DateOnly startDate, DateOnly endDate,String name,String city, String country)
		{
			StartDate = startDate;
			EndDate = endDate;
			Name = name;
			City = city;
			Country = country;
		}
	}
}
