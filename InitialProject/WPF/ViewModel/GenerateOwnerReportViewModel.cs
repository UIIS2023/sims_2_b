using ceTe.DynamicPDF;
using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
	public class GenerateOwnerReportViewModel : ViewModelBase
	{
		public static User LoggedInUser { get; set; }

		public static ObservableCollection<Accommodation> Accommodations { get; set; }

		public static Accommodation SelectedAccommodation { get; set; }

		private readonly AccommodationService accommodationService;

        private readonly AccommodationReservationService accommodationReservationService;

        public ceTe.DynamicPDF.Document Report;

        public System.Action CloseAction { get; set; }



        private DateTime? _startDate;
        public DateTime? startDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(startDate));
                }
            }
        }

        private DateTime? _endDate;
        public DateTime? endDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(endDate));
                }
            }
        }

        private RelayCommand generateCommand;
        public RelayCommand GenerateCommand
        {
            get { return generateCommand; }
            set
            {
                generateCommand = value;
            }
        }


        public GenerateOwnerReportViewModel(User user)
		{
			LoggedInUser = user;
			accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
			Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetByUser(user));
            GenerateCommand = new RelayCommand(Execute_Generate, CanExecute);
            Default();

            


        }


        private void GenerateReport()
		{
            DateOnly StartDate = DateOnly.FromDateTime(startDate ?? DateTime.MinValue);
            DateOnly EndDate = DateOnly.FromDateTime(endDate ?? DateTime.MinValue);

            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 44.0f);
            Report.Pages.Add(page);
            Color color = new RgbColor(0, 0, 200);
            ceTe.DynamicPDF.PageElements.Label header = new ceTe.DynamicPDF.PageElements.Label("Izvestaj o rezervacijama", 0, 0, 504, 100, Font.TimesRoman, 27, TextAlign.Center, color);
        
            ceTe.DynamicPDF.PageElements.Label user = new ceTe.DynamicPDF.PageElements.Label("Korisnik: " + LoggedInUser.Username, 0, 50, 200, 20, Font.TimesRoman, 15, TextAlign.Left);

            ceTe.DynamicPDF.PageElements.Label accommodationName = new ceTe.DynamicPDF.PageElements.Label("Smestaj: " + SelectedAccommodation.Name, 0, 70, 200, 20, Font.TimesRoman, 15, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label datefrom = new ceTe.DynamicPDF.PageElements.Label("Datum od: " + StartDate.ToShortDateString(), 0, 90, 200, 20, Font.TimesRoman, 15, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label dateto = new ceTe.DynamicPDF.PageElements.Label("Datum do: " + EndDate.ToShortDateString(), 0, 110, 200, 20, Font.TimesRoman, 15, TextAlign.Left);

            ceTe.DynamicPDF.PageElements.Label addressLabel = new ceTe.DynamicPDF.PageElements.Label("Adresa: Hajduk Veljkova 5", 290, 50, 200, 20, Font.TimesRoman, 15, TextAlign.Right);
            ceTe.DynamicPDF.PageElements.Label cityLabel = new ceTe.DynamicPDF.PageElements.Label("21000 Novi Sad, Srbija", 290, 70, 200, 20, Font.TimesRoman, 15, TextAlign.Right);
            ceTe.DynamicPDF.PageElements.Label phoneLabel = new ceTe.DynamicPDF.PageElements.Label("Kontakt telefon: 021568136", 290, 90, 200, 20, Font.TimesRoman, 15, TextAlign.Right);
            ceTe.DynamicPDF.PageElements.Label emailLabel = new ceTe.DynamicPDF.PageElements.Label("E-mail: booking@gmail.com", 290, 110, 200, 20, Font.TimesRoman, 15, TextAlign.Right);


            
            Report.Pages[0].Elements.Add(addressLabel);
            Report.Pages[0].Elements.Add(cityLabel);
            Report.Pages[0].Elements.Add(phoneLabel);
            Report.Pages[0].Elements.Add(emailLabel);

            page.Elements.Add(datefrom);
            page.Elements.Add(accommodationName);
            page.Elements.Add(dateto);
            page.Elements.Add(header);
            page.Elements.Add(user);

            if(accommodationReservationService.GetReservationsForReport(LoggedInUser, SelectedAccommodation, StartDate, EndDate).Count != 0)
			{
                ceTe.DynamicPDF.PageElements.Label reservationName = new ceTe.DynamicPDF.PageElements.Label("Ime gosta", 0, 200, 200, 40, Font.TimesRoman, 21, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label startDate = new ceTe.DynamicPDF.PageElements.Label("Pocetni datum", 120, 200, 504, 100, Font.TimesRoman, 21, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label endDate = new ceTe.DynamicPDF.PageElements.Label("Krajnji datum", 270, 200, 504, 100, Font.TimesRoman, 21, TextAlign.Left);
               

                page.Elements.Add(reservationName);
                page.Elements.Add(startDate);
                page.Elements.Add(endDate);
               


                float labelWidth = 150f; 
                float labelHeight = 30f; 
                float horizontalSpacing = 20f; 
                float verticalSpacing = 2f; 
                float initialX = 0; 
                float initialY = 240; 

                float currentX = initialX;
                float currentY = initialY;

                foreach(var r in accommodationReservationService.GetReservationsForReport(LoggedInUser, SelectedAccommodation, StartDate, EndDate))
				{
                    currentX = initialX;


                    ceTe.DynamicPDF.PageElements.Label id = new ceTe.DynamicPDF.PageElements.Label(r.Guest.Username.ToString(), currentX, currentY, 130, labelHeight, Font.TimesRoman,21, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label StartingDate = new ceTe.DynamicPDF.PageElements.Label(r.StartDate.ToString(), currentX + 120f, currentY, 140, labelHeight, Font.TimesRoman, 21, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label EndingDate = new ceTe.DynamicPDF.PageElements.Label(r.EndDate.ToString(), currentX + 270f, currentY, 160, labelHeight, Font.TimesRoman, 21, TextAlign.Left);

                    

                    
                    page.Elements.Add(id);
                    page.Elements.Add(StartingDate);
                    page.Elements.Add(EndingDate);
                    

                    
                    currentY += labelHeight + verticalSpacing;
                }
            }

            else
			{
                ceTe.DynamicPDF.PageElements.Label name = new ceTe.DynamicPDF.PageElements.Label("Izabrani smeštaj nema rezervacija u datom periodu", 0, 130, 330, 20, Font.TimesRoman, 21, TextAlign.Left);
                page.Elements.Add(name);
            }
        }

        private void Execute_Generate(object sender)
		{
            Report = new ceTe.DynamicPDF.Document();
            GenerateReport();
            string filePath = @"C:\Users\Hp\Desktop\SIMS-projekat\SIMS-projekat\InitialProject\InitialProject\OwnerReport\ownerReport.pdf";

            Report.Draw(filePath);
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(processStartInfo);

            CloseAction();

        }
        private bool CanExecute(object parameter)
        {
            return true;
        }
        private void Default()
        {
            SelectedAccommodation = Accommodations.Any() ? Accommodations[0] : null;
            startDate = null;
            endDate = null;
            

        }
    }
}
