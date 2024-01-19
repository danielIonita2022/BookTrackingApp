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
    public class CurrentlyReadingViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private BooksDB _booksDB;

		private List<CurrentlyReading> _currentlyReadingBooks;
		public ObservableCollection<ReadingBookViewModel> CurrentlyReadingBooks { get; set; } = new ObservableCollection<ReadingBookViewModel>();

		public ICommand FinishBookCommand { get; set; }
		public ICommand AddBookCommand { get; set; }
		public ICommand ChangeBookPageCommand { get; set; }


		private async void OnFinishedReading(ReadingBookViewModel book)
		{
			CurrentlyReadingBooks.Remove(book);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentlyReadingBooks)));
			Book bookModel = await _booksDB.GetBookAsync(book.Title);
			BookDetailsViewModel bookDetailsViewModel = new BookDetailsViewModel(_navigation, _booksDB, bookModel, BookType.CurrentlyReading);
			await _navigation.PushAsync(new BookDetailsPage
			{
				BindingContext = bookDetailsViewModel
			});
		}

		private async void OnAddBook()
		{
			await _navigation.PushAsync(new AddBookPage(_navigation, _booksDB, BookType.CurrentlyReading));
		}

		private async void OnChangeBookPage(object objects)
		{
			ReadingBookViewModel bookVM = (ReadingBookViewModel)((List<object>)objects)[0];
			string currentPageString = (string)((List<object>)objects)[1];
			if (int.TryParse(currentPageString, out int newPage))
			{
				if (newPage >= 0 && newPage <= bookVM.PageCount)
				{
					bookVM.CurrentPage = newPage;
					Book book = await _booksDB.GetBookAsync(bookVM.Title);
					CurrentlyReading currentlyReading = _currentlyReadingBooks.Where(cr => cr.BookId == book.BookID).FirstOrDefault();
					currentlyReading.CurrentPage = newPage;
					await _booksDB.UpdateCurrentlyReadingAsync(currentlyReading);
				}
				if (newPage == bookVM.PageCount)
				{
					OnFinishedReading(bookVM);
				}
			}
		}


		public CurrentlyReadingViewModel(INavigation navigation, BooksDB db)
        {
			_navigation = navigation;
			_booksDB = db;
			FinishBookCommand = new Command<ReadingBookViewModel>(OnFinishedReading);
			AddBookCommand = new Command(OnAddBook);
			ChangeBookPageCommand = new Command(OnChangeBookPage);
		}

		public async void InitializeAsync()
		{
			_currentlyReadingBooks = await _booksDB.GetCurrentlyReadingBooksAsync();
			CurrentlyReadingBooks = new ObservableCollection<ReadingBookViewModel>();
			foreach (CurrentlyReading currentlyReading in _currentlyReadingBooks)
			{
				Book book = await _booksDB.GetBookAsync(currentlyReading.BookId);
				CurrentlyReadingBooks.Add(new ReadingBookViewModel
				{
					Title = book.Title,
					Author = book.Author,
					PublicRating = book.PublicRating,
					Publisher = book.Publisher,
					PageCount = book.PageCount,
					PublicationDate = book.PublicationDate,
					Category = book.Category,
					CurrentPage = (int)currentlyReading.CurrentPage
				});
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentlyReadingBooks)));
		}	

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
