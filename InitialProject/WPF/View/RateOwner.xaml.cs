using InitialProject.Domain.Model;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for RateOwner.xaml
    /// </summary>
    public partial class RateOwner : Window
    {
        public RateOwner(User user,AccommodationReservation reservation)
        {
            InitializeComponent();
            RateOwnerViewModel viewModel = new RateOwnerViewModel(user,reservation);
            DataContext = viewModel;
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(this.Close);
        }

        
    }
}
