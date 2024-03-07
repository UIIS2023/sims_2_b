using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class AccommodationReservation : ValidationBase,ISerializable
    {

        public int Id { get; set; }

        public int IdGuest { get; set; }

        public User Guest { get; set; }

        public int IdAccommodation { get; set; }
        public Accommodation Accommodation { get; set; }

        private DateOnly inputStartdate { get; set; }
        public DateOnly StartDate
        {
            get => inputStartdate;
            set
            {
                if (value != inputStartdate)
                {
                    inputStartdate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateOnly inputEnddate { get; set; }
        public DateOnly EndDate
        {
            get => inputEnddate;
            set
            {
                if (value != inputEnddate)
                {
                    inputEnddate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (StartDate == default(DateOnly))
            {
                this.ValidationErrors["StartDate"] = "Start Date is required.";
            }

            if (EndDate == default(DateOnly))
            {
                this.ValidationErrors["EndDate"] = "End date cannot be empty.";

            }


            if (StartDate >= EndDate)
            {
                this.ValidationErrors["StartDate"] = "Start date must be before end date.";
                this.ValidationErrors["EndDate"] = "End date must be after start date.";
            }
        }

        public int DaysNum { get; set; }


        public bool IsCanceled { get; set; }




        public AccommodationReservation()
        {

        }


        public AccommodationReservation(User guest, int idGuest, Accommodation accommodation, int idAccommodation, DateOnly startDate, DateOnly endDate, int daysNum, bool isCanceled)
        {
            Guest = guest;
            IdGuest = idGuest;
            Accommodation = accommodation;
            IdAccommodation = idAccommodation;
            StartDate = startDate;
            EndDate = endDate;
            DaysNum = daysNum;
            IsCanceled = isCanceled;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdGuest.ToString(),
                IdAccommodation.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                DaysNum.ToString(),
                IsCanceled.ToString()


            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdGuest = int.Parse(values[1]);
            IdAccommodation = int.Parse(values[2]);
            StartDate = DateOnly.Parse(values[3]);
            EndDate = DateOnly.Parse(values[4]);
            DaysNum = int.Parse(values[5]);
            IsCanceled = bool.Parse(values[6]);

        }
    }
}
