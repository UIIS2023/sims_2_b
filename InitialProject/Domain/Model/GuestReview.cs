using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class GuestReview : ValidationBase, ISerializable
    {
        public int Id { get; set; }

        public int IdOwner { get; set; }

        public int IdReservation { get; set; }

        public AccommodationReservation Reservation { get; set; }

        private int _cleanlinessGrade;
        public int CleanlinessGrade
        {
            get => _cleanlinessGrade;
            set
            {
                if (value != _cleanlinessGrade)
                {
                    _cleanlinessGrade = value;
                    OnPropertyChanged(nameof(CleanlinessGrade));
                }
            }
        }

        private int _ruleGrade;
        public int RuleGrade
        {
            get => _ruleGrade;
            set
            {
                if (value != _ruleGrade)
                {
                    _ruleGrade = value;
                    OnPropertyChanged(nameof(RuleGrade));
                }
            }
        }

        public string GuestComment { get; set; }

        public int IdGuest { get; set; }

        protected override void ValidateSelf()
        {
            if (this._cleanlinessGrade == 0)
            {
                this.ValidationErrors["CleanlinessGrade"] = "Grade required.";
            }

            if (this._ruleGrade == 0)
            {
                this.ValidationErrors["RuleGrade"] = "Grade required.";
            }
        }


            public GuestReview()
        {

        }

        public GuestReview(int idOwner, int idReservation, int cleanlinessGrade, int ruleGrade, string comment,int idGuest)
        {
            IdOwner = idOwner;
            IdReservation = idReservation;
            CleanlinessGrade = cleanlinessGrade;
            RuleGrade = ruleGrade;
            GuestComment = comment;
            IdGuest = idGuest;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdOwner = int.Parse(values[1]);
            IdReservation = int.Parse(values[2]);
            CleanlinessGrade = int.Parse(values[3]);
            RuleGrade = int.Parse(values[4]);
            GuestComment = values[5];
            IdGuest=int.Parse(values[6]);


        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdOwner.ToString(),
                IdReservation.ToString(),
                CleanlinessGrade.ToString(),
                RuleGrade.ToString(),
                GuestComment,
                IdGuest.ToString(),


            };
            return csvValues;
        }
    }
}
