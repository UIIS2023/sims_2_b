using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class ReccommendationViewModel : BindableBase
    {
        private readonly IMessageBoxService messageBoxService;

        private readonly RecommendationService recommendationService;
        public Action CloseAction { get; set; }

        public User LogedInUser { get; set; }

        public OwnerReview SelectedRate { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        public RecommendationOnAccommodation recommendationOnAccommodation = new RecommendationOnAccommodation();
        
        public ReccommendationViewModel(User user, IMessageBoxService _messageBoxService, OwnerReview ownerReview)
        {
            InitializeCommands();
            LogedInUser=user;
            messageBoxService = _messageBoxService;
            recommendationService= new RecommendationService();
            SelectedRate = ownerReview;

        }

        private RelayCommand reccommend;
        public RelayCommand Reccommend
        {
            get { return reccommend; }
            set
            {
                reccommend = value;
            }
        }

        private RelayCommand close;
        public RelayCommand Close
        {
            get { return close; }
            set
            {
                close = value;
            }
        }

        private RelayCommand instruction;
        public RelayCommand Instruction
        {
            get { return instruction; }
            set
            {
                instruction = value;
            }
        }

        public RecommendationOnAccommodation RecommendationOnAccommodations
        {
            get { return recommendationOnAccommodation; }
            set
            {
                recommendationOnAccommodation = value;
                OnPropertyChanged("RecommendationOnAccommodations");
            }
        }
        private void InitializeCommands()
        {
            Reccommend= new RelayCommand(Execute_Recommend, CanExecute_Command);
            Close = new RelayCommand(Execute_Close, CanExecute_Command);
            Instruction= new RelayCommand(Execute_Instruction, CanExecute_Command);

        }

        private void Execute_Instruction(object obj)
        {
            messageBoxService.ShowMessage( "Nivo 1 - bilo bi lepo renovirati neke sitnice, ali sve funkcioniše kako treba i bez toga \r\n" +
"Nivo 2 - male zamerke na smeštaj koje kada bi se uklonile bi ga učinile savršenim \r\n"+
"Nivo 3 - nekoliko stvari koje su baš zasmetale bi trebalo renovirati \r\n" +
"Nivo 4 - ima dosta loših stvari i renoviranje je stvarno neophodno \r\n" +
 "Nivo 5 - smeštaj je u jako lošem stanju i ne vredi ga uopšte iznajmljivati ukoliko se ne renovira \r\n");
        }

        private void Execute_Recommend(object obj)
        {
            RecommendationOnAccommodations.Validate();
            if (RecommendationOnAccommodations.IsValid)
            {
                RecommendationOnAccommodation newRecommend = new(SelectedRate, RecommendationOnAccommodations.Comment, SelectedRate.Id, (LevelType)Enum.Parse(typeof(LevelType), Level), LogedInUser.Id);
                RecommendationOnAccommodation savedRecommend = recommendationService.Save(newRecommend);
                Guest1MainWindowViewModel.RecommendationList.Add(savedRecommend);
                CloseAction();
            }
        }

        private void Execute_Close(object obj)
        {
            CloseAction();
        }

      
        

        private string level;

        public string Level
        {
            get => level;
            set
            {
                if (value != level)
                {
                    level = value;
                    OnPropertyChanged(nameof(Level));
                }
            }
        }
        private bool CanExecute_Command(object parameter)
        {
            return true;
        }

    }
}
