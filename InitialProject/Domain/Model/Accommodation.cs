using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class Accommodation : ValidationBase, ISerializable
    {

        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        public int IdLocation { get; set; }
        public Location Location { get; set; }

        public AccommodationType Type { get; set; }
        public int MaxGuestNum { get; set; }

        public int MinReservationDays { get; set; }

        public int DaysBeforeCancel { get; set; }

        public List<Image> Images { get; set; }

        public int IdUser { get; set; }



        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this._name))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }

           
        }

        public Accommodation(string name, int idLocation, Location location, AccommodationType type, int maxGuestNum, int minResevationDays, int daysBeforeCancel, int idUser)

        {
            Name = name;
            IdLocation = idLocation;
            Location = location;

            Type = type;
            MaxGuestNum = maxGuestNum;
            MinReservationDays = minResevationDays;
            DaysBeforeCancel = daysBeforeCancel;
            IdUser = idUser;

        }

        public Accommodation()
        {
            Images = new List<Image>();
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            IdLocation = int.Parse(values[2]);
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[3]);
            MaxGuestNum = int.Parse(values[4]);
            MinReservationDays = int.Parse(values[5]);
            DaysBeforeCancel = int.Parse(values[6]);
            IdUser = int.Parse(values[7]);


        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                IdLocation.ToString(),
                Type.ToString(),
                MaxGuestNum.ToString(),
                MinReservationDays.ToString(),
                DaysBeforeCancel.ToString(),
                IdUser.ToString()



            };
            return csvValues;
        }

    }
}