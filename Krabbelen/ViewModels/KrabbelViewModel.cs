using Krabbelen.Models;
using Krabbelen.Views;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Krabbelen.ViewModels
{
	public partial class KrabbelViewModel : BaseViewModel
	{

		#region [ Fields ]

		private KrabbelWindow View;

		#endregion

		#region [ Properties ]

		public MainViewModel VM { get; set; }
		public Krabbel SelectedKrabbel { get; set; }
		public List<string> Subjects { get; set; }

		#endregion

		#region [ Construction ]

		public KrabbelViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
			//Collect all distinct subjects from the Krabbels collection and order them alphabetically
			Subjects = new List<string>(VM.Krabbels
				.Select(k => k.Subject)
				.Distinct()
				.OrderBy(s => s));
		}

		#endregion

		#region [ Public methods ]

		public bool Show(Krabbel selectedKrabbel)
		{
			SelectedKrabbel = selectedKrabbel;
			KrabbelWindow view = new KrabbelWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 40,
				Title = SelectedKrabbel.Id == 0 ? "New Krabbel" : $"Edit Krabbel ({SelectedKrabbel.Id})",
				// DataContext = this //[Wrong data context]
			};

			View = view;
			View.SubjectComboBox.ItemsSource = Subjects;
			View.KrabbelTextBox.Focus();
			bool? result = View.ShowDialog();
			return result ?? false;
		}

		#endregion


	}
}
