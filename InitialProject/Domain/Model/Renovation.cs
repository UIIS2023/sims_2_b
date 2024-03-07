using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
	public class Renovation : ValidationBase, ISerializable
	{
		public int Id { get; set; }

		private DateOnly inputStartdate { get; set; }
		public DateOnly StartDate
		{
			get => inputStartdate;
			set
			{
				if (value != inputStartdate)
				{
					inputStartdate = value;
					OnPropertyChanged(nameof(StartDate));
				}
			}
		}

		private DateOnly inputEnddate { get; set; }
		public DateOnly EndDate
		{
			get => inputEnddate;
			set
			{
				if (value != inputEnddate)
				{
					inputEnddate = value;
					OnPropertyChanged(nameof(EndDate));
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

		public string Description { get; set; }

		public int AccommodationId  { get; set; }

		public Accommodation Accommodation { get; set; }

		public bool IsEnabledForCancel { get; set; }

		public bool IsRenovated { get; set; }

		public Renovation( DateOnly startDate, DateOnly endDate, int duration, string description, int accommodationId, Accommodation accommodation,bool isEnabledForcancel, bool isRenovated)
		{
			
			StartDate = startDate;
			EndDate = endDate;
			Duration = duration;
			Description = description;
			AccommodationId = accommodationId;
			Accommodation = accommodation;
			IsEnabledForCancel = isEnabledForcancel;
			IsRenovated = isRenovated;

		}

		public Renovation()
		{

		}

		public void FromCSV(string[] values)
		{
			Id = int.Parse(values[0]);
			StartDate = DateOnly.Parse(values[1]);
			EndDate = DateOnly.Parse(values[2]);
			Duration = int.Parse(values[3]);
			Description = values[4];
			AccommodationId = int.Parse(values[5]);
			IsEnabledForCancel = bool.Parse(values[6]);
			IsRenovated = bool.Parse(values[7]);


		}

		public string[] ToCSV()
		{
			string[] csvValues =
			{
				Id.ToString(),
				StartDate.ToShortDateString(),
				EndDate.ToShortDateString(),
				Duration.ToString(),
				Description,
				AccommodationId.ToString(),
				IsEnabledForCancel.ToString(),
				IsRenovated.ToString()


			};
			return csvValues;
		}

		protected override void ValidateSelf()
		{
			if (StartDate == default(DateOnly))
			{
				this.ValidationErrors["StartDate"] = "Date required.";
			}

			if (EndDate == default(DateOnly))
			{
				this.ValidationErrors["EndDate"] = "Date required.";

			}

			if (this._duration == 0)
			{
				this.ValidationErrors["Duration"] = "Duration required.";
			}



			if (StartDate >= EndDate)
			{
				this.ValidationErrors["StartDate"] = "Start date must be before end date.";
				this.ValidationErrors["EndDate"] = "End date must be after start date.";
			}
		}
	}
}
