using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.DTO
{
	public class RenovationPeriodDTO
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public RenovationPeriodDTO(DateOnly startDate,DateOnly endDate)
		{
			StartDate = startDate;
			EndDate = endDate;
		}
	}
}
