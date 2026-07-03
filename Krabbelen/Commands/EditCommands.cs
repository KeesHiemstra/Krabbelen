using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Krabbelen.Commands
{
	public class EditCommands
	{
		public static readonly RoutedUICommand Save = new RoutedUICommand
			(
				"_Save",
				"Save",
				typeof(EditCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.S, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand Cancel = new RoutedUICommand
			(
				"_Cancel",
				"Cancel",
				typeof(EditCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Delete = new RoutedUICommand
			(
				"_Delete",
				"Delete",
				typeof(EditCommands),
				new InputGestureCollection() { }
			);

	}
}
