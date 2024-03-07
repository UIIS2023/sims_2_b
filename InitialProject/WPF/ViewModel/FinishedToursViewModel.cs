using ceTe.DynamicPDF;
using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Page = ceTe.DynamicPDF.Page;

namespace InitialProject.WPF.ViewModel
{
    public class FinishedToursViewModel : ViewModelBase
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourService _tourService;
        private readonly TourAttendanceService _tourAttendanceService;
        private readonly MessageBoxService _messageBoxService;


        private RelayCommand statistics;
        public RelayCommand StatisticsCommand
        {
            get { return statistics; }
            set
            {
                statistics = value;
            }
        }

        private RelayCommand report;
        public RelayCommand ReportCommand
        {
            get { return report; }
            set
            {
                report = value;
            }
        }


        public delegate void EventHandler1(Tour tour);

        public event EventHandler1 StatisticsEvent;

        public ceTe.DynamicPDF.Document Report;

        public FinishedToursViewModel(User user)
        {
            LoggedInUser = user;
            _tourService = new TourService();
            _tourAttendanceService= new TourAttendanceService();
            _messageBoxService = new MessageBoxService();
            Tours = new ObservableCollection<Tour>(_tourService.GetFinishedToursByUser(user));

            StatisticsCommand = new RelayCommand(Execute_Statistics, CanExecute_Command);
            ReportCommand = new RelayCommand(Execute_Report, CanExecute_Command);
        }

        private void GenerateReport()
        {
            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 44.0f);
            Report.Pages.Add(page);


            ceTe.DynamicPDF.PageElements.Label header = new ceTe.DynamicPDF.PageElements.Label("Report about tour guests", 0, 0, 504, 100, Font.TimesRoman, 18, TextAlign.Center);
            ceTe.DynamicPDF.PageElements.Label tourName = new ceTe.DynamicPDF.PageElements.Label("Tour name: " + SelectedTour.Name, 0, 50, 200, 20, Font.TimesRoman, 14, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label tourLocation = new ceTe.DynamicPDF.PageElements.Label("Location: " + SelectedTour.Location.City + ", " + SelectedTour.Location.Country, 0, 70, 200, 20, Font.TimesRoman, 14, TextAlign.Left);
            ceTe.DynamicPDF.PageElements.Label tourDate = new ceTe.DynamicPDF.PageElements.Label("Date: " + SelectedTour.Date, 0, 90, 200, 20, Font.TimesRoman, 14, TextAlign.Left);

            page.Elements.Add(tourDate);
            page.Elements.Add(tourLocation);
            page.Elements.Add(header);
            page.Elements.Add(tourName);

            if (_tourAttendanceService.GetAllByTourId(SelectedTour.Id).Count != 0)
            {
                ceTe.DynamicPDF.PageElements.Label guestName = new ceTe.DynamicPDF.PageElements.Label("Guest name", 0, 150, 200, 40, Font.TimesRoman, 14, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label tourPointName = new ceTe.DynamicPDF.PageElements.Label("Attended tourpoint", 120, 150, 504, 100, Font.TimesRoman, 14, TextAlign.Left);
                ceTe.DynamicPDF.PageElements.Label usedVpucher = new ceTe.DynamicPDF.PageElements.Label("Used voucher", 270, 150, 504, 100, Font.TimesRoman, 14, TextAlign.Left);

                page.Elements.Add(guestName);
                page.Elements.Add(tourPointName);
                page.Elements.Add(usedVpucher);

                float labelWidth = 150f; // Adjust the width of each label as needed
                float labelHeight = 30f; // Adjust the height of each label as needed
                float horizontalSpacing = 20f; // Adjust the horizontal spacing between labels as needed
                float verticalSpacing = 2f; // Adjust the vertical spacing between rows as needed
                float initialX = 0; // Initial starting X-coordinate
                float initialY = 180; // Initial starting Y-coordinate

                float currentX = initialX;
                float currentY = initialY;

                foreach (var element in _tourAttendanceService.GetAllByTourId(SelectedTour.Id))
                {
                    currentX = initialX;


                    ceTe.DynamicPDF.PageElements.Label nameG = new ceTe.DynamicPDF.PageElements.Label(element.Guest.Username, currentX, currentY, 130, labelHeight, Font.TimesRoman, 11, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label nameTP = new ceTe.DynamicPDF.PageElements.Label(element.TourPointName, currentX + 120f, currentY, 140, labelHeight, Font.TimesRoman, 11, TextAlign.Left);
                    ceTe.DynamicPDF.PageElements.Label usedV = new ceTe.DynamicPDF.PageElements.Label(element.UsedVoucher ? "used" : "not used", currentX + 270f, currentY, 160, labelHeight, Font.TimesRoman, 11, TextAlign.Left);
                   
                    page.Elements.Add(nameG);
                    page.Elements.Add(nameTP);
                    page.Elements.Add(usedV);

                    // Increment the X-coordinate for the next row
                    currentY += labelHeight + verticalSpacing;

                }
            }
            else
            {
                ceTe.DynamicPDF.PageElements.Label message = new ceTe.DynamicPDF.PageElements.Label("This tour don't have attendences", 0, 130, 330, 20, Font.TimesRoman, 11, TextAlign.Left);
                page.Elements.Add(message);
            }



        }

        private void Execute_Report(object obj)
        {

            Report = new ceTe.DynamicPDF.Document();
            GenerateReport();
            string filePath = "C:\\Users\\Dell\\Desktop\\3 GODINA\\SIMS-projekat\\SIMS-projekat\\InitialProject\\GuideReport\\guideReport.pdf";
            Report.Draw(filePath);
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(processStartInfo);
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_Statistics(object obj)
        {
            if(SelectedTour != null)
            {
                StatisticsEvent?.Invoke(SelectedTour);
            }
            else
            {
                _messageBoxService.ShowMessage("Please, first select a tour");
            }
        }
    }
}
