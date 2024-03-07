using InitialProject.Serializer;
using System;
using System.Data;

namespace InitialProject.Domain.Model
{

    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public Voucher TourVoucher { get; set; }
        
        public bool IsSuper { get; set; }

        public string ImageUrl { get; set; }

        public int Age { get; set; }

        public bool IsSuperGuest { get; set; }

        public User() { }


        public User(string username, string password, Roles role, bool isSuper,int age)
        {
            Username = username;
            Password = password;
            Role = role;
            IsSuper = isSuper;
            Age = age;


        }

        public string[] ToCSV()
        {

            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString(), IsSuper.ToString(), ImageUrl,Age.ToString()};

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Username = values[1];
            Password = values[2];
            Role = (Roles)Enum.Parse(typeof(Roles), values[3]);
            IsSuper = bool.Parse(values[4]);
            ImageUrl = values[5];
            Age = int.Parse(values[6]);

        }
    }
}