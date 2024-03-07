using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace InitialProject.WPF.ViewModel
{
	public class OwnerForumViewModel : ViewModelBase
	{
		public static User LoggedInUser { get; set; }

		public static ObservableCollection<Forums> AllForums { get; set; }

		public static ObservableCollection<Forums> AvailableForums { get; set; }

		private readonly ForumService forumService;

		private readonly CommentService commentService;

		private readonly AccommodationReservationService accommodationReservationService;

		Notifier notifier = new Notifier(cfg =>
		{
			cfg.PositionProvider = new WindowPositionProvider(
				parentWindow: Application.Current.MainWindow,
				corner: Corner.TopRight,
				offsetX: 10,
				offsetY: 10);

			cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
				notificationLifetime: TimeSpan.FromSeconds(3),
				maximumNotificationCount: MaximumNotificationCount.FromCount(5));

			cfg.Dispatcher = Application.Current.Dispatcher;
			cfg.DisplayOptions.Width = 350;
		});

		private ObservableCollection<Comment> _comments;

		public ObservableCollection<Comment> Comments
		{
			get { return _comments; }
			set
			{
				_comments = value;
				OnPropertyChanged(nameof(Comments));
			}
		}

		private Forums _selectedForum;

		public Forums SelectedForum
		{
			get { return _selectedForum; }

			set
			{
				_selectedForum = value;
				Comments = new ObservableCollection<Comment>(commentService.GetByForum(SelectedForum.Id));
				SetBoolProperties();
				SetReportVisibility();
				OnPropertyChanged(nameof(IsUsefull));
				OnPropertyChanged(nameof(Comments));
				OnPropertyChanged(nameof(SelectedForum));
				if (AvailableForums.Any(forum => forum.Id == value.Id))
				{
					_canComment = true;
					CanComment = true;
				}

				else
				{
					_canComment = false;
					CanComment = false;
				}
				OnPropertyChanged(nameof(CanComment));

			}
		}

		private Comment _selectedComment;

		public Comment SelectedComment
		{
			set { _selectedComment = value; }
			get
			{
				return _selectedComment;
				OnPropertyChanged(nameof(SelectedComment));
			}
		}

		private void SetBoolProperties()
		{
			foreach (Comment comment in Comments)
			{
				if (comment.User.Role == Roles.OWNER)
				{
					comment.IsOwnerComment = true;
				}
			}

			int ownerCommentsCount = Comments.Count(comment => comment.IsOwnerComment);
			int guestCommentsCount = Comments.Count(comment => !comment.IsOwnerComment);

			if (ownerCommentsCount > 1 && guestCommentsCount > 1)
			{
				_isUsefull = true;
				IsUsefull = true;
			}

			else
			{
				_isUsefull = false;
				IsUsefull = false;
			}

			
		}

		private void SetReportVisibility()
		{
			
			foreach(Comment comment in Comments)
			{
				comment.CanReport = false;
				List<Accommodation> accommodations = accommodationReservationService.GetAccommodationsByUser(comment.User);
				if(comment.User.Role == Roles.GUEST1 && !accommodations.Any(accommodation => accommodation.IdLocation == SelectedForum.LocationId)){
					comment.CanReport = true;
				}


			}
		}

		private String _yourComment;
		public String YourComment
		{
			get { return _yourComment; }
			set
			{
				_yourComment = value;

			}
		}

		private bool _canComment;

		public bool CanComment
		{
			get { return _canComment; }
			set
			{
				if (_canComment == value)
				OnPropertyChanged(nameof(CanComment));
			}
		}

		private bool _isUsefull;

		public bool IsUsefull
		{
			get { return _isUsefull; }
			set
			{
				if (_isUsefull == value)
					OnPropertyChanged(nameof(IsUsefull));
			}
		}

		private RelayCommand confirmCreate;
		public RelayCommand ConfirmCreate
		{
			get { return confirmCreate; }
			set
			{
				confirmCreate = value;
			}
		}

		private RelayCommand report;
		public RelayCommand Report
		{
			get { return report; }
			set
			{
				report = value;
			}
		}

		public OwnerForumViewModel(User user)
		{
			forumService = new ForumService();
			commentService = new CommentService();
			accommodationReservationService = new AccommodationReservationService();
			AllForums = new ObservableCollection<Forums>(forumService.GetAll());
			ConfirmCreate = new RelayCommand(Execute_ConfirmCreate, CanExecute_Command);
			Report = new RelayCommand(Execute_Report, CanExecute_Command);
			AvailableForums= new ObservableCollection<Forums>(forumService.GetAvailableForums(user));
			LoggedInUser = user;
		}

		private void Execute_ConfirmCreate(object sender)
		{
			Comment newComment = new Comment(YourComment, LoggedInUser, LoggedInUser.Id, SelectedForum.Id, true, false,0, true,SelectedForum );
			Comment savedComment = commentService.Save(newComment);
			YourComment = "";
			Comments.Add(savedComment);
			var options = new MessageOptions { FontSize = 30 };
			notifier.ShowSuccess(" You have successfully added a comment.", options);
		}

		private bool CanExecute_Command(object parameter)
		{

			return true;
			
		}

		private void Execute_Report(object sender)
		{
			var selectedComment = SelectedComment;
			selectedComment.ReportsNumber++;
			selectedComment.AlreadyReported = false;
			Refresh();
			commentService.Update(selectedComment);
		}

		private void Refresh()
		{
			Comments.Clear();
			foreach(Comment c in commentService.GetByForum(SelectedForum.Id))
			{
				Comments.Add(c);
			}
		}
	}
}
