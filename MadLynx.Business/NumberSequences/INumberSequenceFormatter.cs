using System;

namespace MadLynx.Business.NumberSequences
{
	public interface INumberSequenceFormatter
	{
		/// <summary>
		/// Generate number with specific format
		/// </summary>
		string Format(string format, ulong num, DateTime date);
	}
}
