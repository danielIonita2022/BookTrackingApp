using MobileApp.LocalDB;
using MobileApp.ViewModels;
using MobileApp.Enums;

namespace MobileApp
{
	public partial class MyReadBooksPage : ContentPage
	{
		public MyReadBooksPage(INavigation navigation, BooksDB booksDB)
		{
			InitializeComponent();
			var viewModel = new MyReadBooksViewModel(navigation, booksDB);
			BindingContext = viewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MyReadBooksViewModel viewModel)
            {
                viewModel.InitializeAsync();
            }
        }
    }
}