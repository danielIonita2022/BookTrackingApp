using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.ViewModels
{
    public class BookViewModel
    {
		public string? Title { get; set; }
		public string? Author { get; set; }
		public double? PublicRating { get; set; }
		public string? Publisher { get; set; }
		public int? PageCount { get; set; }
		public DateTime? PublicationDate { get; set; }
		public string? Category { get; set; }
		public int? MyRating { get; set; }
		public DateTime? DateFinished { get; set; }
		public string? Notes { get; set; }
	}
}
