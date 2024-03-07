using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface IImageRepository : IRepository<Image>
	{
		List<String> GetUrlByTourId(int id);

		List<String> GetUrlByAccommodationId(int id);

		void StoreImage(Accommodation savedAccommodation, string ImageUrl);
	}
}
