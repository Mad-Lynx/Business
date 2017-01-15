using System;

namespace MadLynx.Business.NumberSequences
{
	public sealed class FormatterContext
	{
		public FormatterContext(string format, ulong numberOrigin, string numberText, DateTime dateTime)
		{
			Format = format;
			NumberOrigin = numberOrigin;
			NumberText = numberText;
			DateTime = dateTime;
		}

		public string Format { get; private set; }

		public ulong NumberOrigin { get; private set; }

		public string NumberText { get; private set; }

		public DateTime DateTime { get; private set; }
	}
}
