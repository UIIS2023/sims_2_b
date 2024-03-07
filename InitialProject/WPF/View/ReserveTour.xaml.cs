using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.WPF.View;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ReserveTour.xaml
    /// </summary>
    public partial class ReserveTour : Window

    { 
        public ReserveTour(Tour tour, TourReservation reserve, User user)
        {
            InitializeComponent();
            ReserveTourViewModel reserveTourViewModel = new ReserveTourViewModel(tour, reserve, user);
            DataContext = reserveTourViewModel;
            if (reserveTourViewModel.CloseAction == null)
                reserveTourViewModel.CloseAction = new Action(this.Close);

        }
        

    }
}
