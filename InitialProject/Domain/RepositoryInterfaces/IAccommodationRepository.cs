using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface IAccommodationRepository : IRepository<Accommodation>
	{
		List<Accommodation> GetByUser(User user);

		List<string> GetAccommodationNames();

	}
}
