using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class ReservationDisplacementRequestService
    {
        private readonly IReservationDisplacementRequestRepository reservationDisplacementRequestRepository;

        
   
        public ReservationDisplacementRequestService()
        {
            reservationDisplacementRequestRepository = Inject.CreateInstance<IReservationDisplacementRequestRepository>();
      
            
        }

        public ReservationDisplacementRequest Save(ReservationDisplacementRequest request)
        {
           return reservationDisplacementRequestRepository.Save(request);   
        }

        public List<ReservationDisplacementRequest> GetAll()
        {
            List<ReservationDisplacementRequest> requests = reservationDisplacementRequestRepository.GetAll();
            if(requests.Count > 0)
			{
                BindData(requests);
            }
            
            return requests;
        }

        public void BindData(List<ReservationDisplacementRequest> requests)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            foreach (ReservationDisplacementRequest r in requests)
            {
               r.Reservation = accommodationReservationService.GetById(r.ReservationId);
            }

        }

       public void BindPaticularData(ReservationDisplacementRequest request)
		{
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            request.Reservation = accommodationReservationService.GetById(request.ReservationId);
		}

        public ReservationDisplacementRequest Update(ReservationDisplacementRequest request)
		{
            return reservationDisplacementRequestRepository.Update(request);
		}

        public List<ReservationDisplacementRequest> GetByOwnerId(int ownerId)
		{
            List<ReservationDisplacementRequest> requests = new List<ReservationDisplacementRequest>();
            List<ReservationDisplacementRequest> allRequests = reservationDisplacementRequestRepository.GetAll();
            if (allRequests.Count > 0)
            {
                BindData(allRequests);
            }

            foreach(ReservationDisplacementRequest r in allRequests)
			{
				if (r.Reservation.Accommodation.IdUser == ownerId && r.Reservation.IsCanceled == false)
				{
                    requests.Add(r);
				}
			}



         
            return requests;
		}


       public List<ReservationDisplacementRequest> GetByAccommodationId(int accommodationId)
		{
            List <ReservationDisplacementRequest> requests = new List<ReservationDisplacementRequest>();
            List <ReservationDisplacementRequest> allRequests = reservationDisplacementRequestRepository.GetAll();
            if(allRequests.Count > 0)
			{
                BindData(allRequests);
			}
           

            foreach(ReservationDisplacementRequest r in allRequests)
			{
				if (r.Reservation.IdAccommodation == accommodationId)
				{
                    requests.Add(r);
				}
			}

            return requests;
		}

        public List<ReservationDisplacementRequest> GetByUser(User user)
        {
            return reservationDisplacementRequestRepository.GetByUser(user);
        }

        public int GetNumberOfRequestsByYear(int year, int accommodationId)
		{
            int count = 0;

            List<ReservationDisplacementRequest> requests = GetByAccommodationId(accommodationId);

            foreach(ReservationDisplacementRequest r in requests)
			{
				if (r.Reservation.StartDate.Year == year)
				{
                    count++;
				}
			}

            return count;

		}

        public int GetNumberOfRequestsByMonth(int month, int year, int accommodationId)
		{
            int count = 0;

            List<ReservationDisplacementRequest> requests = GetByAccommodationId(accommodationId);

            foreach(ReservationDisplacementRequest r in requests)
			{
                if(r.Reservation.StartDate.Year==year && r.Reservation.StartDate.Month == month)
				{
                    count++;
				}
			}

            return count;
		}
    }
}
