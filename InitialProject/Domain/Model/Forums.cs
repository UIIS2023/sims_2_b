using InitialProject.Serializer;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class Forums: ValidationBase,ISerializable
    {
        public int Id { get; set; }

        public Location Location { get; set; }

        public int LocationId { get; set; }

        public int IdUser { get; set; }

        public List<Comment> Comments { get; set; }
        public bool IsClosed { get; set; }

        public User User { get; set; }
        private string mark;
        public string Mark
        {
            get { return mark; }
            set
            {
                if (mark != value)
                {
                    mark = value;
                    OnPropertyChanged(nameof(Mark));
                }
            }
        }
        protected override void ValidateSelf()
        {
            if (this.Location.Equals(null))
            {
                this.ValidationErrors["location"] = " Required Text.";
            }

            /*if (this._ruleGrade == 0)
            {
                this.ValidationErrors["RuleGrade"] = "Required grade.";
            }*/
        }
 
        public Forums(Location location,int locationId, int idUser,  List<Comment> Comments,bool isClosed,User user)
        {
            this.Location = location;
            this.LocationId = locationId;
            this.IdUser = idUser;
            this.Comments = Comments;
            this.IsClosed = isClosed;
            this.User = user;
        }

        public Forums()
        {
            Comments = new List<Comment>();

        }

        public string[] ToCSV()
        {


           
            string[] csvValues =

            {
               Id.ToString(),
               IdUser.ToString(),
               LocationId.ToString(),
               IsClosed.ToString(),
               Mark ?? string.Empty

            };
            return csvValues;
        }



        public void FromCSV(string[] values)
        {
           Id = int.Parse(values[0]);
           IdUser = int.Parse(values[1]);   
           LocationId = int.Parse(values[2]);
           IsClosed = bool.Parse(values[3]);
           Mark = string.IsNullOrEmpty(values[4]) ? null : values[4];

            
        }

    }
}
