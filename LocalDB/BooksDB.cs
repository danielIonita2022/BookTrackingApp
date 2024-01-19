using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Constants;
using MobileApp.Models;
using System.Reflection;
using CsvHelper;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;

namespace MobileApp.LocalDB
{
	public class BooksDB
	{
		SQLiteAsyncConnection Database;

		public BooksDB() 
		{

		}
		public async Task InsertBookData()
		{
			var bookCount = await Database.Table<Book>().CountAsync();
			if (bookCount > 0)
			{
				return;
			}
			var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Book)).Assembly;
			Stream stream = assembly.GetManifestResourceStream("MobileApp.Resources.Data.book_data.csv");
			using (var reader = new StreamReader(stream))
			{
				using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					var records = csv.GetRecords<Book>();
					foreach (var book in records)
					{
						await Database.InsertAsync(book);
					}
				}
			}
		}

		public async Task Init()
		{
			if (Database is not null)
			{
				return;
			}
			Database = new SQLiteAsyncConnection(Constants.Constants.DatabasePath, Constants.Constants.Flags);
			await Database.CreateTableAsync<Book>();
			await Database.CreateTableAsync<ReadBook>();
			await Database.CreateTableAsync<CurrentlyReading>();
		}

		public async Task<List<Book>> GetBooksAsync()
		{
			await Init();
			return await Database.Table<Book>().ToListAsync();
		}
		public async Task<Book> GetBookAsync(int id)
		{
			await Init();
			return await Database.Table<Book>().Where(b => b.BookID == id).FirstOrDefaultAsync();
		}
		public async Task<Book> GetBookAsync(string bookName)
		{
			await Init();
			return await Database.Table<Book>().Where(b => b.Title == bookName).FirstOrDefaultAsync();
		}
		public async Task<List<Book>> GetSearchResults(string query)
		{
			await Init();
			return await Database.Table<Book>().Where(b => b.Title.Contains(query) || b.Author.Contains(query) || b.Category.Contains(query)).ToListAsync();
		}
		public async Task<List<ReadBook>> GetReadBooksAsync()
		{
			await Init();
			return await Database.Table<ReadBook>().ToListAsync();
		}
		public async Task<int> SaveReadBookAsync(ReadBook item)
		{
			await Init();
			if (item.ReadBookID != 0)
				return await Database.UpdateAsync(item);
			else
				return await Database.InsertAsync(item);
		}
		public async Task<int> DeleteReadBookAsync(ReadBook item)
		{
			await Init();
			return await Database.DeleteAsync(item);
		}
		public async Task<CurrentlyReading> GetCurrentlyReadingBookAsync(int bookId)
		{
			await Init();
			return await Database.Table<CurrentlyReading>().Where(b => b.BookId == bookId).FirstOrDefaultAsync();
		}
		public async Task<List<CurrentlyReading>> GetCurrentlyReadingBooksAsync()
		{
			await Init();
			return await Database.Table<CurrentlyReading>().ToListAsync();
		}
		public async Task<int> UpdateCurrentlyReadingAsync(CurrentlyReading item)
		{
			await Init();
			return await Database.UpdateAsync(item);
		}
		public async Task<int> SaveCurrentlyReadingBookAsync(CurrentlyReading item)
		{
			await Init();
			if (item.CurrentlyReadingId != 0)
				return await Database.UpdateAsync(item);
			else
				return await Database.InsertAsync(item);
		}
		public async Task<int> DeleteCurrentlyReadingBookAsync(CurrentlyReading item)
		{
			await Init();
			return await Database.DeleteAsync(item);
		}

		public async Task<int> CountNoBooksInMonth(string month, string year)
		{
			await Init();

			if (!DateTime.TryParseExact($"{month} {year}", "M yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
			{
				throw new ArgumentException("Invalid month and year");
			}

			DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
			DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
			return await Database.Table<ReadBook>()
						 .Where(book => book.DateFinished >= firstDayOfMonth && book.DateFinished < firstDayOfNextMonth)
						 .CountAsync();
		}

		public async Task<int> CountNoPagesInMonth(string month, string year)
		{
			await Init();

			if (!DateTime.TryParseExact($"{month} {year}", "M yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
			{
                throw new ArgumentException("Invalid month and year");
            }

			DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
			DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
			List<ReadBook> readBooks = await Database.Table<ReadBook>()
                         .Where(book => book.DateFinished >= firstDayOfMonth && book.DateFinished < firstDayOfNextMonth).ToListAsync();


            var readBookIds = readBooks.Select(rb => rb.BookID).ToList();

            List<Book> books = await Database.Table<Book>()
                                  .Where(book => readBookIds.Contains(book.BookID))
                                  .ToListAsync();

            int pagesRead = 0;
			foreach (Book book in books)
			{
                pagesRead += (int)book.PageCount;
            }
			return pagesRead;
		}

		public async Task<int> CountNoBooksInYear(string year)
		{
			await Init();

			if (!DateTime.TryParseExact(year, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
			{
				throw new ArgumentException("Invalid year");
			}

			DateTime firstDayOfYear = new DateTime(date.Year, 1, 1);
			DateTime firstDayOfNextYear = firstDayOfYear.AddYears(1);
			return await Database.Table<ReadBook>()
						 .Where(book => book.DateFinished >= firstDayOfYear && book.DateFinished < firstDayOfNextYear)
						 .CountAsync();
		}

		public async Task<Tuple<string, int>> FindFavoriteGenreAndBookCount()
		{
            await Init();
            List<ReadBook> readBooks = await Database.Table<ReadBook>().ToListAsync();
            var readBookIds = readBooks.Select(rb => rb.BookID).ToList();
            List<Book> books = await Database.Table<Book>()
                                  .Where(book => readBookIds.Contains(book.BookID))
                                  .ToListAsync();
            var genres = books.Select(b => b.Category).ToList();
            var favoriteGenre = genres.GroupBy(g => g).OrderByDescending(g => g.Count()).First().Key;
			var favoriteGenreCount = genres.GroupBy(g => g).OrderByDescending(g => g.Count()).First().Count();
            return new Tuple<string, int>(favoriteGenre, favoriteGenreCount);
        }

	}
}
