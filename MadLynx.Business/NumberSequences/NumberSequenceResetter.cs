using System;

namespace MadLynx.Business.NumberSequences
{
	public class NumberSequenceResetter<TCounter> : INumberSequenceResetter where TCounter : ICounter
	{
		private readonly ICounterDao<TCounter> counterDao;

		public NumberSequenceResetter(ICounterDao<TCounter> counterDao)
		{
			this.counterDao = counterDao;
		}

		public void ForceReset(ulong counterNumber, DateTime time)
		{
			var counter = counterDao.GetCounterWithLock(counterNumber);

			DoReset(counter, time);

			counterDao.UpdateCounter(counter);
		}

		public bool TryReset(ulong counterNumber, DateTime time)
		{
			var counter = counterDao.GetCounterWithLock(counterNumber);

			if (!CanReset(counter, time))
				return false;

			DoReset(counter, time);
			counterDao.UpdateCounter(counter);
			return true;
		}

		private bool CanReset(TCounter counter, DateTime time)
		{
			switch (counter.ResetMode)
			{
				case ResetMode.Yearly:
					return time.Year > counter.LastResetTime.Year;

				case ResetMode.Monthly:
					return MonthDifference(time, counter.LastResetTime) > 0;

				case ResetMode.Daily:
					return (time.Date - counter.LastResetTime.Date).TotalDays > 0;
			}

			return false;
		}

		private int MonthDifference(DateTime x, DateTime y)
		{
			var monthDifference = x.Month - y.Month;
			var yearDifference = x.Year - y.Year;

			return monthDifference + 12 * yearDifference;
		}

		private void DoReset(TCounter counter, DateTime time)
		{
			counter.Value = 1;
			counter.LastResetTime = time;
		}
	}
}
