using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    class TourPointService
    {
        private readonly ITourPointRepository _tourPointRepository;
        public TourPointService() {
            _tourPointRepository = Inject.CreateInstance<ITourPointRepository>();
        }

        public void ActivateFirstPoint(Tour tour)
        {
            foreach (TourPoint tourPoint in _tourPointRepository.GetAll())
            {
                if (tourPoint.IdTour == tour.Id && tourPoint.Order == 1)
                {
                    tourPoint.Active = true;
                    _tourPointRepository.Update(tourPoint);
                    return;
                }
            }
        }

        public List<TourPoint> GetAll()
        {
            return _tourPointRepository.GetAll();
        }


        public TourPoint Save(TourPoint tourPoint)
        {
            TourPoint savedTourPoint = _tourPointRepository.Save(tourPoint);
            return savedTourPoint;
        }

        public TourPoint Update(TourPoint tourPoint)
        {
            return _tourPointRepository.Update(tourPoint);
        }

        public TourPoint GetByOrder(int order)
        {
            List<TourPoint> points = _tourPointRepository.GetAll();
            return points.Find(c => c.Order == order);

        }
        public string GetTourPointNameByTourPointId(int idTourPoint)
        {
            foreach(TourPoint tP in _tourPointRepository.GetAll())
            {
                if(idTourPoint==tP.Id)
                {
                    return tP.Name;
                }
            }
            return null;
        }


        public List<TourPoint> GetAllByTourId(int idTour)
        {
            return _tourPointRepository.GetAllByTourId(idTour); ;
        }

        public TourPoint GetById(int id)
        {
            return _tourPointRepository.GetById(id);
        }

        public int GetMaxOrder(int idTour)
        {
            int max = 2;
            foreach (TourPoint tourPoint in _tourPointRepository.GetAll())
            {
                if (tourPoint.IdTour == idTour && tourPoint.Order > max)
                {
                    max = tourPoint.Order;
                }
            }
            return max;
        }

        public string GetTourNameByTourId(int id)
        {
            foreach (TourPoint tP in _tourPointRepository.GetAll())
            {
                if (tP.IdTour==id)
                {
                    return tP.TourName;
                }
            }
            return null;
        }

    }
}
