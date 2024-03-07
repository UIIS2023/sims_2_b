using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourAttendanceRepository : IRepository<TourAttendance>
    {

        List<TourAttendance> GetAllByGuide(User user);

    }
}
