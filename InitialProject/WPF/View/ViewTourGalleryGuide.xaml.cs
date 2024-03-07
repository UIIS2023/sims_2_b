using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
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
using System.Windows.Navigation;
using Image = System.Windows.Controls.Image;


namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ViewTourGalleryGuide.xaml
    /// </summary>
    public partial class ViewTourGalleryGuide : Page
    {

        private readonly IImageRepository _imageRepository;
        public static List<String> ImageUrls = new List<String>();
        public static Tour SelectedTour { get; set; }
        public ViewTourGalleryGuide(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            SelectedTour = selectedTour;
            _imageRepository = Inject.CreateInstance<IImageRepository>();
            ImageUrls = new List<String>(_imageRepository.GetUrlByTourId(SelectedTour.Id));
            DisplayPictures();
        }

        private void DisplayPictures()
        {

            foreach (String url in ImageUrls)
            {

                Image image = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(url, UriKind.Relative);
                bitmapImage.EndInit();
                image.Source = bitmapImage;
                image.Width = 130;
                image.Height = 130;
                image.Margin = new Thickness(20, 0, 10, 20);
                WrapPanel wrapPanel = (WrapPanel)FindName("ImagesPanel");
                wrapPanel.Children.Add(image);
            }
        }


    }
}