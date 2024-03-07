using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class ChooseVoucherViewModel : ViewModelBase
    {
        private readonly VoucherService _voucherService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly TourAttendanceService _tourAttendanceService;
        private readonly IMessageBoxService _messageBoxService;

        public static ObservableCollection<Voucher> VouchersMainList { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public Action CloseAction { get; set; }
        public static User LoggedInUser { get; set; }
        public static TourReservation TourReservation { get; set; }
        public static Tour SelectedTour { get; set; }
        public ICommand UseVoucherCommand { get; set; }
        public ICommand CancelVoucherCommand { get; set; }
        public ChooseVoucherViewModel(User user, Tour tour, TourReservation tourReservation) 
        {
            _voucherService = new VoucherService();
            _tourReservationService = new TourReservationService();
            _tourAttendanceService= new TourAttendanceService();
            _tourService = new TourService();
            _messageBoxService = new MessageBoxService();
            LoggedInUser = user;
            TourReservation = tourReservation;
            SelectedTour = tour;
            VouchersMainList = new ObservableCollection<Voucher>(_voucherService.GetUpcomingVouchers(user));
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            UseVoucherCommand = new RelayCommand(Execute_UseVoucherCommand, CanExecute_Command);
            CancelVoucherCommand =  new RelayCommand(Execute_CancelVoucherCommand, CanExecute_Command);

        }


        private void Execute_CancelVoucherCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_Command(object arg)
        {
            return true;
        }

        private void Execute_UseVoucherCommand(object obj)
        {
            if (TourReservation != null)
            {
                AlreadyUsedVoucherCheck();
            }
            else if (SelectedTour != null)
            {
                CheckVoucherForTour();
            }

        }

        private void AlreadyUsedVoucherCheck()
        {
            if (TourReservation.UsedVoucher==true)
            {
                _messageBoxService.ShowMessage("You can't use voucher because you already used it for this reservation!");
                CloseAction();
            }
            else
            {
                CheckVoucherForReservedTour();
            }
        }

        private void CheckVoucherForTour()
        {
            if (SelectedVoucher != null)
            {
                SelectedTour.UsedVoucher=true;
                _tourService.Update(SelectedTour);
                _voucherService.Delete(SelectedVoucher);
                CloseAction();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a voucher which you want to use");

            }
        }


        private void CheckVoucherForReservedTour()
        {
            if (SelectedVoucher != null)
            {
                TourReservation.UsedVoucher=true;
                _tourReservationService.Update(TourReservation);
                _voucherService.Delete(SelectedVoucher);
                CloseAction();
            }
            else
            {
                _messageBoxService.ShowMessage("Choose a voucher which you want to use");
            }
        }

    }
}
