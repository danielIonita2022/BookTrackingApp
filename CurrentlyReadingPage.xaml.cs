using MobileApp.LocalDB;
using MobileApp.ViewModels;

namespace MobileApp;

public partial class CurrentlyReadingPage : ContentPage
{
	private CurrentlyReadingViewModel _vm;
	private Timer _debounceTimer;
	public CurrentlyReadingPage(INavigation navigation, BooksDB db)
	{
		InitializeComponent();
		_vm = new CurrentlyReadingViewModel(navigation, db);
		BindingContext = _vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CurrentlyReadingViewModel viewModel)
        {
            viewModel.InitializeAsync();
        }
    }
    private void OnCurrentPageTextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender is Entry entry && entry.BindingContext is ReadingBookViewModel bookVM)
		{
			// Reset the timer every time the text changes
			_debounceTimer?.Dispose();
			_debounceTimer = new Timer(_ =>
			{
				// Use Dispatcher to run the update on the UI thread
				entry.Dispatcher.Dispatch(() =>
				{
					if (int.TryParse(entry.Text, out int newPage) && newPage >= 0 && newPage <= bookVM.PageCount && newPage != bookVM.CurrentPage)
					{
						List<object> commandParameter = new List<object> { bookVM, newPage.ToString() };
						if (_vm.ChangeBookPageCommand.CanExecute(commandParameter))
						{
							_vm.ChangeBookPageCommand.Execute(commandParameter);
						}
					}
					else
					{
						// Reset to current page if invalid
						entry.Text = bookVM.CurrentPage.ToString();
					}
				});
			}, null, 1400, Timeout.Infinite); // 1400 milliseconds delay, no subsequent calls
		}
	}

}