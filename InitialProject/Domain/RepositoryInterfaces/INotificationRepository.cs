using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface INotificationRepository : IRepository<Notifications>
	{
		List<Notifications> GetByUserId(int id);

		List<Notifications> GetUnreadedAndTodaysNotifications(int userId);

		List<Notifications> GetNotificationsAboutRequests(int userId);

        List<Notifications> GetNotificationsAboutTourRequests(int userId);

		List<Notifications> GetNotificationsAboutCreatedTours(int userId);
        List<Notifications> GetNotificationsAboutVouchers(int userId);

		 List<Notifications> GetNotificationsAboutForum(int userId);


	}
}
