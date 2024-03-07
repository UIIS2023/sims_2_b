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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourPoints.xaml
    /// </summary>
    public partial class TourPoints : Page
    {
        public TourPointsViewModel pointsView;
        public TourPoints(TourPointsViewModel tourPointsViewModel)
        {
            InitializeComponent();
            pointsView= tourPointsViewModel;
            DataContext = tourPointsViewModel;

        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            pointsView.Changed();

        }
    }
}
