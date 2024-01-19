using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
	public class ReadingBookViewModel : INotifyPropertyChanged
	{
		public string? Title { get; set; }
		public string? Author { get; set; }
		public double? PublicRating { get; set; }
		public string? Publisher { get; set; }
		public int? PageCount { get; set; }
		public DateTime? PublicationDate { get; set; }
		public string? Category { get; set; }

		private int _currentPage;
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				CurrentPageText = $"{_currentPage}";
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProgress)));
			}
		}
		private string _currentPageText;
		public string CurrentPageText
		{
			get => _currentPageText;
			set
			{
				_currentPageText = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPageText)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProgress)));
			}
		}
		public double? CurrentProgress
		{
			get
			{
				if (PageCount <= 0)
				{
					return 0;
				}
				else
				{
					return CurrentPage / (double)PageCount;
				}

			}
		}
		public string CurrentPageTotalPageText
		{
			get
			{
				return $"{CurrentPage}/{PageCount}";
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
