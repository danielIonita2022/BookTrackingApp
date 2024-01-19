using Microcharts;
using MobileApp.LocalDB;
using MobileApp.ViewModels;
using SkiaSharp;

namespace MobileApp;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage(INavigation navigation, BooksDB booksDB)
	{
		InitializeComponent();
		BindingContext = new StatisticsViewModel(navigation, booksDB); ;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StatisticsViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}