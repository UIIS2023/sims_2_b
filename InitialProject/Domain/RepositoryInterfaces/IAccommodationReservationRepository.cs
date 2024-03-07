using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface IAccommodationReservationRepository : IRepository<AccommodationReservation>
	{
		List<AccommodationReservation> GetByAccommodationId(int idAccommodation);
		public List<AccommodationReservation> GetByUser(User user);
	}
}
