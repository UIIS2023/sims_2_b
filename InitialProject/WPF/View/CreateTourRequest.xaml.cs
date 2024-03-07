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
    /// Interaction logic for CreateTourRequest.xaml
    /// </summary>
    public partial class CreateTourRequest : Window
    {
        public CreateTourRequest(User user, int requestNumber, ComplexTourRequests complexTourRequest)
        {
            InitializeComponent();
            CreateTourRequestViewModel createTourRequestViewModel = new CreateTourRequestViewModel(user, complexTourRequest);
            DataContext = createTourRequestViewModel;
            if (createTourRequestViewModel.CloseAction == null)
            {
                createTourRequestViewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
