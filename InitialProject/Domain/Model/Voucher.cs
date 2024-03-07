using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public Voucher() { }

        public Voucher(int idUser, string name, DateOnly expirationDate)
        {
            IdUser = idUser;
            Name = name;
            ExpirationDate = expirationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdUser.ToString(),
                Name,
                ExpirationDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdUser= int.Parse(values[1]);
            Name = values[2];
            ExpirationDate = DateOnly.Parse(values[3]);
        }
    }
}
