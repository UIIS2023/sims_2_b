using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Repository;
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
    /// Interaction logic for CreateReservation.xaml
    /// </summary>
    public partial class CreateReservation : Window
    {
      
        public CreateReservation(Accommodation SelectedAccommodation,User user,AccommodationReservation accommodationReservation, IMessageBoxService message)
        {
            InitializeComponent();
            CreateReservationViewModel createReservation= new CreateReservationViewModel(SelectedAccommodation, user, accommodationReservation, message);
            DataContext = createReservation;
            if (createReservation.CloseAction == null)
                createReservation.CloseAction = new Action(this.Close);

        }



    }
}

