using System;
using System.Globalization;
using MadLynx.Business.NumberSequences;
using Moq;
using NUnit.Framework;

namespace MadLynx.Business.Tests.NumberSequences
{
	[TestFixture]
	public class ResetterTest
	{
		private const ulong CounterNumber = 1;
		private const ulong InitialValue = 123;

		private Mock<ICounterDao<TestCounter>> counterDaoMock;
		private INumberSequenceResetter resetter;

		[SetUp]
		public void Setup()
		{
			counterDaoMock = new Mock<ICounterDao<TestCounter>>();

			resetter = new NumberSequenceResetter<TestCounter>(counterDaoMock.Object);
		}

		[TestCase("2015-12-14", "2015-12-14", ResetMode.Daily)]
		[TestCase("2015-01-01", "2015-12-31", ResetMode.Daily)]
		[TestCase("2015-01-01", "2014-12-31", ResetMode.Daily)]
		[TestCase("2014-01-01", "2015-01-01", ResetMode.Daily)]
		[TestCase("2015-01-01", "2010-01-01", ResetMode.Daily)]
		[TestCase("2015-12-14", "2015-12-14", ResetMode.Never)]
		[TestCase("2015-01-01", "2015-12-31", ResetMode.Never)]
		[TestCase("2015-01-01", "2014-12-31", ResetMode.Never)]
		[TestCase("2014-01-01", "2015-01-01", ResetMode.Never)]
		[TestCase("2015-01-01", "2010-01-01", ResetMode.Never)]
		public void ForceReset(string timeNow, string lastResetTime, ResetMode resetMode)
		{
			var now = DateTime.Parse(timeNow, CultureInfo.InvariantCulture);
			var lastReset = DateTime.Parse(lastResetTime, CultureInfo.InvariantCulture);

			var testCounter = new TestCounter { Number = CounterNumber, Value = InitialValue, LastResetTime = lastReset, ResetMode = resetMode };

			counterDaoMock.Setup(dao => dao.GetCounterWithLock(CounterNumber))
				.Returns(testCounter);

			resetter.ForceReset(CounterNumber, now);

			counterDaoMock.Verify(dao => dao.GetCounterWithLock(CounterNumber), Times.Once);

			counterDaoMock.Verify(dao => dao.UpdateCounter(It.IsAny<TestCounter>()), Times.Once);
			Assert.AreEqual(now, testCounter.LastResetTime);
			Assert.AreEqual(1, testCounter.Value);
		}

		[TestCase("2015-12-14", "2015-12-14", false)]
		[TestCase("2015-01-01", "2015-12-31", false)]
		[TestCase("2015-01-01", "2014-12-31", true)]
		[TestCase("2014-01-01", "2015-01-01", false)]
		[TestCase("2015-01-01", "2010-01-01", true)]
		public void YearlyReset(string timeNow, string lastResetTime, bool resetSuccessed)
		{
			DoTryReset(
				ResetMode.Yearly,
				DateTime.Parse(timeNow, CultureInfo.InvariantCulture),
				DateTime.Parse(lastResetTime, CultureInfo.InvariantCulture),
				resetSuccessed);
		}

		[TestCase("2015-12-14", "2015-12-14", false)]
		[TestCase("2015-12-01", "2015-12-31", false)]
		[TestCase("2015-10-01", "2015-12-31", false)]
		[TestCase("2015-11-01", "2014-10-31", true)]
		[TestCase("2015-11-01", "2015-10-14", true)]
		[TestCase("2015-11-01", "2014-01-01", true)]
		[TestCase("2015-01-01", "2014-12-31", true)]
		[TestCase("2014-11-01", "2015-01-01", false)]
		public void MonthlyReset(string timeNow, string lastResetTime, bool resetSuccessed)
		{
			DoTryReset(
				ResetMode.Monthly,
				DateTime.Parse(timeNow, CultureInfo.InvariantCulture),
				DateTime.Parse(lastResetTime, CultureInfo.InvariantCulture),
				resetSuccessed);
		}

		[TestCase("2015-12-14 00:00:00", "2015-12-14 00:00:00", false)]
		[TestCase("2015-12-14 00:00:00", "2015-12-14 23:59:59", false)]
		[TestCase("2015-12-14 23:59:59", "2015-12-14 00:00:00", false)]
		[TestCase("2015-12-15 00:00:00", "2015-12-14 23:59:59", true)]
		[TestCase("2015-12-15 12:00:00", "2015-12-14 23:59:59", true)]
		[TestCase("2015-12-17 12:00:00", "2015-12-14 23:59:59", true)]
		[TestCase("2015-12-17 12:00:00", "2015-12-14 12:00:00", true)]
		[TestCase("2015-12-10 12:00:00", "2015-12-14 12:00:00", false)]
		[TestCase("2015-12-10 23:00:00", "2015-12-14 00:00:00", false)]
		[TestCase("2015-12-20 23:00:00", "2015-12-14 00:00:00", true)]
		public void DailyReset(string timeNow, string lastResetTime, bool resetSuccessed)
		{
			DoTryReset(
				ResetMode.Daily,
				DateTime.Parse(timeNow, CultureInfo.InvariantCulture),
				DateTime.Parse(lastResetTime, CultureInfo.InvariantCulture),
				resetSuccessed);
		}

		[TestCase("2015-12-14", "2015-12-14", false)]
		[TestCase("2015-12-01", "2015-12-31", false)]
		[TestCase("2015-10-01", "2015-12-31", false)]
		[TestCase("2015-11-01", "2014-10-31", false)]
		[TestCase("2015-11-01", "2014-01-01", false)]
		[TestCase("2015-01-01", "2014-12-31", false)]
		[TestCase("2014-11-01", "2015-01-01", false)]
		[TestCase("2015-01-01", "2010-01-01", false)]
		public void NeverReset(string timeNow, string lastResetTime, bool resetSuccessed)
		{
			DoTryReset(
				ResetMode.Never,
				DateTime.Parse(timeNow, CultureInfo.InvariantCulture),
				DateTime.Parse(lastResetTime, CultureInfo.InvariantCulture),
				resetSuccessed);
		}

		private void DoTryReset(ResetMode resetMode, DateTime now, DateTime lastReset, bool resetSuccessed)
		{
			var testCounter = new TestCounter { Number = CounterNumber, Value = InitialValue, LastResetTime = lastReset, ResetMode = resetMode };

			counterDaoMock.Setup(dao => dao.GetCounterWithLock(CounterNumber))
				.Returns(testCounter);

			var success = resetter.TryReset(CounterNumber, now);

			Assert.AreEqual(resetSuccessed, success);

			counterDaoMock.Verify(dao => dao.GetCounterWithLock(CounterNumber), Times.Once);

			if (success)
			{
				counterDaoMock.Verify(dao => dao.UpdateCounter(It.IsAny<TestCounter>()), Times.Once);
				Assert.AreEqual(now, testCounter.LastResetTime);
				Assert.AreEqual(1, testCounter.Value);
			}
			else
			{
				counterDaoMock.Verify(dao => dao.UpdateCounter(It.IsAny<TestCounter>()), Times.Never);
				Assert.AreEqual(lastReset, testCounter.LastResetTime);
				Assert.AreEqual(InitialValue, testCounter.Value);
			}
		}
	}
}
