using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Converters
{
	public class RatingToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int rating && parameter is string ratingValueString && int.TryParse(ratingValueString, out int ratingValue))
			{
				return rating >= ratingValue ? Colors.Gold : Colors.Gray;
			}
			return Colors.Gray; // default color
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}