using System.Windows;
using System.Windows.Controls;

using Krabbelen.ViewModels;

namespace Krabbelen.Controls
{
	/// <summary>
	/// Interaction logic for KeywordCard.xaml
	/// </summary>
	public partial class KeywordCard : UserControl
	{

		public KeywordCard()
		{
			InitializeComponent();
		}

		private void RemoveKeyword_Click(object sender, RoutedEventArgs e)
		{
			((MainViewModel)DataContext).RemoveKeyword(KeywordText.Text);
		}

	}
}
