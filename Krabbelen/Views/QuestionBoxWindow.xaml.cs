using System.Windows;

using Krabbelen.ViewModels;

namespace Krabbelen.Views
{

	/// <summary>
	/// Interaction logic for QuestionBoxWindow.xaml
	/// </summary>
	public partial class QuestionBoxWindow : Window
	{
		private QuestionBoxViewModel VM;

		public QuestionBoxWindow(QuestionBoxViewModel questionBoxViewModel)
		{
			InitializeComponent();
			VM = questionBoxViewModel;
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			VM.AnswerKeyUp(sender, e);
		}

	}

}
