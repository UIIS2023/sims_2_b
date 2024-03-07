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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.View
{
	/// <summary>
	/// Interaction logic for Tutorial.xaml
	/// </summary>
	public partial class Tutorial : UserControl
	{
		public Tutorial()
		{
			InitializeComponent();
			media.Source = new Uri(@"C:\Users\Hp\Downloads\tutorijal.mp4");
			media.LoadedBehavior = MediaState.Manual;
			media.UnloadedBehavior = MediaState.Manual;
			media.Volume = 70;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			media.Pause();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			media.Play();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(0);
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(160);
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(202);
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(249);
		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(305);
		}

		private void Button_Click_7(object sender, RoutedEventArgs e)
		{
			media.Position = TimeSpan.FromSeconds(414);
		}
	}
}
