using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitialProject.Applications.UseCases
{
    public class ComplexTourRequestService
    {
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;
        private readonly UserService _userService;
        private readonly TourRequestService _tourRequestService;
        //private readonly LocationService _locationService;

        public ComplexTourRequestService()
        {
            _complexTourRequestRepository = Inject.CreateInstance<IComplexTourRequestRepository>();
            _userService = new UserService();
            _tourRequestService = new TourRequestService();
            //_locationService = new LocationService();
        }

        public List<ComplexTourRequests> GetAll()
        {
            List<ComplexTourRequests> tourRequests = _complexTourRequestRepository.GetAll();
            return BindData(tourRequests);
        }

        public List<ComplexTourRequests> GetAllByUser(User user)
        {
            List<ComplexTourRequests> tourRequests = new List<ComplexTourRequests>();
            foreach(ComplexTourRequests tR in _complexTourRequestRepository.GetAll())
            {
                if(tR.GuestId == user.Id)
                {
                    tourRequests.Add(tR);
                }
            }
            return BindData(tourRequests);
          }
        public List<ComplexTourRequests> GetOnHoldAndNotAttended(User guide)
        {
            List<ComplexTourRequests> requests = new List<ComplexTourRequests>();
            foreach(ComplexTourRequests comReq in GetAll())
            {
                if((comReq.Status == RequestType.OnHold) && (!_tourRequestService.CheckInComplexAttendance(guide, comReq.Id)))
                {
                    requests.Add(comReq);
                }
            }
            return requests;
        }

        public List<ComplexTourRequests> BindData(List<ComplexTourRequests> requests)
        {
            foreach (ComplexTourRequests tr in requests)
            {
                tr.Guest = _userService.GetById(tr.GuestId);
            }
            return requests;
        }

        public void Delete(ComplexTourRequests complexTourRequest)
        {
            _complexTourRequestRepository.Delete(complexTourRequest);
        }

        public ComplexTourRequests Save(ComplexTourRequests complexTourRequest)
        {
            return _complexTourRequestRepository.Save(complexTourRequest);
        }

        public ComplexTourRequests Update(ComplexTourRequests complexTourRequest)
        {
            return _complexTourRequestRepository.Update(complexTourRequest);
        }

        public ComplexTourRequests GetById(int id)
        {
            return _complexTourRequestRepository.GetById(id);
        }

    }
}
