using InitialProject.Applications.DTO;
using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace InitialProject.WPF.ViewModel
{
	public class StatisticsForAccommodationViewModel : ViewModelBase
	{
		public static Accommodation SelectedAccommodation { get; set; }

		public static User LoggedInUser { get; set; }


		public static ObservableCollection<int> AvailableYears1 { get; set; }

		public static ObservableCollection<string> AvailableYears { get; set; }

		public static ObservableCollection<string> YearsList { get; set; }


		private readonly AccommodationReservationService accommodationReservationService;

		public UserControl CurrentUserControl { get; set; }


		private string _selectedYear;
		public string SelectedYear
		{
			get => _selectedYear;
			set
			{
				_selectedYear = value;
				OnPropertyChanged(nameof(SelectedYear));
				if (SelectedYear.Equals("None"))
				{
					var yearlyStatisticsViewModel = new YearlyStatisticsUCViewModel(LoggedInUser, SelectedAccommodation);
					CurrentUserControl.Content = new YearlyStatisticsUC(LoggedInUser, yearlyStatisticsViewModel);
				}
				else
				{
					var monthlyStatisticsViewModel = new MonthlyStatisticsUCViewModel(LoggedInUser, SelectedAccommodation, int.Parse(SelectedYear));
					CurrentUserControl.Content = new MonthlyStatisticsUC(LoggedInUser, monthlyStatisticsViewModel);
				}
				
			}
		}



		public StatisticsForAccommodationViewModel(Accommodation selectedAccommodation,User owner )
		{
			
			accommodationReservationService = new AccommodationReservationService();
			var yearlyStatisticsViewModel = new YearlyStatisticsUCViewModel(owner,selectedAccommodation);
			CurrentUserControl = new YearlyStatisticsUC(owner, yearlyStatisticsViewModel);
			InitializeProperties(selectedAccommodation, owner);
		}



		private void InitializeProperties(Accommodation selectedAccommodation, User owner)
		{ 
			AvailableYears1 = new ObservableCollection<int>(accommodationReservationService.GetYearsForAccommodation(selectedAccommodation.Id));
			AvailableYears1 = new ObservableCollection<int>(AvailableYears1.Prepend(0).ToList());
			AvailableYears = new ObservableCollection<string>(AvailableYears1.Select(year => year.ToString()).ToList());
			YearsList = new ObservableCollection<string>(AvailableYears.Select(y => y == "0" ? "None" : y.ToString()).ToList());
			SelectedAccommodation = selectedAccommodation;
			LoggedInUser = owner;
		}
	}
}
