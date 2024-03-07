using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.Applications.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly VoucherService _voucherService;
        private TourPointService _tourPointService;
        private TourReservationService _tourReservationService;
        private TourAttendanceService _tourAttendenceService;
        private LocationService _locationService;
        private readonly ImageRepository _imageRepository;
        private UserService _userService;



        public TourService()
        {
            _tourRepository = Inject.CreateInstance<ITourRepository>();
            _tourPointService= new TourPointService();
            _tourReservationService = new TourReservationService();
            _tourAttendenceService = new TourAttendanceService();
            _voucherService = new VoucherService();
            _locationService= new LocationService();
            _imageRepository = new ImageRepository();
            _userService = new UserService();
        }

        public List<Tour> BindData(List<Tour> tours)
        {
            foreach(Tour t in tours)
            {
                t.Location = _locationService.GetById(t.IdLocation);
                t.ImageSource = _imageRepository.GetFirstUrlByTourId(t.Id);
            }
            return tours;
        }

        public List<Tour> GetUpcomingToursByUser(User user)
        {
            List<Tour> Tours = new List<Tour>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (Tour tour in GetAllByUser(user))
            {
                if (tour.Date.CompareTo(today) >= 0 && IsTimePassed(tour))
                {
                    Tours.Add(tour);
                }
            }
            return BindData(Tours);
        }

        public List<Tour> GetUpcomingTours()
        {
            List<Tour> tours = new List<Tour>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (Tour tour in GetAll())
            {
                if (tour.Date.CompareTo(today) >= 0 && IsTimePassed(tour))
                {
                    tours.Add(tour);
                }
            }

            tours = tours.OrderBy(t => IsSuperUser(t.IdUser)).ToList();

            return tours;
        }

        private bool IsSuperUser(int userId)
        {
            User user = _userService.GetById(userId);
            return user?.IsSuper == false;
        }

        public int GetNumOfUpcomingTours(User user)
        {
            List<Tour> Tours = GetUpcomingToursByUser(user);
            int n = 0;
            foreach(Tour tour in Tours)
            {
                n++;
            }
            return n;
        }

        public List<Tour> GetFinishedToursByUser(User user)
        {
            List<Tour> Tours = new List<Tour>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (Tour tour in GetAllByUser(user))
            {
                if (tour.Date.CompareTo(today) < 0)
                {
                    Tours.Add(tour);
                }
            }
            return Tours;
        }

        public List<Tour> GetAllByUser(User user)
        {
            List<Tour> tours = _tourRepository.GetByUser(user);
            tours = BindData(tours);
            return tours;
        }

        public List<Tour> GetActiveTour()
        {
            List<Tour> activeTour = new List<Tour>();
            List<Tour> tours = GetAll();
            foreach (Tour t in tours)
            {
                foreach (TourAttendance tourAttendance in _tourAttendenceService.GetAll())
                {
                    if (t.Id == tourAttendance.IdTour && t.Active==true)
                    {
                        if (activeTour.Count==0)
                        {
                            activeTour.Add(_tourRepository.GetById(t.Id));
                            activeTour = BindData(activeTour);
                        }

                    }
                }
            }
            return BindData(activeTour);
        }


        public Tour Update(Tour tour)
        {
            return _tourRepository.Update(tour);
        }
        public string GetTourNameById(int id)
        {
            return _tourRepository.GetTourNameById(id);
        }

        public Tour GetById(int id)
        {
            Tour tour = _tourRepository.GetById(id);
            tour.Location = _locationService.GetById(tour.IdLocation);
            return tour;
        }

        public bool IsTimePassed(Tour tour)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (tour.Date == today)
            {
                if (tour.StartTime <= currentTime) // ako bude trebalo za kasnije -> tour.StartTime.AddHours(time.Duration) <= currentTime
                    return false;
            }
            return true;
        }

        public void StartTour(Tour tour)
        {
            tour.Active = true;
            _tourPointService.ActivateFirstPoint(tour);
            _tourRepository.Update(tour);
        }


        public Tour Save(Tour tour)
        {
            Tour savedTour = _tourRepository.Save(tour);
            return savedTour;
        }

        public List<Tour> GetAllByUserAndDate(User user, DateTime currentDay)
        {

            List<Tour> tours = _tourRepository.GetAllByUserAndDate(user, currentDay);
            tours = BindData(tours);
            return tours;
        }


        public Location GetLocationById(int id)
        {
            return _tourRepository.GetLocationById(id);
        }

        public List<Tour> GetAll()
        {
            List<Tour> tours = _tourRepository.GetAll();
            tours= BindData(tours);
            return tours;
        }

        public void CancelTour(Tour tour)
        {
            _tourRepository.Delete(tour);
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (TourReservation tr in _tourReservationService.GetAll())
            { 
                if(tr.IdTour == tour.Id)
                {
                    Voucher voucher = new Voucher(tr.IdUser, "Cancellation voucher", today.AddYears(1));
                    _voucherService.Save(voucher);
                }
            }

            _tourReservationService.DeleteTour(tour);
        }

        public bool IsCancellationPossible(Tour tour)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            DateOnly futureDate = today.AddDays(2);

            if (tour.Date.CompareTo(futureDate) > 0)
            {
                return true;
            }
            else if (tour.Date.CompareTo(futureDate) == 0 && tour.StartTime > currentTime)
            {
                return true;
            }

            return false;
        }

        public void DelayGuideUpcomingTours(User guide)
        {
            List<Tour> tours = GetUpcomingToursByUser(guide);
            foreach (Tour tour in tours)
            {
                GivingDelayVouchers(tour);
                _tourReservationService.DeleteTour(tour);
                _tourRepository.Delete(tour);
            }
        }

        private void GivingDelayVouchers(Tour tour)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            foreach (TourReservation tr in _tourReservationService.GetAll())
            {
                if (tr.IdTour == tour.Id)
                {
                    Voucher voucher = new Voucher(tr.IdUser, "Demission voucher", today.AddYears(2));
                    _voucherService.Save(voucher);
                }
            }
        }


        public Tour GetTopTour(User user)
        {
            int max = 0;
            int idTour = 0;
            int j = 0;

            foreach (Tour t in GetAllByUser(user))
            {
                j = FindAttendanceNum(user, j, t);
                if (j > max)
                {
                    max = j;
                    idTour = t.Id;
                }
                j = 0;
            }

            return GetById(idTour);
        }

        public Tour GetTopYearTour(User user, int year)
        {
            int max = 0;
            int idTour = 0;
            int j = 0;

            foreach (Tour t in GetAllByUser(user))
            {
                if (t.Date.Year == year)
                {
                    j = FindAttendanceNum(user, j, t);
                    if (j > max)
                    {
                        max = j;
                        idTour = t.Id;
                    }
                    j = 0;
                }
            }

            return GetById(idTour);
        }

        private int FindAttendanceNum(User user, int j, Tour t)
        {
            foreach (TourAttendance ta in _tourAttendenceService.GetAllByGuide(user))
            {
                if (t.Id == ta.IdTour)
                {
                    j++;
                }
            }

            return j;
        }

        public List<int> GetAllYears()
        {
            List<int> years = new List<int>();
            foreach (Tour t in _tourRepository.GetAll())
            {
                if (!years.Contains(t.Date.Year))
                {
                    years.Add(t.Date.Year);
                }
            }
            return years;
        }


        public bool IsUserFree(User user, DateOnly date)
        {
            foreach (Tour t in GetAllByUser(user))
            {
                if(t.Date == date)
                {
                    return false;
                }
            }
            return true;
        }
        public List<Tour> GetPastYearTours()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            List<Tour> tours = new List<Tour>();

            foreach (Tour tour in GetAll())
            {
                if (tour.Date.AddYears(1) > today)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        public List<Tour> GetAllCreatedToursByRequest()
        {
            List<Tour> tourList = new List<Tour>();
            foreach (Tour t in _tourRepository.GetAll())
            {
                if (t.IdRequest != 0)
                {
                    tourList.Add(t);
                    
                }
            }
            tourList=BindData(tourList);
            return tourList;
        }

        public Tour GetTourByName(string name)
        {
            Tour tour = new Tour();
            foreach(Tour t in _tourRepository.GetAll())
            {
                if(t.Name == name)
                {
                    tour = t;
                }
            }

            return tour;
        }

       private List<string> GetGuideLanguages(User user)
       {
            List<string> languages = new List<string>();
            foreach (Tour t in GetAllByUser(user))
            {
                if (!languages.Contains(t.Language.ToLower()))
                {
                    languages.Add(t.Language.ToLower());
                }
            }
            return languages;
        }

        public List<Tour> GetLastYearTours(User user)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            List<Tour> tours = new List<Tour>();

            foreach (Tour tour in GetFinishedToursByUser(user))
            {
                if (tour.Date.AddYears(1) > today)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }

        public Dictionary<string, int> FillDictionary(User guide)
        {
            Dictionary<string, int> toursPerLanguage = new Dictionary<string, int>();

            foreach (string language in GetGuideLanguages(guide))
            {
                foreach (Tour tour in GetLastYearTours(guide))
                {
                    if (tour.Language.ToLower() == language.ToLower())
                    {
                        if (toursPerLanguage.ContainsKey(language))
                        {
                            toursPerLanguage[language]++;
                        }
                        else
                        {
                            toursPerLanguage.Add(language, 1);
                        }
                    }
                }
            }
            return toursPerLanguage;
        }

        public List<int> GetAllToursIdInRange(DateOnly start, DateOnly end)
        {
            List<int> ids = new List<int>();
            foreach(Tour t in GetAll())
            {
                if(t.Date > start && t.Date< end)
                {
                    ids.Add(t.Id);
                }
            }
            return ids;
        }
    }
}
