using Krabbelen.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;


namespace Krabbelen.ViewModels
{
	public partial class MainViewModel : BaseViewModel
	{

		#region [ Fields ]

		public readonly MainWindow View;

		#endregion

		#region [ Properties ]

		public Krabbel SelectedKrabbel { get; set; }
		public ObservableCollection<Krabbel> Krabbels { get; set; } = 
			new ObservableCollection<Krabbel>();

		#endregion

		#region [ Construction ]

		internal MainViewModel(MainWindow view)
		{
			View = view;

			#region Window Title

			string name = Assembly.GetExecutingAssembly().GetName().Name;
			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
			View.Title = $"{name} - {version} (Debug)";
#else
      View.Title = $"{name} - {version}";
#endif

			#endregion


			Krabbels.Add(new Krabbel() { Id = 1, Text = "Test Krabbel" });
		}

		#endregion

		#region [ Public methods ]


		#endregion


		internal void NewKrabbel()
		{
			SelectedKrabbel = new Krabbel();
			OpenKrabbel(SelectedKrabbel);
		}

		internal void OpenKrabbel(Krabbel selectedKrabbel)
		{
			SelectedKrabbel = selectedKrabbel;
			KrabbelViewModel view = new KrabbelViewModel(this);
			view.Show(SelectedKrabbel);
		}

		public void SaveKrabbel()
		{
			if (SelectedKrabbel.Id == 0)
			{
				Krabbels.Add(SelectedKrabbel);
				SelectedKrabbel.Id = Krabbels.Count;
				SelectedKrabbel.Changed = DateTime.Now;

			}
		}

		internal void MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender == null) { return; }
			foreach (Krabbel item in ((DataGrid)e.Source).SelectedItems)
			{
				if (item.Id != 0)
				{
					OpenKrabbel(item);
				}
			}

		}

		internal void KeyDown(object sender, KeyEventArgs e)
		{
			if (sender == null) { return; }
			foreach (Krabbel item in ((DataGrid)e.Source).SelectedItems)
			{
				if (item.Id != 0)
				{
					OpenKrabbel(item);
				}
			}
		}
	}
}
