using MobileApp.ViewModels;

namespace MobileApp;

public partial class BookDetailsPage : ContentPage
{
	public BookDetailsPage()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		(sender as Button).BackgroundColor = Colors.BlueViolet;
	}
}