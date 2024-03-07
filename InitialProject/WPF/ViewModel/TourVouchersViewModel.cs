using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.View;
using InitialProject.WPF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModel
{
    public class TourVouchersViewModel : ViewModelBase
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly VoucherService _voucherService;

        public static ObservableCollection<Voucher> VouchersMainList { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public static User LoggedInUser { get; set; }
        public static TourReservation TourReservation { get; set; }
        public static Tour SelectedTour { get; set; }

        public TourVouchersViewModel(User user,Tour tour, TourReservation tourReservation)
        {
            _voucherService = new VoucherService();
            LoggedInUser = user;
            TourReservation = tourReservation;
            SelectedTour = tour;
            VouchersMainList = new ObservableCollection<Voucher>(_voucherService.GetUpcomingVouchers(user));
            
        }

    }
}
