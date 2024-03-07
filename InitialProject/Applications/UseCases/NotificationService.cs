using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Serializer;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.Applications.UseCases
{    
	public class NotificationService
	{
		private readonly INotificationRepository _notificationRepository;

		private readonly AccommodationReservationService accommodationReservationService;

		private readonly ReservationDisplacementRequestService reservationDisplacementRequestService;

		private readonly TourRequestService tourRequestService;

		private readonly TourService tourService;

		private readonly TourAttendanceService tourAttendanceService;

		private readonly VoucherService voucherService;
		private readonly ForumService forumService;


        public NotificationService()
		{
			_notificationRepository = Inject.CreateInstance<INotificationRepository>();
			accommodationReservationService = new AccommodationReservationService();
			reservationDisplacementRequestService = new ReservationDisplacementRequestService();
			tourRequestService = new TourRequestService();
			tourService = new TourService();
			tourAttendanceService = new TourAttendanceService();
			voucherService = new VoucherService();
			forumService=new ForumService();
		}

		public Notifications GenerateNotificationAboutGuestRating(User user, AccommodationReservation reservation)
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			string title = "Guest rating notice";
			string content = $"Please note that you have not rated a guest named {reservation.Guest.Username}. The deadline for evaluation is {6-(today.DayNumber - reservation.EndDate.DayNumber)} days and you can do so by clicking on the button on the right.";


			Notifications existingNotification = _notificationRepository.GetByUserId(user.Id).FirstOrDefault(n => n.Content == content);


			if (existingNotification != null )
			{
				return null;
			}

			
			return new Notifications(user.Id, title, content, NotificationType.RateGuest, false, today);
			
		}


		private Notifications GenerateNotificationsAboutRequests(User user)
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			string title = "Notification of the request to move the reservation";
			string content = "There are still requests to move reservations that are pending. Click the button next to it and approve or deny the request";

			Notifications existingNotification = _notificationRepository.GetByUserId(user.Id).FirstOrDefault(n => n.Content == content);

			if(existingNotification != null)
			{
				return null;
			}

			return new Notifications(user.Id, title, content, NotificationType.CheckRequests, false,today);
		}

		private Notifications GenerateNotificationsAboutForum(User user, Forums forum)
		{
			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			string title = "New forum announcement";
			string content = $"Some guest has just opened new forum for {forum.Location.City}. You have accommodation there, so you can leave comments or just read them.";

			Notifications existingNotification = _notificationRepository.GetByUserId(user.Id).FirstOrDefault(n => n.Content == content);

			if(existingNotification != null)
			{
				if (existingNotification.IsRead)
				{
					return null;
				}
			}
			

			return new Notifications(user.Id, title, content, NotificationType.Forum, false, today);
		}



		public List<Notifications> NotifyOwner1(User user)
		{
			List<AccommodationReservation> reservations = accommodationReservationService.GetFilteredReservations(user);

			List<Notifications> myNotifications = _notificationRepository.GetUnreadedAndTodaysNotifications(user.Id);

			DateOnly today = DateOnly.FromDateTime(DateTime.Now);

			

			foreach (AccommodationReservation res in reservations)
			{

                Notifications notif = GenerateNotificationAboutGuestRating(user,res);
				if(notif != null)
				{
					Notifications savedNotif = _notificationRepository.Save(notif);
					myNotifications.Add(savedNotif);
				}
				  
				
			}


			return myNotifications;
		}

		public List<Notifications> NotifyOwner2(User user)
		{
			List<Notifications> notifications = _notificationRepository.GetNotificationsAboutRequests(user.Id);

			List<ReservationDisplacementRequest> requests = reservationDisplacementRequestService.GetByOwnerId(user.Id);

			List<ReservationDisplacementRequest> onHoldRequests = requests.FindAll(r => r.Type == RequestType.OnHold);

			if(onHoldRequests.Count > 0)
			{
				Notifications notif = GenerateNotificationsAboutRequests(user);
				if(notif != null)
				{
					Notifications savedNotif = _notificationRepository.Save(notif);
					notifications.Add(savedNotif);
				}
			}

			return notifications;
		}

		public List<Notifications> NotifyOwner3(User user)
		{
			List<Notifications> notifications = new List<Notifications>();

			List<Forums> availableForums = forumService.GetAvailableForums(user);

			foreach (Forums forum in availableForums)
			{
				Notifications notif = GenerateNotificationsAboutForum(user, forum);
				
				if(notif != null)
				{
					Notifications savedNotif = _notificationRepository.Save(notif);
					notifications.Add(notif);
				}
			}

			return notifications;

		}

		public List<Notifications> NotifyOwner(User user)
		{
			var notifications1 = NotifyOwner1(user);
			var notifications2 = NotifyOwner2(user);
			var notification3 = NotifyOwner3(user);
			var allNotifications = notifications1.Concat(notifications2).Concat(notification3).ToList();
			return allNotifications;
		}



		public List<Notifications> GetByUserId(int id)
		{
			return _notificationRepository.GetByUserId(id);
		}

		public List<Notifications> GetAll()
		{
			return _notificationRepository.GetAll();
		}

		public Notifications Update(Notifications notification)
		{
			return _notificationRepository.Update(notification);
			
		}
    
		public List<Notifications> NotifyGuest2(User user)
			{
				var notifications1 = NotifyGuest21(user);
				var notifications2 = NotifyGuest22(user);
				var notifications3 = NotifyGuest23(user);
				var notifications = notifications1.Concat(notifications2).ToList();
				var allnotifications = notifications3.Concat(notifications).ToList();
				return allnotifications;
			}

        private List<Notifications> NotifyGuest23(User user)
        {
			int numAttendance = 0;
			int numWon = 0;
            List<Notifications> notifications = _notificationRepository.GetNotificationsAboutVouchers(user.Id);

			foreach(Tour tour in tourService.GetPastYearTours())
			{
                foreach (TourAttendance tourAttendance in tourAttendanceService.GetAllAttendedToursByUser(user))
                {
					if(tour.Id == tourAttendance.IdTour)
					{
                        numAttendance++;
                    }
                }
            }
			

			foreach(Voucher voucher in voucherService.GetUpcomingVouchers(user))
			{
				if(voucher.Name == "Won voucher")
				{
					numWon++;
				}
			}

			if(numAttendance >= 5 && numWon==0)
			{
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
				/*
                DateOnly futureDate = today.AddMonths(6);
				Voucher voucher = new Voucher(user.Id, "Won voucher", futureDate);

                Voucher savedVoucher = voucherService.Save(voucher);
				TourVouchersViewModel.VouchersMainList.Add(savedVoucher);*/

                Notifications notif = GenerateNotificationsAboutVouchers(user);
                if (notif != null)
                {
                    Notifications savedNotif = _notificationRepository.Save(notif);
                    notifications.Add(savedNotif);
                }
            }


			return notifications;
        }

        private Notifications GenerateNotificationsAboutVouchers(User user)
        {
			int count = voucherService.GetAll().Count();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            string title = "Notification of won vouchers";
            string content = $"Guest won {count +1} voucher. Click the button next to see more about this voucher";


            Notifications existingNotification = _notificationRepository.GetByUserId(user.Id).FirstOrDefault(n => n.Content == content);

            if (existingNotification != null)
            {
                return null;
            }


            return new Notifications(user.Id, title, content, NotificationType.VoucherWon, false, today);
        }

        private List<Notifications> NotifyGuest22(User user)
        {
            List<Notifications> notifications = _notificationRepository.GetNotificationsAboutCreatedTours(user.Id);

            List<TourRequest> requests = tourRequestService.GetAll();

            List<TourRequest> rejectedRequests = requests.FindAll(r => r.Status == RequestType.Rejected && r.Status != RequestType.RejectedCreated);

            if (rejectedRequests.Count>0)
             {
                foreach (TourRequest res in rejectedRequests)
                {
					foreach(Tour t in tourService.GetAllCreatedToursByRequest())
					{
						if((res.Location.Country == t.Location.Country && res.Location.City == t.Location.City) || res.TourLanguage==t.Language)
						{
                                Notifications notif = GenerateNotificationsAboutCreatedTours(user, res, t);
                                if (notif != null)
                                {
                                    Notifications savedNotif = _notificationRepository.Save(notif);
                                    notifications.Add(savedNotif);
                                }
                        }
					}
               }
            }

            return notifications;
        }

        private List<Notifications> NotifyGuest21(User user)
        {
            List<Notifications> notifications = _notificationRepository.GetNotificationsAboutTourRequests(user.Id);
				
            return notifications;
        }

		public void GenerateNotifications(TourRequest tourRequest, Tour tour)
		{
            string title1 = "Notification of the accepted request";
            string content1 = $"Guide accepted {tourRequest.Id}. requests. Click the button next to see more about this tour request";
           

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            
			Notifications notif1 = new Notifications(tourRequest.IdGuest, title1, content1, NotificationType.CheckAcceptedTourRequest, false, today);
            _notificationRepository.Save(notif1);

        }


		private Notifications GenerateNotificationsAboutCreatedTours(User user, TourRequest req, Tour tour)
		{
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            string title = "Notification of the created tours";
            string content = $"Guide created {tour.Id}. tour {tour.Name}. Click the button next to see more about this tour";

			req.Status = RequestType.RejectedCreated;
			tourRequestService.Update(req);

            Notifications existingNotification = _notificationRepository.GetByUserId(user.Id).FirstOrDefault(n => n.Content == content);

            if (existingNotification != null)
            {
                return null;
            }


            return new Notifications(user.Id, title, content, NotificationType.CheckCreatedTour, false, today);
        }

        public void Delete(Notifications notification)
        {

			_notificationRepository.Delete(notification);
        }

		public Tour GetTourByNotification(Notifications notification)
		{
            string sentence = notification.Content;
            int wordStart = 14; // start position of the word to be removed

            string beforeWord = sentence.Substring(0, wordStart);
            string afterWord = sentence.Substring(wordStart);

            // Find the position of the next space character
            int spacePos = afterWord.IndexOf('.');

            string wordToRemove = afterWord.Substring(0, spacePos);

			int idTour = int.Parse(wordToRemove);

            Tour tour = tourService.GetById(idTour);

            return tour;

        }

        public TourRequest GetTourRequestByNotification(Notifications notification)
        {
            string sentence = notification.Content;
            int wordStart = 15; // start position of the word to be removed

            string beforeWord = sentence.Substring(0, wordStart);
            string afterWord = sentence.Substring(wordStart);

            // Find the position of the next space character
            int spacePos = afterWord.IndexOf('.');

            string wordToRemove = afterWord.Substring(0, spacePos);

            int idTourRequest = int.Parse(wordToRemove);

            TourRequest tourRequest = tourRequestService.GetById(idTourRequest);

            return tourRequest;

        }
	}
}
