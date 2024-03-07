using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class Location : ValidationBase, ISerializable
    {
        public int Id { get; set; }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }


        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this._city))
            {
                this.ValidationErrors["City"] = "City must be selected.";
            }

            if (string.IsNullOrWhiteSpace(this._country))
            {
                this.ValidationErrors["Country"] = "Country must be selected.";
            }

        }


        public Location() { }


        public Location(string city, string country)
        {

            City = city;
            Country = country;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                City,
                Country
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }

    }
}
