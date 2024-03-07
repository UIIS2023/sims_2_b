using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace InitialProject.Domain.Model
{
    public class TourAttendance : ISerializable
    {
        public int Id { get; set; }
        public int IdTour { get; set; }
        public int IdGuide { get; set; }
        public Tour Tour { get; set; }
        public int IdGuest { get; set; }
        public User Guest { get; set; }
        public int IdTourPoint { get; set; }
        public TourPoint TourPoint { get; set; }
        public string TourName { get; set; }
        public bool UsedVoucher { get; set; }
        public bool Rated { get; set; }
        public string TourPointName { get; set; }
        public string ImageSource { get; set; }
        public TourAttendance() { }

        public TourAttendance(int idTour, int idGuide, int idGuest, int idTourPoint, bool usedVoucher, string tourPointName)
        {
            IdTour = idTour;
            IdGuide = idGuide;
            IdGuest = idGuest;
            IdTourPoint = idTourPoint;
            UsedVoucher = usedVoucher;
            Rated=false;   
            TourPointName = tourPointName;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdTour = int.Parse(values[1]);
            IdGuide = int.Parse(values[2]);
            IdGuest = int.Parse(values[3]);
            IdTourPoint = int.Parse(values[4]);
            UsedVoucher = bool.Parse(values[5]);
            Rated = bool.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdTour.ToString(),
                IdGuide.ToString(),
                IdGuest.ToString(),
                IdTourPoint.ToString(),
                UsedVoucher.ToString(),
                Rated.ToString(),
            };
            return csvValues;
        }
    }
}
