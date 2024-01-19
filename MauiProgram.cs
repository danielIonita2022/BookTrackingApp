using Microsoft.Extensions.Logging;
using Microcharts.Maui;
using MobileApp.LocalDB;
using SkiaSharp.Views.Maui.Controls.Hosting;
namespace MobileApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMicrocharts()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddSingleton<BooksDB>();


#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}