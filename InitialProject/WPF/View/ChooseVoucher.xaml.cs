using InitialProject.Domain.Model;
using InitialProject.View;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.View
{
    /// <summary>
    /// Interaction logic for ChooseVoucher.xaml
    /// </summary>
    public partial class ChooseVoucher : Window
    {
        public ChooseVoucher(User user, Tour tour, TourReservation tourReservation)
        {
            InitializeComponent();
            ChooseVoucherViewModel chooseVoucherViewModel = new ChooseVoucherViewModel(user, tour, tourReservation);
            DataContext = chooseVoucherViewModel;
            if (chooseVoucherViewModel.CloseAction == null)
            {
                chooseVoucherViewModel.CloseAction = new Action(this.Close);
            }
                
        }
    }
}
