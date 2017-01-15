using System;
using System.Threading;
using MadLynx.Business.NumberSequences;
using NUnit.Framework;

namespace MadLynx.Business.Tests.NumberSequences
{
	[TestFixture]
	public abstract class CounterConcurrencyBaseTest<TCounter> where TCounter : ICounter
	{
		protected const ulong CounterNum = 9998765;
		protected const int CounterStartValue = 123;

		private ICounterDao<TCounter> counterDao;

		protected virtual TimeSpan Timeout
		{
			get { return TimeSpan.FromSeconds(2); }
		}

		[SetUp]
		public void Setup()
		{
			OnSetup();
		}

		[Test]
		public void TestConcurrencyAccess()
		{
			// double check if starting value is as we expected (especially because we should do this in separate thread and transaction)
			var counter = counterDao.GetCounter(CounterNum);
			Assert.IsNotNull(counter, "Could not find counter in database/counterDao");
			Assert.AreEqual(CounterStartValue, counter.Value);

			var resetEvent = new ManualResetEvent(false);
			var resetEventTrue = new ManualResetEvent(true);

			var threadOne = new Thread(() => IncrementCounter(resetEventTrue, resetEvent));
			var threadTwo = new Thread(() => IncrementCounter(resetEvent, resetEventTrue));

			threadOne.Start();
			threadTwo.Start();

			// 2s timeout for deadlock
			var successfulTerminated = threadOne.Join(Timeout) & threadTwo.Join(Timeout);
			Assert.IsTrue(successfulTerminated, "There was some deadlocks and process was terminated!");

			// Check if we really increment counter by 2
			counter = counterDao.GetCounter(CounterNum);
			Assert.AreEqual(CounterStartValue + 2, counter.Value);
		}

		protected abstract ICounterDao<TCounter> CreateCounterDao();

		protected virtual void OnSetup()
		{
			counterDao = CreateCounterDao();
			InsertNewCounter();
		}

		protected virtual void InsertNewCounter()
		{
			var newCounter = InitCounter();
			counterDao.UpdateCounter(newCounter);
		}

		protected abstract TCounter InitCounter();

		/// <summary>
		/// Incrementing counter. This should be run in separate and totally new transaction
		/// </summary>
		protected virtual void IncrementCounter(WaitHandle wait, EventWaitHandle set)
		{
			wait.WaitOne();
			var counter = counterDao.GetCounterWithLock(CounterNum);

			set.Set();

			// Give some time to other thread to select 'wrong/old' data
			Thread.Sleep(100);

			counter.Value++;

			counterDao.UpdateCounter(counter);
		}
	}
}
