using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace InitialProject.WPF.ViewModel
{
    public class TourStatisticsViewModel
    {
        public Tour SelectedTour { get; set; }
        public int Youngest { get; set; }
        public int MediumAge { get; set; }
        public int Oldest { get; set; }
        public String WithVoucher { get; set; }
        public String WithoutVoucher { get; set; }

        private readonly TourAttendanceService _tourAttendanceService;
        public TourStatisticsViewModel(Tour tour) 
        {
            SelectedTour = tour;
            _tourAttendanceService = new TourAttendanceService();
            InitializeProperties();
        }

        void InitializeProperties()
        {
            Youngest = _tourAttendanceService.FindYoungest(SelectedTour);
            MediumAge = _tourAttendanceService.FindMediumAge(SelectedTour);
            Oldest = _tourAttendanceService.FindOldestAge(SelectedTour);

            WithVoucher = _tourAttendanceService.FindWithVoucher(SelectedTour).ToString() + "%";
            WithoutVoucher = (100 - _tourAttendanceService.FindWithVoucher(SelectedTour)).ToString() + "%";
        }


    }
}
