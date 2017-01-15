using System;

namespace MadLynx.Business.NumberSequences
{
	public interface INumberSequenceGenerator
	{
		/// <summary>
		/// Get next number in sequence
		/// </summary>
		string GenerateNum(ulong counterNumber, DateTime date, ulong offset = 0);

		/// <summary>
		/// Get next number in sequence, use specific format instead of format from Counter
		/// </summary>
		string GenerateNum(ulong counterNumber, string format, DateTime? date = null, ulong offset = 0);
	}
}
