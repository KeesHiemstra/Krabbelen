using Krabbelen.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace Krabbelen.Views
{
	/// <summary>
	/// Interaction logic for KlabbelWindow.xaml
	/// </summary>
	public partial class KrabbelWindow : Window
	{
		public MainViewModel VM { get; set; }

		public KrabbelWindow(KrabbelViewModel krabbelViewModel)
		{
			InitializeComponent();

			VM = krabbelViewModel.VM;
			DataContext = VM;
			KrabbelTextBox.Focus();
		}

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = !string.IsNullOrEmpty(KrabbelTextBox.Text);

		private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			VM.SaveKrabbel();
			DialogResult = true;
		}

		private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = true;

		private void CancelCommand_Execute(object sender, ExecutedRoutedEventArgs e) => 
			DialogResult = false;

		private void AddKeyword(object sender, RoutedEventArgs e)
		{
			VM.CreateNewKeyword();
		}

	}
}
