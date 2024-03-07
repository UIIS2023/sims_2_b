using ceTe.DynamicPDF;
using GalaSoft.MvvmLight.Command;
using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModel
{
    public class ReportGuest2ViewModel : ViewModelBase
    {
        private readonly TourAttendanceService _tourAttendanceService;
        private readonly TourService _tourService;
        public User LoggedInUser { get; set; }
        public ceTe.DynamicPDF.Document Report;
        public RelayCommand CreateReportCommand { get; set; }


        private string _startDay;
        public string StartingDate
        {
            get
            {
                return _startDay;
            }
            set
            {
                if (value != _startDay)
                {
                    _startDay = value;
                    OnPropertyChanged("StartDay");
                }
            }
        }
        private string _endDay;
        public string EndingDate
        {
            get
            {
                return _endDay;
            }
            set
            {
                if (value != _endDay)
                {
                    _endDay = value;

                    OnPropertyChanged("StartDay");
                }
            }
        }

        public ReportGuest2ViewModel(User user)
        {
            LoggedInUser = user;
            _tourAttendanceService = new TourAttendanceService();
            _tourService = new TourService();
            CreateReportCommand =  new RelayCommand(Execute_CreateReportCommand, CanExecute_Command);

        }
        private bool CanExecute_Command()
        {
            return true;
        }

        private void Execute_CreateReportCommand()
        {
            Report = new ceTe.DynamicPDF.Document();
            GenerateReport();
            string filePath = "C:\\Users\\Katarina\\Desktop\\sims-projekat\\SIMS-projekat\\InitialProject\\Guest2Report\\guest2Report.pdf";
            Report.Draw(filePath);

            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(processStartInfo);

        }

        private void GenerateReport()
        {
            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 44.0f);
            Report.Pages.Add(page);

            string fullName = LoggedInUser.Username;

            ceTe.DynamicPDF.PageElements.Label header = new ceTe.DynamicPDF.PageElements.Label("Report about all attendances on tours in a certain period of time", 0, 0, 504, 100, Font.TimesRoman, 18, TextAlign.Center);
            ceTe.DynamicPDF.PageElements.Label user = new ceTe.DynamicPDF.PageElements.Label("User name: " + fullName, 0, 50, 200, 20, Font.TimesRoman, 14, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label datefrom = new ceTe.DynamicPDF.PageElements.Label("Start date: " + StartingDate, 0, 90, 200, 20, Font.TimesRoman, 14, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label dateto = new ceTe.DynamicPDF.PageElements.Label("End date: " + EndingDate, 0, 110, 200, 20, Font.TimesRoman, 14, TextAlign.Left);

            page.Elements.Add(datefrom);
            page.Elements.Add(dateto);
            page.Elements.Add(header);
            page.Elements.Add(user);

            List<int> ids = new List<int>(_tourService.GetAllToursIdInRange(DateOnly.Parse(StartingDate), DateOnly.Parse(EndingDate)));

            if (_tourAttendanceService.AllForReport(ids).Count != 0)
            {
                ceTe.DynamicPDF.PageElements.Label tourName = new ceTe.DynamicPDF.PageElements.Label("Tour name", 0, 150, 200, 40, Font.TimesRoman, 14, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label tourPointName = new ceTe.DynamicPDF.PageElements.Label("Tour point name", 120, 150, 504, 100, Font.TimesRoman, 14, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label presence = new ceTe.DynamicPDF.PageElements.Label("Presence", 270, 150, 504, 100, Font.TimesRoman, 14, TextAlign.Left);

                page.Elements.Add(tourName);
                page.Elements.Add(tourPointName);
                page.Elements.Add(presence);

                float labelWidth = 150f; // Adjust the width of each label as needed
                float labelHeight = 30f; // Adjust the height of each label as needed
                float horizontalSpacing = 20f; // Adjust the horizontal spacing between labels as needed
                float verticalSpacing = 2f; // Adjust the vertical spacing between rows as needed
                float initialX = 0; // Initial starting X-coordinate
                float initialY = 180; // Initial starting Y-coordinate

                float currentX = initialX;
                float currentY = initialY;

                

                foreach (var element in _tourAttendanceService.AllForReport(ids))
                {
                    currentX = initialX;


                    ceTe.DynamicPDF.PageElements.Label TourName = new ceTe.DynamicPDF.PageElements.Label(element.TourName, currentX, currentY, 130, labelHeight, Font.TimesRoman, 11, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label TourPointName = new ceTe.DynamicPDF.PageElements.Label(element.TourPointName, currentX + 120f, currentY, 140, labelHeight, Font.TimesRoman, 11, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label Presence = new ceTe.DynamicPDF.PageElements.Label(element.TourPoint.Active ? "present" : "not present", currentX + 270f, currentY, 160, labelHeight, Font.TimesRoman, 11, TextAlign.Left);


                    page.Elements.Add(TourName);
                    page.Elements.Add(TourPointName);
                    page.Elements.Add(Presence);

                    currentY += labelHeight + verticalSpacing;

                }
            }
            else
            {
                ceTe.DynamicPDF.PageElements.Label name = new ceTe.DynamicPDF.PageElements.Label("U izabranom periodu gost nije imao prisustva na turama", 0, 130, 330, 20, Font.TimesRoman, 11, TextAlign.Left);
                page.Elements.Add(name);
            }


        
        }
    }
}
