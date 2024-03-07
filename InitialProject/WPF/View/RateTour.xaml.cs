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
using Image = InitialProject.Domain.Model.Image;

namespace InitialProject.WPF.View
{
    /// <summary>
    /// Interaction logic for RateTour.xaml
    /// </summary>
    public partial class RateTour : Window
    {
        public RateTour(User user, TourAttendance selectedTourAttendence)
        {
            InitializeComponent();
            RateTourViewModel rateTourViewModel = new RateTourViewModel(user, selectedTourAttendence);
            DataContext=rateTourViewModel;
            if (rateTourViewModel.CloseAction == null)
                rateTourViewModel.CloseAction = new Action(this.Close);


        }

    }
}
