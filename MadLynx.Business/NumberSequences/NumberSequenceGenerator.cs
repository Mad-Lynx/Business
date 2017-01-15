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

		#region Constructors

		public NumberSequenceGenerator(ICounterDao<TCounter> counterDao, INumberSequenceFormatter numberSequenceFormatter)
		{
			this.counterDao = counterDao;
			this.numberSequenceFormatter = numberSequenceFormatter;
		}

		#endregion

		/// <summary>
		/// Get next number in sequence
		/// </summary>
		public string GenerateNum(ulong counterNumber, Func<DateTime> dateGetter = null, ulong offset = 0)
		{
			return GenerateNum(counterNumber, null, dateGetter, offset);
		}

		/// <summary>
		/// Get next number in sequence
		/// </summary>
		public string GenerateNum(ulong counterNumber, string format, Func<DateTime> dateGetter = null, ulong offset = 0)
		{
			var counter = counterDao.GetCounterWithLock(counterNumber);

			var value = counter.Value ?? 1;
			counter.Value = value + 1;

			counterDao.UpdateCounter(counter);

			return numberSequenceFormatter.Format(format ?? counter.Format, value + offset, dateGetter != null ? dateGetter() : DateTime.Now);
		}
	}
}
