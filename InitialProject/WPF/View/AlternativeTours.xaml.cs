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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AlternativeTours.xaml
    /// </summary>
    public partial class AlternativeTours : Window
    {
        public AlternativeTours(User user, Tour tour, TourReservation tourReservation, int againGuestNum, Tour alternativeTour)
        {
            InitializeComponent();
            AlternativeToursViewModel alternativeTourViewModel = new AlternativeToursViewModel(user, tour, tourReservation, againGuestNum, alternativeTour);
            DataContext = alternativeTourViewModel;
            if (alternativeTourViewModel.CloseAction == null)
                alternativeTourViewModel.CloseAction = new Action(this.Close);
        }
    }
}
