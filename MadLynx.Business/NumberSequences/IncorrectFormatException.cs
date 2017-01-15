using System;

namespace MadLynx.Business.NumberSequences
{
	public class IncorrectFormatException : Exception
	{
		public IncorrectFormatException(string format, string num, string message)
			: base(message)
		{
			Format = format;
			Num = num;
		}

		public string Format { get; private set; }

		public string Num { get; private set; }
	}
}
