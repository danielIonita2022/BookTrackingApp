using Microcharts;
using MobileApp.LocalDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
	public class StatisticsViewModel : INotifyPropertyChanged
	{
		private INavigation _navigation;
		private BooksDB _booksDB;

		private SkiaSharp.SKColor[] _barChartColors = new SkiaSharp.SKColor[]
		{
			SkiaSharp.SKColor.Parse("#73B1F4"),
			SkiaSharp.SKColor.Parse("#9FC6F0"),

            SkiaSharp.SKColor.Parse("#9BD66D"),
            SkiaSharp.SKColor.Parse("#C3E589"),
            SkiaSharp.SKColor.Parse("#EDF8C1"),

            SkiaSharp.SKColor.Parse("#FDC667"),
            SkiaSharp.SKColor.Parse("#FFA34D"),
            SkiaSharp.SKColor.Parse("#FF7E47"),

            SkiaSharp.SKColor.Parse("#F7A278"),
            SkiaSharp.SKColor.Parse("#E77C6D"),
            SkiaSharp.SKColor.Parse("#D56A5D"),

            SkiaSharp.SKColor.Parse("#5DADEC"),
        };

        private BarChart _booksReadChart;
		public BarChart BooksReadChart
		{
			get => _booksReadChart;
			set
			{
				_booksReadChart = value;
				OnPropertyChanged(nameof(BooksReadChart));
			}
		}

		private LineChart _pagesReadChart;
		public LineChart PagesReadChart
		{
			get => _pagesReadChart;
			set
			{
                _pagesReadChart = value;
                OnPropertyChanged(nameof(PagesReadChart));
            }
		}


        public ObservableCollection<string> Months { get; set; }
		public ObservableCollection<string> Years { get; set; }


		private string _selectedYear;
		public string SelectedYear
		{
			get => _selectedYear;
			set
			{
				_selectedYear = value;
				OnPropertyChanged(nameof(SelectedYear));
                _ = OnYearChanged();
			}
		}

		private string _favoriteGenreName;
		public string FavoriteGenreName
		{
			get => _favoriteGenreName;
			set
			{
                _favoriteGenreName = value;
                OnPropertyChanged(nameof(FavoriteGenreName));
            }
		}

		private string _favoriteGenreBookCount;
		public string FavoriteGenreBookCount
		{
			get => _favoriteGenreBookCount;
			set
			{
                _favoriteGenreBookCount = value;
                OnPropertyChanged(nameof(FavoriteGenreBookCount));
            }
		}

        public StatisticsViewModel(INavigation navigation, BooksDB booksDB)
		{
			_navigation = navigation;
			_booksDB = booksDB;
            int currentYear = DateTime.Now.Year;
            Years = new ObservableCollection<string>(
                Enumerable.Range(2000, currentYear - 2000 + 1).Select(y => y.ToString()));
        }

		public async Task InitializeAsync()
		{
            int currentYear = DateTime.Now.Year;
            BooksReadChart = new BarChart();
			PagesReadChart = new LineChart();
            SelectedYear = currentYear.ToString();
            await OnYearChanged();
            await OnFavoriteGenreChanged();
        }

		public async Task OnFavoriteGenreChanged()
		{
            Tuple<string, int> FavoriteGenreTuple = await _booksDB.FindFavoriteGenreAndBookCount();
			FavoriteGenreName = FavoriteGenreTuple.Item1;
			FavoriteGenreBookCount = FavoriteGenreTuple.Item2.ToString();
		}

		public async Task OnYearChanged()
		{
			//await _booksDB.Init();
			await UpdateChartsAsync();
        }

		private async Task UpdateChartsAsync()
		{
			Task<int>[] bookCountTasks = new Task<int>[12];
			for (int i = 0; i < 12; i++)
			{
				bookCountTasks[i] = _booksDB.CountNoBooksInMonth((i + 1).ToString(), SelectedYear);
			}

			ChartEntry[] barChartEntries = new ChartEntry[12];
			for (int i = 0; i < 12; i++)
			{
				int booksCount = await bookCountTasks[i];
				barChartEntries[i] = new ChartEntry(booksCount)
				{
					Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1),
					ValueLabel = booksCount.ToString(),
					Color = _barChartColors[i % _barChartColors.Length]
				};
			}

			BooksReadChart = new BarChart()
			{
				Entries = barChartEntries,
				BackgroundColor = SkiaSharp.SKColor.Parse("#FFFFFF"),
				LabelTextSize = 15,
				IsAnimated = true,
				BarAreaAlpha = 150,

			};

			Task<int>[] pageCountTasks = new Task<int>[12];
			for (int i = 0; i < 12; i++)
			{
				pageCountTasks[i] = _booksDB.CountNoPagesInMonth((i + 1).ToString(), SelectedYear);
			}

			ChartEntry[] lineChartEntries = new ChartEntry[12];
			for (int i = 0; i < 12; i++)
			{
				int pageCount = await pageCountTasks[i];
				lineChartEntries[i] = new(pageCount)
				{
					Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1),
					ValueLabel = pageCount.ToString(),
					Color = _barChartColors[i % _barChartColors.Length]
				};
			}
			PagesReadChart = new LineChart()
			{
				Entries = lineChartEntries,
				BackgroundColor = SkiaSharp.SKColor.Parse("#FFFFFF"),
				LabelTextSize = 15,
				IsAnimated = true,
				LineMode = LineMode.Spline,
				LineSize = 8,
				LineAreaAlpha = 150,
			};


			OnPropertyChanged(nameof(BooksReadChart));
			OnPropertyChanged(nameof(PagesReadChart));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
