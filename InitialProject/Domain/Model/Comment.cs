using InitialProject.Serializer;
using InitialProject.Validations;
using System;

namespace InitialProject.Domain.Model
{
    public class Comment : ValidationBase, ISerializable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
       

        public Forums forum { get; set; }
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

        public int UserId { get; set; }
        public int ForumId { get; set; }

        public bool IsOwnerComment { get; set; }

        public bool CanReport { get; set; }

        public int ReportsNumber { get; set; }

        public bool AlreadyReported { get; set; }
        public Comment() { }

        public Comment(string text, User user, int idForum,Forums forum)
        {
            Text = text;
            User = user;
            ForumId = idForum;
            this.forum = forum;
}
        

        public Comment(string text, User user, int userId, int forumId, bool isOwnerComment, bool canReport, int reportsNumber, bool alreadyReported, Forums forum)
        {
            Text = text;
            User = user;
            UserId = userId;
            ForumId = forumId;
            IsOwnerComment = isOwnerComment;
            CanReport = canReport;
            ReportsNumber = reportsNumber;
            AlreadyReported = alreadyReported;
            this.forum=forum;
        }
        protected override void ValidateSelf()
        {
            if (this.Text.Equals(null))
            {
                this.ValidationErrors["Text"] = " Required Text.";
            }

            /*if (this._ruleGrade == 0)
            {
                this.ValidationErrors["RuleGrade"] = "Required grade.";
            }*/
        }
        public string[] ToCSV()
        {

            string[] csvValues =

             {
               Id.ToString(),
               UserId.ToString(),
               Text.ToString(),
               ForumId.ToString(),
               IsOwnerComment.ToString(),
               CanReport.ToString(),
               ReportsNumber.ToString(),
               AlreadyReported.ToString(),
               Mark 
               

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {


            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            Text = values[2];
            ForumId = int.Parse(values[3]);
            IsOwnerComment = bool.Parse(values[4]);
            CanReport = bool.Parse(values[5]);
            ReportsNumber = int.Parse(values[6]);
            AlreadyReported = bool.Parse(values[7]);
            Mark = values[8];

        }

    }
}
