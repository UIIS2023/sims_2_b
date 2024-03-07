using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Serializer;
using InitialProject.Validations;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Model
{
    public class TourRequest : ValidationBase, ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public int IdGuest { get; set; }
        public int IdGuide { get; set; }
        public int IdLocation { get; set; }
        public RequestType Status { get; set; }
        public int IdComplexTour { get; set; }
        public int GuestNum { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string _language;
        public string TourLanguage
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged(nameof(TourLanguage));
                }
            }
        }

        

        private DateOnly inputStartdate { get; set; }
        public DateOnly NewStartDate
        {
            get => inputStartdate;
            set
            {
                if (value != inputStartdate)
                {
                    inputStartdate = value;
                    OnPropertyChanged(nameof(NewStartDate));
                }
            }
        }

        private DateOnly inputEnddate { get; set; }
        public DateOnly NewEndDate
        {
            get => inputEnddate;
            set
            {
                if (value != inputEnddate)
                {
                    inputEnddate = value;
                    OnPropertyChanged(nameof(NewEndDate));
                }
            }
        }



        public TourRequest()
        {
        }

        public TourRequest(Location location, int idGuest, string language, int guestNum, DateOnly startDate, DateOnly endDate, int idLocation, string description, int idComplexTour, int idGuide)
        {
            Location = location;
            IdGuest = idGuest;
            TourLanguage = language;
            GuestNum = guestNum;
            NewStartDate =startDate;
            NewEndDate = endDate;
            IdLocation = idLocation;
            Description = description;
            Status = RequestType.OnHold;
            IdComplexTour=idComplexTour;
            IdGuide =idGuide;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdGuest.ToString(),
                TourLanguage,
                GuestNum.ToString(),
                NewStartDate.ToString(),
                NewEndDate.ToString(),
                IdLocation.ToString(),
                Description,
                Status.ToString(),
                IdComplexTour.ToString(),
                IdGuide.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdGuest= int.Parse(values[1]);
            TourLanguage = values[2];
            GuestNum = int.Parse(values[3]);
            NewStartDate = DateOnly.Parse(values[4]);
            NewEndDate = DateOnly.Parse(values[5]);
            IdLocation = int.Parse(values[6]);
            Description = values[7];
            Status = (RequestType)Enum.Parse(typeof(RequestType), values[8]);
            IdComplexTour = int.Parse(values[9]);
            IdGuide = int.Parse(values[10]);
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this._language))
            {
                this.ValidationErrors["TourLanguage"] = "Language cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(this._description))
            {
                this.ValidationErrors["Description"] = "Description cannot be empty.";
            }


            if (NewStartDate == default(DateOnly))
            {
                this.ValidationErrors["NewStartDate"] = "Start date is required.";
            }

            if (NewEndDate == default(DateOnly))
            {
                this.ValidationErrors["NewEndDate"] = "End date is required.";

            }

            if (NewStartDate >= NewEndDate)
            {
                this.ValidationErrors["NewStartDate"] = "Start must be before end.";
                this.ValidationErrors["NewEndDate"] = "End must be after start.";
            }
        }
    }
}
