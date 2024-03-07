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
	public class YearlyStatisticsUCViewModel : ViewModelBase
	{
		public static Accommodation SelectedAccommodation { get; set; }

		public static User LoggedInUser { get; set; }

		public static int Year { get; set; }


		private readonly AccommodationReservationService accommodationReservationService;

		public static ObservableCollection<StatisticsViewModel> Statistics { get; set; }


		public YearlyStatisticsUCViewModel(User owner, Accommodation accommodation)
		{
			LoggedInUser = owner;
			SelectedAccommodation=accommodation;
			accommodationReservationService = new AccommodationReservationService();
			Statistics =  new ObservableCollection<StatisticsViewModel>(accommodationReservationService.GetYearlyStatistics(SelectedAccommodation.Id).Select(StatisticsViewModel.MapToViewModel));
			Year = accommodationReservationService.GetBusiestYear(accommodation.Id);
		}
		

	}
}
