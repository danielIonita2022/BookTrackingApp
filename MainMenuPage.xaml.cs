using MobileApp.ViewModels;

namespace MobileApp;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
		InitializeComponent();
		BindingContext = new MainMenuViewModel(Navigation);
	}
}