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
    /// Interaction logic for RecommendationView.xaml
    /// </summary>
    public partial class RecommendationView : Window
    {
        public RecommendationView(User user, IMessageBoxService  message , OwnerReview ownerReview)
        {
            InitializeComponent();
            ReccommendationViewModel reccommendationOnAccommodationViewModel = new ReccommendationViewModel(user, message, ownerReview);
            DataContext = reccommendationOnAccommodationViewModel;
            if (reccommendationOnAccommodationViewModel.CloseAction == null)
                reccommendationOnAccommodationViewModel.CloseAction = new Action(this.Close);
        }
    }
}
