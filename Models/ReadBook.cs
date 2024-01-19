using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableAttribute = SQLite.TableAttribute;

namespace MobileApp.Models
{
	[Table("ReadBook")]
	public class ReadBook
	{
		[PrimaryKey, AutoIncrement]
		public int ReadBookID { get; set; }
		[ForeignKey("BookId")]
		public int BookID { get; set; }
		public int? MyRating { get; set; }
		public DateTime? DateFinished { get; set; }
		public string? Notes { get; set; }
	}
}
