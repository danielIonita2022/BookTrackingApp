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
	[Table("CurrentlyReading")]
	public class CurrentlyReading
	{
		[PrimaryKey, AutoIncrement]
		public int CurrentlyReadingId { get; set; }
		[ForeignKey("BookId")]
		public int BookId { get; set; }
		public int? CurrentPage { get; set; }
	}
}
