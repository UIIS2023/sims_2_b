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
    /// Interaction logic for MenuWindowGuest2.xaml
    /// </summary>
    public partial class MenuWindowGuest2 : Window
    {
        public MenuWindowGuest2(User user, MenuWindowGuest2ViewModel menuWindowGuest2ViewModel)
        {
            InitializeComponent();
            DataContext = menuWindowGuest2ViewModel;
            if (menuWindowGuest2ViewModel.CloseAction == null)
                menuWindowGuest2ViewModel.CloseAction = new Action(() =>
                {
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                });

        }
    }
}
