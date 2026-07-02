using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Krabbelen.Views;

namespace Krabbelen.ViewModels
{
	public class QuestionBoxViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private QuestionBoxWindow View;
		private ObservableCollection<string>? ForbiddenKeywords;

		#endregion

		#region [ Properties ]


		#endregion

		#region [ Construction ]

		public QuestionBoxViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
		}

		#endregion

		#region [ Public methods ]

		public string Show(
			string question, 
			string title, 
			ObservableCollection<string>? forbiddenKeywords)
		{
			string result = string.Empty;

			// Layout window
			QuestionBoxWindow view = new QuestionBoxWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
				Title = title,
				DataContext = this
			};

			View = view;
			ForbiddenKeywords = forbiddenKeywords;

			View.Answer.Focus();
			View.ShowDialog();

			// Process the answer
			result = View.Answer.Text.Trim();

			return result;
		}

		#endregion

		internal void AnswerKeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					// Cancel the question box by clearing the answer and closing the window.
					View.Answer.Text = string.Empty;
					View.Close();
					break;
				case Key.Enter:
					if (View.Answer.Text == string.Empty)
					{
						// Do not accept empty answers
						return;
					}
					if (!ValidAnswer(View.Answer.Text))
					{
						// Do not accept invalid answers
						return; 
					}
					// Process the answer
					View.Close();
					break;
			}
		}

		private bool ValidAnswer(string text)
		{
			if (ForbiddenKeywords == null)
				return true;

			bool result = false;

			text = text.Trim().ToLower();
			string foundKeyword = ForbiddenKeywords.FirstOrDefault(x => x.Trim().ToLower() == text);
			if (foundKeyword == null)
			{
				result = true;
			}

			return result;
		}

	}
}
