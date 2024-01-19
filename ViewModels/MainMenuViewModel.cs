using MobileApp.LocalDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileApp.ViewModels
{
	public class MainMenuViewModel : INotifyPropertyChanged
	{
		private INavigation _navigation;
		private BooksDB _booksDB;
		public ICommand MyReadBooksCommand { get; set; }
		public ICommand CurrentlyReadingCommand { get; set; }
		public ICommand StatisticsCommand { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public MainMenuViewModel(INavigation navigation)
		{
			_navigation = navigation;
			_booksDB = new BooksDB();
			MyReadBooksCommand = new Command(OnMyReadBooks);
			CurrentlyReadingCommand = new Command(OnCurrentlyReading);
			StatisticsCommand = new Command(OnStatistics);
		}

		private async void OnMyReadBooks()
		{
			await _navigation.PushAsync(new MyReadBooksPage(_navigation, _booksDB));
		}
		private async void OnCurrentlyReading() 
		{
			await _navigation.PushAsync(new CurrentlyReadingPage(_navigation, _booksDB));
		}
		private async void OnStatistics() 
		{
			await _navigation.PushAsync(new StatisticsPage(_navigation, _booksDB));
		}
	}
}
