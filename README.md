# Krabbelen

A small application to store small memories. The application uses the `MVVM architectural pattern`.

A krabbel can have `keywords` and the `ObservableCollection<string>` was show as `(Collection)` in the `DataGrid`.

I used public method in the class to display the `Keyword` properly:

```csharp
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
````

The data is stored in `Json` file.