using CHi.Extensions;

using Krabbelen.Models;

using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Formatting = Newtonsoft.Json.Formatting;


namespace Krabbelen.ViewModels
{
	public partial class MainViewModel : BaseViewModel
	{
		const string FILENAME = @"%OneDrive%\Data\Krabbelen.json";

		#region [ Fields ]

		public readonly MainWindow View;

		#endregion

		#region [ Properties ]

		public Krabbel SelectedKrabbel { get; set; }
		public ObservableCollection<Krabbel> Krabbels { get; set; } =
			new ObservableCollection<Krabbel>();
		public ObservableCollection<Controls.KeywordCard> Keywords { get; set; } =
			new ObservableCollection<Controls.KeywordCard>();

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

			LoadFile();
		}

		#endregion

		#region [ Public methods ]


		#endregion

		internal void LoadFile()
		{
			string path = FILENAME.TranslatePath();
			if (File.Exists(path))
			{
				string json = File.ReadAllText(path);
				ObservableCollection<Krabbel> krabbels = 
					JsonConvert.DeserializeObject<ObservableCollection<Krabbel>>(json);
				Krabbels = krabbels;
			}
			else
			{
				Krabbels = new ObservableCollection<Krabbel>();
			}
		}

		internal void Shutdown()
		{
			SaveFile();
			Application.Current.Shutdown();
		}

		internal void SaveFile()
		{
			string json = JsonConvert.SerializeObject(Krabbels, Formatting.Indented);
			try
			{
				using (StreamWriter stream = new StreamWriter(FILENAME.TranslatePath()))
				{
					stream.Write(json);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving file: {ex.Message}",
					"Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		internal void NewKrabbel()
		{
			SelectedKrabbel = new Krabbel();
			OpenKrabbel(SelectedKrabbel);
		}

		internal void OpenKrabbel(Krabbel selectedKrabbel)
		{
			SelectedKrabbel = selectedKrabbel;
			KrabbelViewModel view = new KrabbelViewModel(this);
			CopyKeywords();
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

		/// <summary>
		/// Removes the specified keyword from the selected note and from the Keywords collection.
		/// </summary>
		/// <remarks>
		/// If SelectedNote or its Keywords collection is null, no action is taken. 
		/// Removes the first entry in Keywords whose KeywordText.Text equals the provided keyword;
		/// if no match is found, no change occurs.
		/// </remarks>
		/// <param name="keyword">The keyword text to remove.</param>
		public void RemoveKeyword(string keyword)
		{
			if (SelectedKrabbel != null && SelectedKrabbel.Keywords != null)
			{
				//Delete the keyword from the note and from the ObservableCollection of KeywordCards.
				SelectedKrabbel.Keywords.Remove(keyword);
				Keywords.Remove(Keywords.FirstOrDefault(x => x.KeywordText.Text == keyword));
			}
		}

		/// <summary>
		/// Make a copy of the keywords into the ObservableCollection of KeywordCards.
		/// </summary>
		private void CopyKeywords()
		{
			if (SelectedKrabbel == null) return;

			Keywords.Clear();
			if (SelectedKrabbel.Keywords != null)
			{
				foreach (string keyword in SelectedKrabbel.Keywords)
				{
					Keywords.Add(new Controls.KeywordCard() { KeywordText = { Text = keyword } });
				}
			}
		}

		internal void CreateNewKeyword()
		{
			string newKeyword;
			QuestionBoxViewModel question = new QuestionBoxViewModel(this);
			newKeyword = question.Show("Keyword:", "New Keyword", SelectedKrabbel?.Keywords);

			if (string.IsNullOrWhiteSpace(newKeyword))
			{
				return;
			}
			SelectedKrabbel?.Keywords.Add(newKeyword);
			CopyKeywords();
		}

	}
}
