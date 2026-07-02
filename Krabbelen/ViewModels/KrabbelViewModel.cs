using Krabbelen.Models;
using Krabbelen.Views;

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

		#endregion

		#region [ Construction ]

		public KrabbelViewModel(MainViewModel mainViewModel)
		{
			VM = mainViewModel;
		}

		#endregion

		#region [ Public methods ]

		public void Show(Krabbel selectedKrabbel)
		{
			SelectedKrabbel = selectedKrabbel;
			KrabbelWindow view = new KrabbelWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
				Title = "New Krabbel",
				// DataContext = this [Wrong data context]
			};
			View = view;
			View.ShowDialog();
		}

		#endregion


	}
}
