using InitialProject.Serializer;
using InitialProject.Validations;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.Domain.Model
{
    public class Tour : ValidationBase, ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public int IdLocation { get; set; }
        public List<Image> Images { get; set; }
        public TimeOnly StartTime { get; set; }
        public int FreeSetsNum { get; set; }
        public bool Active { get; set; }
        public bool Paused { get; set; }
        public int IdUser { get; set; }
        public bool UsedVoucher { get; set; }
        public int IdRequest { get; set; }
        public int MaxGuestNum { get; set; }
        
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _description;
        public string Descripiton
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        


        private string _durationS;
        public string DurationS
        {
            get => _durationS;
            set
            {
                if (value != _durationS)
                {
                    _durationS = value;
                    OnPropertyChanged(nameof(DurationS));
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }


        private string _points;
        public string Points
        {
            get => _points;
            set
            {
                if (value != _points)
                {
                    _points = value;
                    OnPropertyChanged("Points");
                }
            }
        }

        private DateOnly _startDate;
        public DateOnly Date
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }



        private string _imagesUrl;
        public string ImageUrls
        {
            get => _imagesUrl;
            set
            {
                if (value != _imagesUrl)
                {
                    _imagesUrl = value;
                    OnPropertyChanged("ImageUrls");
                }
            }
        }

        public string ImageSource { get; set; }

        
        public Tour()
        {
            Images = new List<Image>();
        }


        public Tour(string name, Location location, string language, int maxGuestNum, DateOnly date, TimeOnly startTime, int duration, int freeSetsNum, bool active, int idUser, int idLocation, bool usedVoucher)

        {
            Name = name;
            Location = location;
            Language = language;
            MaxGuestNum = maxGuestNum;
            Date = date;
            StartTime = startTime;
            Duration = duration;
            FreeSetsNum = freeSetsNum;
            Active = active;
            Paused = false;
            IdUser = idUser;
            IdLocation = idLocation;
            Images = new List<Image>();
            UsedVoucher=usedVoucher;
            IdRequest = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Language,
                MaxGuestNum.ToString(),
                Date.ToString(),
                StartTime.ToString(),
                Duration.ToString(),
                FreeSetsNum.ToString(),
                Active.ToString(),
                Paused.ToString(),
                IdUser.ToString(),
                IdLocation.ToString(),
                UsedVoucher.ToString(),
                IdRequest.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Language = values[2];
            MaxGuestNum = int.Parse(values[3]);
            Date = DateOnly.Parse(values[4]);
            StartTime = TimeOnly.Parse(values[5]);
            Duration = int.Parse(values[6]);
            FreeSetsNum = int.Parse(values[7]);
            Active = bool.Parse(values[8]);
            Paused = bool.Parse(values[9]);
            IdUser = int.Parse(values[10]);
            IdLocation = int.Parse(values[11]);
            UsedVoucher = bool.Parse(values[12]);
            IdRequest = int.Parse(values[13]);
        }

        protected override void ValidateSelf()
        { 

        }
    }
}