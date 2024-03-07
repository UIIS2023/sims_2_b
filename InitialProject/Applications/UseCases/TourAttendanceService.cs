using InitialProject.Repository;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Injector;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Windows.Forms;

namespace InitialProject.Applications.UseCases
{
    public class TourAttendanceService
    {
        private readonly ITourAttendanceRepository _tourAttendenceRepository;
        private readonly UserService _userService;
        private readonly TourPointService _tourPointService;
        private readonly TourReservationService _tourReservationService;
        private readonly ImageRepository _imageRepository;
        public TourAttendanceService()
        {
            _tourAttendenceRepository = Inject.CreateInstance<ITourAttendanceRepository>();
            _userService = new UserService();
            _tourPointService = new TourPointService();
            _tourReservationService = new TourReservationService();
            _imageRepository = new ImageRepository();
        }

        public TourAttendance Save(TourAttendance tourAttendance)
        {
            return _tourAttendenceRepository.Save(tourAttendance);
        }
        public List<TourAttendance> GetAll()
        {
            List<TourAttendance> tours = _tourAttendenceRepository.GetAll();
            return BindData(tours);
        }

        public void Delete(TourAttendance tourattendance)
        {
            _tourAttendenceRepository.Delete(tourattendance);
        }

        public List<TourAttendance> GetAllByTourId(int id)
        {
            List<TourAttendance> tours = GetAll();
            return tours.FindAll(t => t.IdTour == id);
        }

        private List<TourAttendance> BindData(List<TourAttendance> tours)
        {
            foreach(TourAttendance ta in tours)
            {
                ta.Guest = _userService.GetById(ta.IdGuest);
                ta.TourPointName = _tourPointService.GetTourPointNameByTourPointId(ta.IdTourPoint);
                ta.TourName = _tourReservationService.GetTourNameByTourId(ta.IdTour);
                ta.TourPoint = _tourPointService.GetById(ta.IdTourPoint);
                ta.ImageSource = _imageRepository.GetFirstUrlByTourId(ta.IdTour);
            }
            return tours;
        }

        public List<TourAttendance> GetAllAttendedToursByUser(User user)
        {
            List<TourAttendance> tours = _tourAttendenceRepository.GetAll();
            return BindData(tours.FindAll(t => t.IdGuest == user.Id));
        }

        public TourAttendance Update(TourAttendance tourattendance)
        {
            return _tourAttendenceRepository.Update(tourattendance);
        }

        public List<TourAttendance> GetAllByGuide(User user)
        {
            return _tourAttendenceRepository.GetAllByGuide(user);
        }


        public double FindWithVoucher(Tour tour)
        {
            double n = 0;
            double with = 0;
            foreach (TourAttendance ta in _tourAttendenceRepository.GetAll())
            {
                if (tour.Id == ta.IdTour)
                {
                    n++;
                    if (ta.UsedVoucher == true)
                    {
                        with++;
                    }
                }
            }
            return CalculateRes(n, with);
        }

        private double CalculateRes(double n, double with)
        {
            if (n == 0)
            {
                return 0;
            }

            double res = 100 * (with / n);
            return res;
        }


        public int FindYoungest(Tour tour)
        {
            int i = 0;
            foreach (TourAttendance ta in GetAllByTourId(tour.Id))
            {
                User user = _userService.GetById(ta.IdGuest);
                if (user.Age < 18)
                {
                    i++;
                }
            }
            return i;
        }

        public int FindMediumAge(Tour tour)
        {
            int i = 0;
            foreach (TourAttendance ta in GetAllByTourId(tour.Id))
            {
                User user = _userService.GetById(ta.IdGuest);
                if (user.Age >= 18 && user.Age <= 50)
                {
                    i++;
                }
            }
            return i;
        }

        public int FindOldestAge(Tour tour)
        {
            int i = 0;
            foreach (TourAttendance ta in GetAllByTourId(tour.Id))
            {
                User user = _userService.GetById(ta.IdGuest);
                if (user.Age > 50)
                {
                    i++;
                }
            }
            return i;
        }

        public List<TourAttendance> AllForReport(List<int> ids)
        {
            List<TourAttendance> tourAttendances = new List<TourAttendance>();
            foreach (TourAttendance tA in GetAll()) 
            {
                foreach(int id in ids)
                {
                    if(tA.IdTour == id)
                    {
                        tourAttendances.Add(tA);
                    }
                }
            }
            return BindData(tourAttendances);
        }

    }
}
