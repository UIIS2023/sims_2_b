using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class MonthlyStatisticsUCViewModel : ViewModelBase
	{
		public static Accommodation SelectedAccommodation { get; set; }

		public static User LoggedInUser { get; set; }

		public static int SelectedYear { get; set; }

		public int Month { get; set; }

		private readonly AccommodationReservationService accommodationReservationService;

		public static ObservableCollection<StatisticsViewModel> Statistics { get; set; }

		public MonthlyStatisticsUCViewModel(User owner, Accommodation accommodation, int year)
		{
			SelectedAccommodation=accommodation;
			SelectedYear=year;
			LoggedInUser=owner;
			accommodationReservationService = new AccommodationReservationService();
			Statistics = new ObservableCollection<StatisticsViewModel>(accommodationReservationService.GetMonthlyStatistics(SelectedYear,SelectedAccommodation.Id).Select(StatisticsViewModel.MapToViewModel));
			Month = accommodationReservationService.GetBusiestmonth(accommodation.Id, year);
		}
		
	}
}
