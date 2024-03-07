using InitialProject.Applications.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.WPF.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : UserControl
	{
		

       public OwnerMainWindow(User user, OwnerMainWindowViewModel ownerMainWindowViewModel)
		{
			InitializeComponent();
			DataContext = ownerMainWindowViewModel;
		    Loaded += Window_Loaded;

		}

	   private void Window_Loaded(object sender, RoutedEventArgs e )
		{
			if (OwnerMainWindowViewModel.FilteredReservations.Count > 0)
			{
				MessageBox.Show("Neki gosti nisu ocenjeni. Ukoliko zelite da ih ocenite otidjite na tab Guests for evaluation");
				
			}
		}

		
	}
}
