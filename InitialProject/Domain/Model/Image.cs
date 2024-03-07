using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class Image : ISerializable

    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int IdAccommodation { get; set; }

        public int IdTour { get; set; }

        public int IdOwner { get; set; }
        public string Path { get; set; }
        public bool Cover { get; set; }
        public ImageType Type { get; set; }

        public Image(string url, int idAccommodation, int idTour, int idOwner)
        {
            Url = url;
            IdAccommodation = idAccommodation;
            IdTour = idTour;
            IdOwner = idOwner;
        }

        public Image()
        {

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Url = values[1];
            IdAccommodation = int.Parse(values[2]);
            IdTour = int.Parse(values[3]);
            IdOwner= int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Url,
                IdAccommodation.ToString(),
                IdTour.ToString(),
                IdOwner.ToString(),
            };
            return csvValues;

        }
    }
}
