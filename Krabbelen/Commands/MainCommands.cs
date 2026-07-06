using System.Windows.Input;

namespace Krabbelen.Commands
{
	public class MainCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"E_xit",
				"Exit",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt),
					new KeyGesture(Key.W, ModifierKeys.Control)
				}
			);
		
		public static readonly RoutedUICommand NewKrabbel = new RoutedUICommand
			(
				"_New Krabbel",
				"New krabbel",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.N, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand ShowHistory = new RoutedUICommand
			(
				"_Show History",
				"ShowHistory",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.OemQuestion)
				}
			);

	}
}
