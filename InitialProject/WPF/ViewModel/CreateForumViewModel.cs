using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Converters;
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
    public class CreateForumViewModel:BindableBase
    {
        public User LogedInUser { get; set; }
        public Action CloseAction { get; set; }
        public static ObservableCollection<String> Countries { get; set; }

        private readonly LocationService locationService;
        private readonly ForumService forumService;
        private readonly IMessageBoxService messageBoxService;

        private readonly CommentService commentService;
        public Forums SelectedForum { get; set; }
        public Location SelectedLocation { get; set; }

        public Comment comment { get; set; }

        public CreateForumViewModel(User user,Location location)
        {
            forumService = new ForumService();
            locationService= new LocationService();
            commentService= new CommentService();
            LogedInUser = user;
            messageBoxService = new MessageBoxService();
            SelectedLocation = location;
            Cities = new ObservableCollection<String>();
            Countries = new ObservableCollection<String>(locationService.GetAllCountries());
            IsCityEnabled = false;
            InitializeCommands();



        }
        private void InitializeCommands()
        {
            AddForum = new RelayCommand(Execute_AddForum, CanExecute_Command);
        }

        private void Execute_AddForum(object obj)
        {
            List<Forums> allForums = forumService.GetAll();
            List<Location> allLocations = locationService.GetAll();

            foreach (Location loc in allLocations)
            {
                bool matchFound = false;

                foreach (Forums forum in allForums)
                {
                    if (forum.Location.Id == loc.Id)
                    {
                        if (loc.City.Equals(SelectedCity) && loc.Country.Equals(SelectedCountry))
                        {
                            matchFound = true;
                            break;
                        }
                    }
                }

                if (matchFound)
                {
                    messageBoxService.ShowMessage("Forum na temu putovanja u " + SelectedCity + ", " + SelectedCountry + " je već pokrenut. Pogledajte u listi foruma!");
                    return;
                }
            }

            int idForum = forumService.GetNextForumId();
            Comment newComment = new Comment(Comment,LogedInUser,LogedInUser.Id,idForum,false,false,0,true, SelectedForum);
            Comment savedComment = commentService.Save(newComment);

            List<Comment> comments = new List<Comment> { savedComment };
            SelectedLocation = locationService.GetLocationByCityandCountry(SelectedCity, SelectedCountry);
            Forums newForum = new Forums(SelectedLocation, SelectedLocation.Id,LogedInUser.Id, comments, false, LogedInUser);
            Forums savedForum = forumService.Save(newForum, newComment);
            Guest1MainWindowViewModel.Forums.Insert(0, newForum);
            Guest1MainWindowViewModel.YourForums.Add(newForum);
            SortForums();

            CloseAction();
        }

        private void SortForums()
        {
            Guest1MainWindowViewModel.Forums = new ObservableCollection<Forums>(Guest1MainWindowViewModel.SortingForums.OrderByDescending(f => f.Id));
        }


        private bool CanExecute_Command(object parameter)
        {
            return true;
        }
        private RelayCommand addForum;
        public RelayCommand AddForum
        {
            get => addForum;
            set
            {
                if (value != addForum)
                {
                    addForum = value;
                    OnPropertyChanged("AddForum");
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

        private ObservableCollection<String> _cities;
        public ObservableCollection<String> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private bool _isCityEnabled;
        public bool IsCityEnabled
        {
            get { return _isCityEnabled; }
            set
            {
                _isCityEnabled = value;
                OnPropertyChanged(nameof(IsCityEnabled));
            }
        }

        private String _selectedCity;
        public String SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;

            }
        }

        private String _selectedCountry;
        public String SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    Cities = new ObservableCollection<String>(locationService.GetCities(SelectedCountry));
                    if (Cities.Count == 0)
                    {
                        IsCityEnabled = false;
                    }
                    else
                    {
                        IsCityEnabled = true;
                    }
                    OnPropertyChanged(nameof(Cities));
                    OnPropertyChanged(nameof(SelectedCountry));
                    OnPropertyChanged(nameof(IsCityEnabled));
                }
            }
        }
    }
}
