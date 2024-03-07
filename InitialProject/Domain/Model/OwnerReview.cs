using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
	public class OwnerReview :ValidationBase, ISerializable
    {
		public int Id { get; set; }

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

		private int _corectness;
		public int OwnerCorrectness
		{
			get => _corectness;
			set
			{
				if (value != _corectness)
				{
					_corectness = value;
					OnPropertyChanged(nameof(OwnerCorrectness));
				}
			}
		}

		private string _comment;
		public string Comment
		{
			get => _comment;
			set
			{
				if (value != _comment)
				{
					_comment = value;
					OnPropertyChanged("Comment");
				}
			}
		}
		private string imageUrl;
		public string ImageUrl
		{
			get => imageUrl;
			set
			{
				if (value != imageUrl)
				{
					imageUrl = value;
					OnPropertyChanged("ImageUrl");
				}
			}
		}

		public int ReservationId { get; set; }

        public int IdUser { get; set; }
		protected override void ValidateSelf()
		{
			if (this._cleanlinessGrade == 0)
			{
				this.ValidationErrors["CleanlinessGrade"] = "CleanlinessGrade is required.";
			}
			if (this._corectness == 0)
			{
				this.ValidationErrors["OwnerCorrectness"] = "OwnerCorrectness is required.";
			}
			if (string.IsNullOrWhiteSpace(this._comment))
			{
				this.ValidationErrors["Comment"] = "Comment cannot be empty.";
			}
			if (string.IsNullOrWhiteSpace(this.imageUrl))
			{
				this.ValidationErrors["ImageUrl"] = "ImageUrl cannot be empty.";
			}
		}

		public AccommodationReservation Reservation { get; set; }

		public OwnerReview()
		{

		}

		public OwnerReview(int ownerCorrectness, int cleanliness, string comment, int reservationId, AccommodationReservation reservation,int idGuest)
		{
			OwnerCorrectness = ownerCorrectness;
			CleanlinessGrade = cleanliness;
			Comment = comment;
			ReservationId = reservationId;
			Reservation = reservation;
            IdUser = idGuest;

		}

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                OwnerCorrectness.ToString(),
                CleanlinessGrade.ToString(),
                Comment,
                ReservationId.ToString(),
                IdUser.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            OwnerCorrectness= int.Parse(values[1]);
            CleanlinessGrade= int.Parse(values[2]);
            Comment = values[3];
            ReservationId = int.Parse(values[4]);
            IdUser = int.Parse(values[5]);

        }
    }
}
