using MobileApp.LocalDB;
using MobileApp.Models;
using MobileApp.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileApp.ViewModels
{
	public class AddBookViewModel : INotifyPropertyChanged
	{
		private INavigation _navigation;
		private BooksDB _db;
		private BookType _bookType;
		public ObservableCollection<Book> Books { get; set; }
		public bool NoResultsFound { get; set; } = false;
		public string Title { get; set; }
		public string Author { get; set; }
		public Book SelectedBook { get; set; }
		public ICommand SearchCommand { get; }
		public ICommand AddBookCommand { get; }

		public AddBookViewModel(INavigation navigation, BooksDB db, BookType bookType)
		{
			SearchCommand = new Command<string>(SearchForBooks);
			AddBookCommand = new Command(AddBook);
			Books = new ObservableCollection<Book>();
			_navigation = navigation;
			_db = db;
			_bookType = bookType;
		}

		private async void SearchForBooks(string query)
		{
			if (query != "")
			{
				string uppercaseFirstLetter = char.ToUpper(query[0]).ToString();
				string uppercaseQuery = uppercaseFirstLetter + query[1..];
				Books = new ObservableCollection<Book>(await _db.GetSearchResults(uppercaseQuery));
			}
			if (Books.Count == 0)
			{
				NoResultsFound = true;
			}
			else
			{
				NoResultsFound = false;
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Books)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoResultsFound)));
		}

		private async void AddBook()
		{
			if (SelectedBook != null)
			{
				if (_bookType == BookType.Read)
				{
					var bookDetailsVM = new BookDetailsViewModel(_navigation, _db, SelectedBook, _bookType);
					var bookDetailsPage = new BookDetailsPage
					{
						BindingContext = bookDetailsVM
					};
					await _navigation.PushAsync(bookDetailsPage);
				}
				else if (_bookType == BookType.CurrentlyReading)
				{
					CurrentlyReading currentlyReadingBook = new CurrentlyReading
					{
						BookId = SelectedBook.BookID,
						CurrentPage = 0
					};
					await _db.SaveCurrentlyReadingBookAsync(currentlyReadingBook);
					await Application.Current.MainPage.DisplayAlert("Success", "Book added successfully!", "OK");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
