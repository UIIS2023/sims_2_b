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

namespace InitialProject.WPF.View
{
    /// <summary>
    /// Interaction logic for Guest1NotiificationsView.xaml
    /// </summary>
    public partial class Guest1NotiificationsView : Window
    {
        public Guest1NotiificationsView(User user)
        {
            InitializeComponent();
            Guest1NotificationsViewModel guest1NotiificationsViewModel = new Guest1NotificationsViewModel(user);
            DataContext = guest1NotiificationsViewModel;
          
            if (guest1NotiificationsViewModel.CloseAction == null)
                guest1NotiificationsViewModel.CloseAction = new Action(this.Close);
        }
    }
}
