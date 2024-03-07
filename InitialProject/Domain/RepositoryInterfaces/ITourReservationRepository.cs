using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository : IRepository<TourReservation>
    {
        List<TourReservation> GetByUser(User user);

        List<TourReservation> GetByTour(int idTour);

    }
}
