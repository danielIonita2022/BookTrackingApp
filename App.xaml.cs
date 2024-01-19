using MobileApp.LocalDB;
using MobileApp.ViewModels;

namespace MobileApp
{
	public partial class App : Application
	{
		public App()
		{
			//var mainPage = new MainMenuPage()
			//{
			//	BindingContext = new MainMenuViewModel()
			//};
			//MainPage = mainPage;

			MainPage = new NavigationPage(new MainMenuPage());
		}

		protected override async void OnStart()
		{
			BooksDB db = new BooksDB();
			await db.Init();
			await db.InsertBookData();
			base.OnStart();
		}
	}
}