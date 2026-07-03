using Krabbelen.ViewModels;

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Krabbelen
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		#region [ Fields ]

		#endregion

		#region [ Properties ]

		MainViewModel MainVM { get; set; }

		#endregion

		#region [ Construction ]

		public MainWindow()
		{

			InitializeComponent();

			MainVM = new MainViewModel(this);
			DataContext = MainVM;

		}

		#endregion

		#region [ Public methods ]


		#endregion

		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = true;

		private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e) => 
			MainVM.Shutdown();

		private void NewKrabbelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => 
			e.CanExecute = true;

		private void NewKrabbelCommand_Execute(object sender, ExecutedRoutedEventArgs e) => 
			MainVM.NewKrabbel();

		private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => 
			MainVM.MouseDoubleClick(sender, e);

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			// If there are no unsaved changes, we can exit without prompting the user.
			if (!MainVM.KrabblesChanged) { return; }

			if (MainVM.AskClosing)
			{
				MessageBoxResult result = MessageBox.Show(
					"Do you want to save your changes before closing?",
					"Krabbelen",
					MessageBoxButton.YesNoCancel,
					MessageBoxImage.Question);
				switch (result)
				{
					case MessageBoxResult.Yes:
						MainVM.SaveFile();
						break;
					case MessageBoxResult.No:
						break;
					case MessageBoxResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}

	}
}
