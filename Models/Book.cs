using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColumnAttribute = SQLite.ColumnAttribute;
using TableAttribute = SQLite.TableAttribute;

namespace MobileApp.Models
{
	[Table("Book")]
	public class Book
	{
		[PrimaryKey, AutoIncrement, Column("BookID")]
		public int BookID { get; set; }
		public string? Title { get; set; }
		public string? Author { get; set; }
		public double? PublicRating { get; set; }
		public string? Publisher { get; set; }
		public int? PageCount { get; set; }
		public DateTime? PublicationDate { get; set; }
		public string? Category { get; set; }
	}
}
