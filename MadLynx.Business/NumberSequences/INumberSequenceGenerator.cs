using System;

namespace MadLynx.Business.NumberSequences
{
	public interface INumberSequenceGenerator
	{
		/// <summary>
		/// Get next number in sequence
		/// </summary>
		string GenerateNum(ulong counterNumber, Func<DateTime> dateGetter = null, ulong offset = 0);

		/// <summary>
		/// Get next number in sequence, use specific format instead of format from Counter
		/// </summary>
		string GenerateNum(ulong counterNumber, string format, Func<DateTime> dateGetter = null, ulong offset = 0);
	}
}
