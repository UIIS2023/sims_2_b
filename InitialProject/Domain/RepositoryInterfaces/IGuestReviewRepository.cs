using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface IGuestReviewRepository : IRepository<GuestReview>
	{
        public List<GuestReview> GetByUser(User user);
    }
}