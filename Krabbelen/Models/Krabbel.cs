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
		public DateTime Created { get; private set; } = DateTime.Now;
		public DateTime Changed { get; set; }
		public bool HasChanged { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
			Changed = DateTime.Now;
			HasChanged = true;
		}

	}

}
