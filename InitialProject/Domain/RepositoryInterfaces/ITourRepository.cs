using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRepository: IRepository<Tour>
    {
        List<Tour> GetByUser(User user);
        string GetTourNameById(int id);
        List<Tour> GetAllByUserAndDate(User user, DateTime currentDay);
        Location GetLocationById(int id);

    }
}
