using MobileApp.Enums;
using MobileApp.LocalDB;
using MobileApp.Models;
using MobileApp.ViewModels;

namespace MobileApp;

public partial class AddBookPage : ContentPage
{
	public AddBookPage(INavigation navigation, BooksDB db, BookType bookType)
	{
		InitializeComponent();
		BindingContext = new AddBookViewModel(navigation, db, bookType);
	}

	private void OnSortOptionChanged(object sender, EventArgs e)
	{
		var selectedOption = SortOptionPicker.SelectedItem as string;
		var books = SearchResults.ItemsSource;
		var bookList = new List<Book>(books as IEnumerable<Book>);

		switch (selectedOption)
		{
			case "Title (A-Z)":
				bookList = bookList.OrderBy(b => b.Title).ToList();
				break;
			case "Author (A-Z)":
				bookList = bookList.OrderBy(b => b.Author).ToList();
				break;
			default:
				// Leave default ordering or apply any other logic
				break;
		}

		SearchResults.ItemsSource = bookList;
	}
}