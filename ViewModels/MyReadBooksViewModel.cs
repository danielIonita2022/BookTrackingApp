using MobileApp.Enums;
using MobileApp.LocalDB;
using MobileApp.Models;
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
	public class MyReadBooksViewModel : INotifyPropertyChanged
	{
		private BooksDB _db;
		private INavigation _navigation;
		private List<ReadBook> _books;
		private ICommand _addButtonPressedCommand;
		public ICommand AddButtonPressedCommand
		{
			get
			{
				if (_addButtonPressedCommand == null)
				{
					_addButtonPressedCommand = new Command(OnAddButtonPressed);
				}
				return _addButtonPressedCommand;
			}
			set 
			{ 
				_addButtonPressedCommand = value; 
			}
		}
		private ICommand _deleteBookCommand;
		public ICommand DeleteBookCommand
		{
			get
			{
				if (_deleteBookCommand == null)
				{
					_deleteBookCommand = new Command<BookViewModel>(OnDeleteBookAsync);
				}
				return _deleteBookCommand;
			}
			set
			{
				_deleteBookCommand = value;
			}
		}
		public ObservableCollection<BookViewModel> Books { get; set; }

		public MyReadBooksViewModel(INavigation navigation, BooksDB db)
		{
			_navigation = navigation;
			_db = db;
		}

		public async void InitializeAsync()
		{
			 _books = await _db.GetReadBooksAsync();
			Books = new ObservableCollection<BookViewModel>();
			foreach (ReadBook readBook in _books)
			{
				Book book = await _db.GetBookAsync(readBook.BookID);
				Books.Add(new BookViewModel
				{
					Title = book.Title,
					Author = book.Author,
					PublicRating = book.PublicRating,
					Publisher = book.Publisher,
					PageCount = book.PageCount,
					PublicationDate = book.PublicationDate,
					Category = book.Category,
					MyRating = readBook.MyRating,
					DateFinished = readBook.DateFinished,
					Notes = readBook.Notes
				});
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Books)));
		}

		private async void OnDeleteBookAsync(object param)
		{
			BookViewModel bookViewModel = (BookViewModel)param;
			ReadBook readBook = _books[Books.IndexOf(bookViewModel)];
			Books.Remove(bookViewModel);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Books)));
			await _db.DeleteReadBookAsync(readBook);
		}

		private void OnAddButtonPressed()
		{
			_navigation.PushAsync(new AddBookPage(_navigation, _db, BookType.Read));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Books)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
