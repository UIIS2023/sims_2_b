using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class ForumDisplayViewModel:BindableBase
    {
        public Action CloseAction;
        public User LogedInUser { get; set; }
        private readonly CommentService commentService;

        private readonly ForumService forumService;

        private readonly LocationService locationService;
        private readonly UserService userService;
        public Forums SelectedForum { get; set; }

        public static ObservableCollection<Comment> Comments { get; set; }
        public Comment comment1 = new Comment();

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }
        private string url;
        public string ImageUrl
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }

        private string comment;
        public string NewCommentText
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(NewCommentText));
                }
            }
        }

        
        private bool _isClosed;
        public bool IsEnabled
        {
            get { return _isClosed; }
            set
            {
                if (_isClosed != value)
                {
                    _isClosed = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }
        public Comment CommentDisplay
        {
            get { return comment1; }
            set
            {
                comment1 = value;
                OnPropertyChanged("Comment");
            }
        }
        public ForumDisplayViewModel(User user, Forums forum)
        {
            LogedInUser = user;
            forumService = new ForumService();
            commentService=new CommentService();
            locationService= new LocationService();
            SelectedForum = forum;
            userService= new UserService();
            Comments = new ObservableCollection<Comment>(commentService.GetCommentsByForum(forum.Id));
            CheckStatus();

            ImageUrl = userService.GetImageUrlByUserId(LogedInUser.Id);
            BindUser();
            City = forum.Location.City;
            Country = forum.Location.Country;
            InitializeCommands();
            DisplaySpecial();

        }

        public void DisplaySpecial()
        {

            foreach (Comment comment in Comments)
            {
                comment.forum = SelectedForum;
                int userId = comment.User.Id;

               
                    bool hasVisitedLocation = locationService.HasUserVisitedLocation(userId, comment.forum.Location.Id, comment);

                    if (hasVisitedLocation )
                    {
                        comment.Mark ="✅";
                        commentService.Update(comment);
                    }
                
            }
        }



        public void CheckStatus()
        {
            if(SelectedForum.IsClosed)
            {
                IsEnabled = false;
                NewCommentText = "Forum is closed..";
                Country += "🔐";
            }
            else IsEnabled = true;
        }

        private void BindUser()
        {

            foreach (Comment c in Comments)
            {

                c.User = userService.GetById(c.UserId);

            }
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private RelayCommand sendCommentCommand;
        public RelayCommand SendCommentCommand
        {
            get { return sendCommentCommand; }
            set
            {
                sendCommentCommand = value;
            }
        }

        private void InitializeCommands()
        {
            SendCommentCommand = new RelayCommand(Execute_SendCommentCommand, CanExecute_Command);

        }

        private void Execute_SendCommentCommand(object obj)
        {

            Comment newComment = new Comment(NewCommentText, LogedInUser, LogedInUser.Id, SelectedForum.Id, false, false, 0, true, SelectedForum);
            Comment savedComment = commentService.Save(newComment);

            SelectedForum.Comments.Add(savedComment);
            forumService.Update(SelectedForum);
            Comments.Add(newComment);          
        }
    }
}
