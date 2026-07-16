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

#if DEBUG
			Log.Write("Krabbelen started");
#endif

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

		private void Window_Closing(object sender, CancelEventArgs e) => MainVM.WindowClosing(sender, e);

		private void ShowHistoryCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) =>
			e.CanExecute = true;

		private void ShowHistoryCommand_Execute(object sender, ExecutedRoutedEventArgs e) =>
			MainVM.ShowHistory();

	}
}
