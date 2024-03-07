using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IReservationDisplacementRequestRepository:IRepository<ReservationDisplacementRequest>
    {
        public List<ReservationDisplacementRequest> GetByUser(User user);
    }
}
