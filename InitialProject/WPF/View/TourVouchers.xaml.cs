using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TourVouchers.xaml
    /// </summary>
    public partial class TourVouchers : UserControl
    {
        
        public TourVouchers(User user, Tour tour, TourReservation tourReservation, TourVouchersViewModel tourVouchersViewModel)
        {
            InitializeComponent();
            DataContext=tourVouchersViewModel;
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
