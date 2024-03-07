using InitialProject.Domain.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Image = System.Windows.Controls.Image;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ViewGallery.xaml
    /// </summary>
    public partial class ViewAccommodationGallery : UserControl
	{
		public static Accommodation SelectedAccommodation { get; set; }

		
        private readonly ImageRepository _imageRepository;

		public static List<String> ImageUrls = new List<String>();
		public ViewAccommodationGallery(Accommodation selectedAccommodation)
		{
			InitializeComponent();
			DataContext = this;
			SelectedAccommodation = selectedAccommodation;
			_imageRepository = new ImageRepository();
			ImageUrls = new List<String>(_imageRepository.GetUrlByAccommodationId(SelectedAccommodation.Id));
			

			DisplayPictures();

		}

		private void DisplayPictures()
		{
			
			foreach(String url in ImageUrls)
			{
				
				Image image = new Image();
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = new Uri(url, UriKind.Relative);
				bitmapImage.EndInit();
				image.Source = bitmapImage;
				image.Width = 190;
				image.Height = 230;
				image.Margin = new Thickness(35,0,35,10);
				WrapPanel wrapPanel = (WrapPanel)FindName("ImagesPanel");
				wrapPanel.Children.Add(image);

			}
			
		}

	}
}
