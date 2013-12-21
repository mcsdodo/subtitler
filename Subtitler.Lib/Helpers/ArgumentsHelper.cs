using System.Collections.Generic;

namespace Subtitler.Lib.Helpers
{
	public static class ArgumentsHelper
	{
		/// <summary>
		/// returns arg entry from application arguments
		/// </summary>
		public static string ParseArgumentsForKey(string[] args, string key)
		{
			var arguments = new Dictionary<string, string>();

			if (args.Length > 1 && args.Length % 2 == 1)
			{
				for (int i = 1; i < args.Length; i += 2)
				{
					arguments.Add(args[i].Substring(1), args[i + 1]);
				}
			}

			if (arguments.ContainsKey(key))
			{
				return arguments[key];
			}

			return "";
		}

		public static string ParseFirstArgument(string[] args)
		{
			if (args.Length > 1)
			{
				return args[1];
			}
			return "";
		}
	}
}
