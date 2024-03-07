using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace InitialProject.Domain.Model
{
    public class TourGuideReview : ValidationBase, ISerializable
    {
        public int Id { get; set; }
        public User Guest { get; set; }
        public int IdGuest { get; set; }
        public int IdGuide { get; set; }
        public TourPoint TourPoint{ get; set; }
        public int IdTourPoint { get; set; }
        public List<Image> Images { get; set; }
        public bool IsReviewValid { get; set; }
        public int IdTour { get; set; }

        private int _guideKnowledge;
        public int GuideKnowledge
        {
            get => _guideKnowledge;
            set
            {
                if (value != _guideKnowledge)
                {
                    _guideKnowledge = value;
                    OnPropertyChanged(nameof(GuideKnowledge));
                }
            }
        }

        private int _guideLanguage;
        public int GuideLanguage
        {
            get => _guideLanguage;
            set
            {
                if (value != _guideLanguage)
                {
                    _guideLanguage = value;
                    OnPropertyChanged(nameof(GuideLanguage));
                }
            }
        }

        private int _interestingTour;
        public int InterestingTour
        {
            get => _interestingTour;
            set
            {
                if (value != _interestingTour)
                {
                    _interestingTour = value;
                    OnPropertyChanged(nameof(InterestingTour));
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
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }


        public TourGuideReview()
        {
            Images = new List<Image>();
        }
        public TourGuideReview(int idGuest, int idGuide, int idTourPoint, int guideKnowledge, int guideLanguage, int interestingTour, string comment, int idTour)

        {
            IdGuest = idGuest;
            IdGuide = idGuide;
            IdTourPoint= idTourPoint;
            GuideKnowledge = guideKnowledge;
            GuideLanguage=guideLanguage;
            InterestingTour=interestingTour;
            Comment=comment;
            IsReviewValid = false;
            IdTour=idTour;
            Images = new List<Image>();
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            IdGuest= int.Parse(values[1]);
            IdGuide = int.Parse(values[2]);
            IdTourPoint = int.Parse(values[3]);
            GuideKnowledge = int.Parse(values[4]);
            GuideLanguage = int.Parse(values[5]);
            InterestingTour = int.Parse(values[6]);
            Comment = values[7];
            IsReviewValid = bool.Parse(values[8]);
            IdTour = int.Parse(values[9]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                IdGuest.ToString(),
                IdGuide.ToString(),
                IdTourPoint.ToString(),
                GuideKnowledge.ToString(),
                GuideLanguage.ToString(),
                InterestingTour.ToString(),
                Comment,
                IsReviewValid.ToString(),
                IdTour.ToString()
            };

            return csvValues;
        }

        protected override void ValidateSelf()
        {
            if (this._guideKnowledge == 0)
            {
                this.ValidationErrors["GuideKnowledge"] = "GuideKnowledge is required.";
            }
            if (this._guideLanguage == 0)
            {
                this.ValidationErrors["GuideLanguage"] = "GuideLanguage is required.";
            }
            if(this._interestingTour == 0)
            {
                this.ValidationErrors["InterestingTour"] = "InterestingTour is required.";
            }
            if (string.IsNullOrWhiteSpace(this._comment))
            {
                this.ValidationErrors["Comment"] = "Comment cannot be empty.";
            }
        }
    }
}
