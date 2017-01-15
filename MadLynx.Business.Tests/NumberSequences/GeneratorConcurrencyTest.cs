using System;
using System.Linq;
using System.Threading;
using MadLynx.Business.NumberSequences;
using NUnit.Framework;

namespace MadLynx.Business.Tests.NumberSequences
{
	[TestFixture]
	[Category("Smoke")]
	public class GeneratorConcurrencyTest
	{
		private const ulong CounterNum = 9998765;
		private const int CounterStartValue = 123;

		[Test]
		public void TestConcurrencyAccess()
		{
			var generator = CreateGenerator();

			// Generate first number, and check if everything is ok
			Assert.AreEqual("N/0123", generator.GenerateNum(CounterNum));

			ParallelEnumerable.Range(1, 20).ForAll(
				i =>
				{
					var num = generator.GenerateNum(CounterNum);
					Console.WriteLine("{0:00}) [Thread-{1:00}] {2}", i, Thread.CurrentThread.ManagedThreadId, num);
				});

			Assert.AreEqual("N/0144", generator.GenerateNum(CounterNum));
		}

		private static NumberSequenceGenerator<TestCounter> CreateGenerator()
		{
			var counterDao = new TestCounterDao();
			var numberSequenceFormatter = new NumberSequenceFormatter();
			var generator = new NumberSequenceGenerator<TestCounter>(counterDao, numberSequenceFormatter);

			counterDao.UpdateCounter(new TestCounter { Number = CounterNum, Format = "N/####", Value = CounterStartValue });
			return generator;
		}
	}
}
