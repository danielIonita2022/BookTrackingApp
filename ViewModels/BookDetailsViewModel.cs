using MobileApp.Enums;
using MobileApp.LocalDB;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileApp.ViewModels
{
	public class BookDetailsViewModel : INotifyPropertyChanged
	{
		private INavigation _navigation;
		private Book _selectedBook;
		private BooksDB _db;
		private BookType _bookType;

		private int _myRating;
		public int MyRating
		{
			get => _myRating;
			set
			{
				_myRating = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyRating)));
			}
		}
		public DateTime DateFinished { get; set; } = DateTime.Now;
		public string Notes { get; set; }
		public ICommand SaveBookCommand { get; set; }
		public ICommand SetRatingCommand { get; set; }

		public BookDetailsViewModel(INavigation navigation, BooksDB dB, Book selectedBook, BookType bookType)
		{
			_navigation = navigation;
			_selectedBook = selectedBook;
			_db = dB;
			_bookType = bookType;
			SaveBookCommand = new Command(OnSaveBook);
			SetRatingCommand = new Command<string>(SetRating);
		}

		private void SetRating(string param)
		{
			int rating = int.Parse(param);
			MyRating = rating;
		}

		private async void OnSaveBook()
		{
			ReadBook readBook = new ReadBook();
			readBook.BookID = _selectedBook.BookID;
			readBook.MyRating = MyRating;
			readBook.DateFinished = DateFinished;
			readBook.Notes = Notes;
			await _db.SaveReadBookAsync(readBook);
			if (_bookType == BookType.CurrentlyReading)
			{
				CurrentlyReading currentlyReadingBook = await _db.GetCurrentlyReadingBookAsync(_selectedBook.BookID);
				await _db.DeleteCurrentlyReadingBookAsync(currentlyReadingBook);
			}
			await _navigation.PopAsync();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
