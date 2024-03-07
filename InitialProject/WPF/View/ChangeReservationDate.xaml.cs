using InitialProject.Applications.UseCases;
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
    /// Interaction logic for ChangeReservationDate.xaml
    /// </summary>
    public partial class ChangeReservationDate : Window
    {
        public ChangeReservationDate(User user,AccommodationReservation res,ReservationDisplacementRequest req, IMessageBoxService message)
        {
            InitializeComponent();
            ChangeReservationDateViewModel viewModel = new ChangeReservationDateViewModel(user,res,req, message);
            DataContext = viewModel;
            if (viewModel.CloseAction == null)
                viewModel.CloseAction = new Action(this.Close);



        }
    }
    }

