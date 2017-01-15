using System;
using MadLynx.Business.NumberSequences;

namespace MadLynx.Business.Tests.NumberSequences
{
	public class TestStringFormatFormatter : INumberSequenceFormatter
	{
		/// <summary>
		/// Generate number with specific format
		/// </summary>
		public string Format(string format, ulong num, DateTime date)
		{
			return String.Format(format, num, date);
		}
	}
}
