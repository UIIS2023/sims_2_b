using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class SuperGuest:BindableBase, ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }

        private string bonus;
        public string Bonus
        {
            get { return bonus; }
            set
            {
                bonus = value;
                OnPropertyChanged(nameof(Bonus));
            }
        }



        public  DateTime SuperGuestExpirationDate { get; set; }

        public SuperGuest(int guestId, string bonus,  DateTime superGuestExpirationDate)
        {
            
            GuestId = guestId;
            Bonus = bonus;
            SuperGuestExpirationDate = superGuestExpirationDate;
        }

        public SuperGuest()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                Bonus,
                SuperGuestExpirationDate.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId= int.Parse(values[1]);
            Bonus=values[2];
            SuperGuestExpirationDate = DateTime.Parse(values[3]);

        }
    }
}
