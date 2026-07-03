using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Krabbelen.Models
{
	public class Krabbel : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Text 
		{
			get;
			set
			{
				if (field != value)
				{
					field = value;
					OnPropertyChanged();
				}
			} 
		}
		public ObservableCollection<string> Keywords { get; set; } = new ObservableCollection<string>();
		public DateTime Created { get; set; }
		public DateTime Changed { get; set; }

		public string DisplayKeywords
		{
			get
			{
				if (Keywords == null || Keywords.Count == 0)
					return string.Empty;
				return string.Join(", ", Keywords);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
			Changed = DateTime.Now;
			//HasChanged = true;
		}

	}

}
