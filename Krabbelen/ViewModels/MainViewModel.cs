using CHi.Extensions;

using Krabbelen.Models;
using Krabbelen.Views;

using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		#region [ Fields ]

		const string FILENAME = @"%OneDrive%\Data\Krabbelen.json";
		public bool AskClosing = true;
		public bool DeleteOpenKrabbel = false;
		public readonly MainWindow View;

#endregion

		#region [ Properties ]

		public Krabbel SelectedKrabbel { get; set; }
		public ObservableCollection<Krabbel> Krabbels { get; set; } =
			new ObservableCollection<Krabbel>();
		public ObservableCollection<Controls.KeywordCard> Keywords { get; set; } =
			new ObservableCollection<Controls.KeywordCard>();

		// This property is used to track if any changes have been made to the Krabbels
		// collection or any of its items. The OnPropertyChanged event handler for the
		// Krabbels collection and its items will set this property to true when a change occurs.
		public bool KrabbelsChanged 
		{ 
			get; 
			set
			{
				if (value != field)
				{
					field = value;
					OnPropertyChanged();
				}
			}
		} = false;

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

		/// <summary>
		/// Loads the Krabbels from the JSON file specified by FILENAME. 
		/// If the file exists, it reads the content, deserializes it into an ObservableCollection of 
		/// Krabbel objects, and assigns it to the Krabbels property. 
		/// If the file does not exist, it initializes Krabbels as an empty collection.
		/// </summary>
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

		internal void WindowClosing(object sender, CancelEventArgs e)
		{
			// If there are no unsaved changes, we can exit without prompting the user.
			if (!KrabbelsChanged) { return; }

			if (AskClosing)
			{
				MessageBoxResult result = MessageBox.Show(
					"Do you want to save your changes before closing?",
					"Krabbelen",
					MessageBoxButton.YesNoCancel,
					MessageBoxImage.Question);
				switch (result)
				{
					case MessageBoxResult.Yes:
						SaveFile();
						break;
					case MessageBoxResult.No:
						break;
					case MessageBoxResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
		}

		/// <summary>
		/// Saves the current state of the application and shuts down the application.
		/// </summary>
		internal void Shutdown()
		{
			AskClosing = false;
			SaveFile();
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Saves the current collection of Krabbels to a JSON file specified by FILENAME.
		/// </summary>
		internal void SaveFile()
		{
			string json = JsonConvert.SerializeObject(Krabbels.OrderByDescending(k => k.Changed), Formatting.Indented);
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
			bool result = view.Show(SelectedKrabbel);
			KrabbelsChanged = KrabbelsChanged || result;
		}

		public void SaveKrabbel()
		{
			if (SelectedKrabbel.Id == 0)
			{
				Krabbels.Insert(0, SelectedKrabbel);
				SelectedKrabbel.Id = Krabbels.Count;
			}
			SelectedKrabbel.Changed = DateTime.Now;
			KrabbelsChanged = true;
		}

		/// <summary>
		/// Open the selected Krabbel in a new window for editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender == null) { return; }

			// Only one selected item is allowed.
			if (((DataGrid)e.Source).SelectedItems.Count != 1) { return; }
			
			Krabbel selectedItem = (Krabbel)((DataGrid)e.Source).SelectedItem;
			if (selectedItem.Id != 0)
			{
				OpenKrabbel(selectedItem);
			}

			// Deleting the current krabbel was only possible that the sequence was closed.
			if (DeleteOpenKrabbel)
			{
				Krabbels.Remove(SelectedKrabbel);
				SelectedKrabbel = null;
				DeleteOpenKrabbel = false;
				KrabbelsChanged = true;
				return;
			}

			View.MainDataGrid.ItemsSource = null;
			View.MainDataGrid.ItemsSource = Krabbels;
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
				KrabbelsChanged = true;
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
					// After 3 times, an exception is thrown, but can be dismissed.
					try
					{
						Keywords.Add(new Controls.KeywordCard() { KeywordText = { Text = keyword } });

					}
					catch {	}				
				}
			}
		}

		/// <summary>
		/// Adds a new keyword to the selected krabbel and updates the Keywords collection.
		/// </summary>
		internal void CreateNewKeyword()
		{
			string newKeyword;
			QuestionBoxViewModel question = new QuestionBoxViewModel(this);
			newKeyword = question.Show("Keyword:", "New Keyword", SelectedKrabbel?.Keywords);
			newKeyword = newKeyword?.Trim();

			if (string.IsNullOrWhiteSpace(newKeyword)) { return; }

			SelectedKrabbel?.Keywords.Add(newKeyword);
			KrabbelsChanged = true;
			CopyKeywords();
		}

		/// <summary>
		/// Deletes the currently selected Krabbel from the collection of Krabbels.
		/// </summary>
		internal void DeleteKrabbel(object sender)
		{
			if (SelectedKrabbel != null)
			{
				DeleteOpenKrabbel = true;
				((KrabbelWindow)sender).Close();
				KrabbelsChanged = true;
			}
		}

		public void ShowHistory()
		{

			_ = new HistoryWindow()
			{
				Left = View.Left + 20,
				Top = View.Top + 20
			}.ShowDialog();

		}

	}
}
