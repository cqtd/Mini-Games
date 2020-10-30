using System;

namespace Service.TCP
{
	public class LogManager
	{
		public static void Verbose(string msg)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(msg);
			Console.ForegroundColor = color;
		}

		public static void Success(string msg)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(msg);
			Console.ForegroundColor = color;
		}

		public static void Warn(string msg)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(msg);
			Console.ForegroundColor = color;
		}

		public static void Critical(string msg)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ForegroundColor = color;
		}
	}
}