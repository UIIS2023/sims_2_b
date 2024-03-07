using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
	public class NotificationRepository : INotificationRepository
	{
        public const string FilePath = "../../../Resources/Data/notifications.csv";

        private readonly Serializer<Notifications> _serializer;

        private List<Notifications> _notifications;

        public NotificationRepository()
        {
            _serializer = new Serializer<Notifications>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public List<Notifications> GetAll()
        {
            return _notifications;
        }

        public Notifications Save(Notifications notification)
        {
            notification.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

        public int NextId()
        {

            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(c => c.Id) + 1;
        }

        public void Delete(Notifications notification)
        {

            Notifications founded = _notifications.Find(n => n.Id == notification.Id);
            _notifications.Remove(founded);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public Notifications Update(Notifications notification)
        {

            Notifications current = _notifications.Find(n => n.Id == notification.Id);
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

     
        public Notifications GetById(int id)
        {

            return _notifications.Find(n => n.Id == id);
        }

        public List<Notifications> GetByUserId( int id)
		{
            return _notifications.FindAll(n=> n.UserId == id );
		}

        public List<Notifications> GetUnreadedAndTodaysNotifications(int userId)
		{
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            return _notifications.FindAll(n => n.UserId == userId && !n.IsRead && n.NotifDate==today && n.NotifType==NotificationType.RateGuest);
		}

        public List<Notifications> GetNotificationsAboutRequests(int userId)
		{
            return _notifications.FindAll(n => n.UserId==userId && n.NotifType==NotificationType.CheckRequests && !n.IsRead);
		}

        public List<Notifications> GetNotificationsAboutForum(int userId)

        {
            
            return _notifications.FindAll(n => n.UserId == userId && n.NotifType == NotificationType.Forum && !n.IsRead );
        }

        public List<Notifications> GetNotificationsAboutTourRequests(int userId)
        {
            return _notifications.FindAll(n => n.UserId==userId && n.NotifType==NotificationType.CheckAcceptedTourRequest && !n.IsRead);
        }

        public List<Notifications> GetNotificationsAboutCreatedTours(int userId)
        {
            return _notifications.FindAll(n => n.UserId==userId && n.NotifType==NotificationType.CheckCreatedTour && !n.IsRead);
        }

        public List<Notifications> GetNotificationsAboutVouchers(int userId)
        {
            return _notifications.FindAll(n => n.UserId==userId && n.NotifType==NotificationType.VoucherWon && !n.IsRead);
        }
    }
}
