using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class Guest1ProfilViewModel:ViewModelBase
    {
        public Action CloseAction;

        private readonly UserService userService;

        public int reservationsLastYear;
        public string UserImageSource { get; set; }
        public User LogedInUser { get; set; }

        public SuperGuest _superGuest { get; set; }
        public int NumberOfMissingRes { get; set; }

        private readonly SuperGuestService superGuestService;
        public static int bonusPoints { get; set; }

        private readonly IMessageBoxService messageBoxService;
        public Guest1ProfilViewModel(User user)
        {
            LogedInUser = user;
            userService=new UserService();
            UserImageSource = userService.GetImageUrlByUserId(user.Id);
            InitializeCommands();
            messageBoxService=new MessageBoxService();
            superGuestService= new SuperGuestService();
            _superGuest = superGuestService.GetSuperGuest(LogedInUser.Id);
            reservationsLastYear = superGuestService.LastYearReservations(Guest1MainWindowViewModel.AccommodationsReservationList);
            IsSuperGuest();


        }

        private bool CanExecute_Command(object parameter)
        {
            return true;
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

        private RelayCommand superGuest;

        public RelayCommand SuperGuest
        {
            get { return superGuest; }
            set
            {
                superGuest = value;
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

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
                OnPropertyChanged(nameof(ImageOpacity));
                OnPropertyChanged(nameof(ImageEffectOpacity));
            }
        }

        public double ImageOpacity => IsEnabled ? 1 : 0.3;

        public double ImageEffectOpacity => IsEnabled ? 1 : 0;
        private void InitializeCommands()
        {
            Close = new RelayCommand(Execute_Close, CanExecute_Command);
            Instruction = new RelayCommand(Execute_Instruction, CanExecute_Command);
            SuperGuest= new RelayCommand(Execute_SuperGuest, CanExecute_Command);



        }

        private void Execute_SuperGuest(object obj)
        {
            NumberOfMissingRes = 10 - reservationsLastYear;


            if (IsSuperGuest())
            {
                SuperGuestProfile superGuestProfile = new SuperGuestProfile(LogedInUser);
                superGuestProfile.Show();
            }
            else
            {
                messageBoxService.ShowMessage("Jos uvijek niste zadovoljili uslove super gosta. Fale vam " + NumberOfMissingRes +" rezervacije!");
            }
        }

        private void Execute_Instruction(object obj)
        {
            messageBoxService.ShowMessage("Gost može postati super-gost ako u prethodnih godinu dana ima bar 10 rezervacija. " +
                "Super-gost titula traje godinu dana i prestaje da važi ako gost ne bude ponovo zadovoljio uslov od 10 rezervacija." +
                " Super-gost dobija 5 bonus poena koje može potrošiti u narednih godinu dana, nakon čega se bodovi resetuju na 0" +
                " (ako gost ne uspe da održi titulu super-gosta onda mu se svakako brišu bonus poeni, a ako uspe da produži onda se " +
                "resetuju i dobija 5 novih, dakle ne mogu se akumulirati). Prilikom svake naredne rezervacije se troši jedan bonus poen" +
                " što donosi popuste, što znači da će super-gost imati 5 rezervacija sa popustom.");
        }

        private void Execute_Close(object obj)
        {
            CloseAction();
        }

       

        private bool IsSuperGuest()
        {
            (bool isSuperGuest, bool isEnabled,int bonus) = superGuestService.CheckSuperGuest(LogedInUser, _superGuest, Guest1MainWindowViewModel.AccommodationsReservationList);
            IsEnabled = isEnabled;
            bonusPoints=bonus;
            return isSuperGuest;
        }



    }
}
