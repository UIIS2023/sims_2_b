using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Injector
{
    public class Inject
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
    {
        { typeof(IUserRepository), new UserRepository() },
        { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
        { typeof(IAccommodationRepository), new AccommodationRepository() },
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(IImageRepository), new ImageRepository() },
        { typeof(IOwnerReviewRepository), new OwnerReviewRepository() },
        { typeof(IGuestReviewRepository), new GuestReviewRepository() },
        { typeof(ITourPointRepository), new TourPointRepository() },
        { typeof(ITourAttendanceRepository), new TourAttendanceRepository()},
        { typeof(ITourRepository), new TourRepository()},
        { typeof(ITourReservationRepository), new TourReservationRepository()},
        { typeof(IVoucherRepository), new VoucherRepository()},
        { typeof(IReservationDisplacementRequestRepository), new ReservationDisplacementRequestRepository()},
        { typeof(ITourGuideReviewRepository), new TourGuideReviewRepository() },
        { typeof(ITourRequestsRepository), new TourRequestsRepository() },
        { typeof(IRecommendationRepository), new RecommendationRepository() },
        { typeof(INotificationRepository), new NotificationRepository() },
        { typeof(ISuperGuestRepository), new SuperGuestRepository() },
        { typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository() },
        {typeof(IForumRepository), new ForumRepository() },
         {typeof(ICommentRepository), new CommentRepository() },
        { typeof(IRenovationRepository), new RenovationRepository() },


    };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
