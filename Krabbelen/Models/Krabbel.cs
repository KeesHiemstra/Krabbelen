using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
		public DateTime Changed { get; set; }

		[JsonIgnore]
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
		public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}

}
