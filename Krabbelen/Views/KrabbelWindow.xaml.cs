using Krabbelen.Models;
using Krabbelen.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Krabbelen.Views
{
	/// <summary>
	/// Interaction logic for KlabbelWindow.xaml
	/// </summary>
	public partial class KrabbelWindow : Window
	{
		private KrabbelViewModel kVM;

		public KrabbelWindow(KrabbelViewModel krabbelViewModel)
		{
			InitializeComponent();
			DataContext = krabbelViewModel;

			kVM = krabbelViewModel;
			KrabbelTextBox.Focus();
		}

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = !string.IsNullOrEmpty(KrabbelTextBox.Text);

		private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			kVM.VM.SaveKrabbel();
			DialogResult = true;
		}

		private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = true;

		private void CancelCommand_Execute(object sender, ExecutedRoutedEventArgs e) => 
			DialogResult = false;

	}
}
