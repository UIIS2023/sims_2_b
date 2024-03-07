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
    /// Interaction logic for Whenever_WhereverWindow.xaml
    /// </summary>
    public partial class Whenever_WhereverWindow : Window
    {
        public Whenever_WhereverWindow(User user,Accommodation accommodation)
        {
            InitializeComponent();
            Whenever_WhereverViewModel whenever_WhereverViewModel = new Whenever_WhereverViewModel(user,accommodation);
            DataContext = whenever_WhereverViewModel;
            if (whenever_WhereverViewModel.CloseAction == null)
                whenever_WhereverViewModel.CloseAction = new Action(this.Close);
        }
    }
}
