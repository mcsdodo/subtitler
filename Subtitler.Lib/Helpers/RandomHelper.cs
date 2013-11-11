using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Subtitler.Lib.Helpers
{
	public static class RandomHelper
	{
		private static readonly Random RandomSeed = new Random();

		/// <summary>
		/// Generates a random string with the given length
		/// </summary>
		/// <param name="size">Size of the string</param>
		/// <param name="lowerCase">If true, generate lowercase string</param>
		/// <returns>Random string</returns>
		public static string RandomString(int size, bool lowerCase)
		{
			// StringBuilder is faster than using strings (+=)
			var randStr = new StringBuilder(size);

			// Ascii start position (65 = A / 97 = a)
			var start = (lowerCase) ? 97 : 65;

			// Add random chars
			for (var i = 0; i < size; i++)
				randStr.Append((char)(26 * RandomSeed.NextDouble() + start));

			return randStr.ToString();
		}

		public static int RandomInt(int min, int max)
		{
			return RandomSeed.Next(min, max);
		}

		public static double RandomDouble()
		{
			return RandomSeed.NextDouble();
		}

		public static double RandomNumber(int min, int max, int digits)
		{
			return Math.Round(RandomSeed.Next(min, max - 1) + RandomSeed.NextDouble(), digits);
		}

		public static bool RandomBool()
		{
			return (RandomSeed.NextDouble() > 0.5);
		}

		public static DateTime RandomDate()
		{
			return RandomDate(new DateTime(1900, 1, 1), DateTime.Now);
		}

		public static DateTime RandomDate(DateTime from, DateTime to)
		{
			var range = new TimeSpan(to.Ticks - from.Ticks);
			return from + new TimeSpan((long)(range.Ticks * RandomSeed.NextDouble()));
		}

		public static Color RandomColor()
		{
			return Color.FromRgb((byte)RandomSeed.Next(255), (byte)RandomSeed.Next(255), (byte)RandomSeed.Next(255));
		}

		/// <summary>
		/// Returns random item from collection based on Guid.NewGuid
		/// </summary>
		public static T RandItem<T>(this List<T> list)
		{
			//int index = (new Random()).Next(list.Count);
			//return list[index];

			return list.OrderBy(s => Guid.NewGuid()).First();
		}
	}
}
