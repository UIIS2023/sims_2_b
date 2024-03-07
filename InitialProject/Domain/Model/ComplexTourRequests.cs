using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class ComplexTourRequests : ISerializable
    {
        public int Id { get; set; }
        public int RequestNumber { get; set; }
        public RequestType Status { get; set; }
        public User Guest { get; set; }
        public int GuestId { get; set; }

        public List<TourRequest> TourRequests { get; set; }
        public ComplexTourRequests()
        {
            TourRequests = new List<TourRequest>();
        }

        public ComplexTourRequests(int requestNumber, RequestType status, int guestId)
        {
            RequestNumber = requestNumber;
            Status = status;
            TourRequests = new List<TourRequest>();
            GuestId = guestId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            RequestNumber = int.Parse(values[1]);
            Status = (RequestType)Enum.Parse(typeof(RequestType), values[2]);
            GuestId = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                RequestNumber.ToString(),
                Status.ToString(),
                GuestId.ToString(),
            };
            return csvValues;
        }
        
    }
}
