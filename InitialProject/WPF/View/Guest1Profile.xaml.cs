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
    /// Interaction logic for Guest1Profile.xaml
    /// </summary>
    public partial class Guest1Profile : Window
    {
        public Guest1Profile(User user)
        {
            InitializeComponent();
            Guest1ProfilViewModel guest1 = new Guest1ProfilViewModel(user);
            DataContext = guest1;
            if (guest1.CloseAction == null)
                guest1.CloseAction = new Action(this.Close);
        }
    }
}
