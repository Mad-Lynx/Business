using MadLynx.Business.NumberSequences;

namespace MadLynx.Business.Tests.NumberSequences
{
	public class SimpleCounterConcurrencyTest : CounterConcurrencyBaseTest<TestCounter>
	{
		protected override ICounterDao<TestCounter> CreateCounterDao()
		{
			return new TestCounterDao();
		}

		protected override TestCounter InitCounter()
		{
			return new TestCounter
			{
				Number = CounterNum,
				Format = "N/####",
				Value = CounterStartValue,
			};
		}

		////protected override void IncrementCounter(WaitHandle wait, EventWaitHandle set)
		////{
		////	var transaction = BeginTransaction();
		////	base.IncrementCounter(wait, set);
		////	CommitTransaction(transaction);
		////}
	}
}