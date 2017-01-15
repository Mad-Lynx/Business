using System.Collections.Generic;
using System.Threading;
using MadLynx.Business.NumberSequences;

namespace MadLynx.Business.Tests.NumberSequences
{
	internal class TestCounterDao : ICounterDao<TestCounter>
	{
		private readonly Dictionary<ulong, TestCounter> counters = new Dictionary<ulong, TestCounter>();

		public TestCounter GetCounter(ulong counterNumber)
		{
			return GetCounter(counterNumber, false);
		}

		public TestCounter GetCounterWithLock(ulong counterNumber)
		{
			return GetCounter(counterNumber, true);
		}

		public void UpdateCounter(TestCounter counter)
		{
			var newCounter = !counters.ContainsKey(counter.Number);
			if (newCounter)
				counters.Add(counter.Number, counter.Clone());

			counters[counter.Number].Value = counter.Value;

			if (!newCounter)
				Monitor.Exit(counters[counter.Number]);
		}

		private TestCounter GetCounter(ulong counterNumber, bool forUpdate)
		{
			TestCounter counter;
			if (counters.TryGetValue(counterNumber, out counter))
			{
				if (forUpdate)
					Monitor.Enter(counter);
				return counter.Clone();
			}

			return null;
		}
	}
}
