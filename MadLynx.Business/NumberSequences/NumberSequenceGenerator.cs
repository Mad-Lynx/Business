using System;

namespace MadLynx.Business.NumberSequences
{
	/// <summary>
	/// Number sequence generator
	/// </summary>
	public class NumberSequenceGenerator<TCounter> : INumberSequenceGenerator where TCounter : ICounter
	{
		private readonly ICounterDao<TCounter> counterDao;
		private readonly INumberSequenceFormatter numberSequenceFormatter;

		public NumberSequenceGenerator(ICounterDao<TCounter> counterDao, INumberSequenceFormatter numberSequenceFormatter)
		{
			this.counterDao = counterDao;
			this.numberSequenceFormatter = numberSequenceFormatter;
		}

		/// <summary>
		/// Get next number in sequence
		/// </summary>
		public string GenerateNum(ulong counterNumber, DateTime date, ulong offset = 0)
		{
			return GenerateNum(counterNumber, null, date, offset);
		}

		/// <summary>
		/// Get next number in sequence
		/// </summary>
		public string GenerateNum(ulong counterNumber, string format, DateTime? date = null, ulong offset = 0)
		{
			var counter = counterDao.GetCounterWithLock(counterNumber);

			var value = counter.Value ?? 1;
			counter.Value = value + 1;

			counterDao.UpdateCounter(counter);

			return numberSequenceFormatter.Format(format ?? counter.Format, value + offset, date ?? DateTime.Now);
		}
	}
}
