using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Applications.UseCases
{
    public class TourGuideReviewsService
    {
        private readonly ITourGuideReviewRepository _tourGuideRepository;
        private readonly UserService _userService;
        private readonly TourPointService _tourPointService;
        private readonly TourService _tourService;
        public TourGuideReviewsService()
        {
            _tourGuideRepository= Inject.CreateInstance<ITourGuideReviewRepository>();
            _userService = new UserService();
            _tourPointService= new TourPointService();
            _tourService= new TourService();
        }

        public List<TourGuideReview> GetAllByUser(User user)
        {
            List<TourGuideReview> _reviews = _tourGuideRepository.GetAllByUser(user);
            foreach(TourGuideReview review in _reviews)
            {
                review.Guest = _userService.GetById(review.IdGuest);
                review.TourPoint = _tourPointService.GetById(review.IdTourPoint);
            }
            return _reviews;
        }

        public TourGuideReview Update(TourGuideReview review)
        {
            return _tourGuideRepository.Update(review);
        }

        public double GetAvarageGrade(User user)
        {
            double sum = 0;
            double n = 0;

            foreach (TourGuideReview review in _tourGuideRepository.GetAllByUser(user))
            {
                sum += review.GuideLanguage;
                sum += review.GuideKnowledge;
                n += 2;
            }
            if(n == 0)
            {
                return 0;
            }
            return sum / n;
        }

        
        /*
        public List<TourGuideReview> GetLastYearReviews(User user)
        {
            List<TourGuideReview> lastYearReviews = new List<TourGuideReview>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach(Tour tour in )
            foreach (TourGuideReview review in _tourGuideRepository.GetAllByUser(user))
            {
                if(review.Date.AddYears(1) > today)
                {
                    lastYearReviews.Add(review);
                }
            }

        }*/

        /* public string GetTopLanguage()
        {
            string requestLanguage;
            Dictionary<string, int> languages_num = new Dictionary<string, int>();
            foreach (string language in GetAllLanguages())
            {
                foreach (TourRequest request in GetThisYearRequests())
                {
                    requestLanguage = request.TourLanguage.ToLower();
                        
                    if (requestLanguage.Equals(language.ToLower()))
                    {
                        if (languages_num.ContainsKey(requestLanguage))
                        {
                            languages_num[requestLanguage]++;
                        }
                        else
                        {
                            languages_num.Add(requestLanguage, 1);
                        }
                    }
                }
            }
            int maxNum = languages_num.Values.Max();
            string topLanguage = languages_num.FirstOrDefault(x => x.Value == maxNum).Key;
            return topLanguage;
        }*/


        public double CheckThisYearAvarageGrade(string language, User user)
        {
            double sum = 0;
            double n = 0;

            foreach (TourGuideReview review in GetAllLanguageAndThisYear(language, user ))
            {
                sum += review.GuideLanguage;
                sum += review.GuideKnowledge;
                n += 2;
            }
            if (n == 0)
            {
                return 0;
            }
            return sum / n;
        }

        public List<TourGuideReview> GetAllLanguageAndThisYear(string language, User user)
        {
            List<TourGuideReview> reviews = new List<TourGuideReview>();
            foreach(Tour tour in _tourService.GetLastYearTours(user))
            {
                if(tour.Language.ToLower() == language.ToLower())
                {
                    foreach (TourGuideReview review in GetAllByUser(user))
                    {
                        if (review.IdTour == tour.Id)
                        {
                            reviews.Add(review);
                        }
                    }
                }
            }
            return reviews;
        }

        public string CheckLanguage(User guide)
        {
            Dictionary<string, int> toursPerLanguage = _tourService.FillDictionary(guide);
            
            foreach(var langNum in toursPerLanguage)
            {
                if(langNum.Value >= 20)
                {
                    if (CheckThisYearAvarageGrade(langNum.Key, guide) > 4)
                    {
                        return langNum.Key;
                    } 
                }
            }

            return null;
        }

        public bool IsGuideSuper(User guide)
        {
            if (CheckLanguage(guide) != null)
            {
                guide.IsSuper = true;
                _userService.Update(guide);
                return true;
            }
            guide.IsSuper = false;
            _userService.Update(guide);
            return false;
        }
    }
}
