using System.Collections.Generic;

namespace Subtitler.Lib.Helpers
{
	public static class ArgumentsHelper
	{
		public static string ParseArguments(string[] args, string arg)
		{
			var arguments = new Dictionary<string, string>();

			if (args.Length > 1 && args.Length % 2 == 1)
			{
				for (int i = 1; i < args.Length; i += 2)
				{
					arguments.Add(args[i].Substring(1), args[i + 1]);
				}
			}

			if (arguments.ContainsKey(arg))
			{
				return arguments[arg];
			}

			return "";
		}
	}
}
