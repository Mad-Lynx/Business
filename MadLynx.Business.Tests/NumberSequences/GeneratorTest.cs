using System;
using System.Globalization;
using MadLynx.Business.NumberSequences;
using Moq;
using NUnit.Framework;

namespace MadLynx.Business.Tests.NumberSequences
{
	[TestFixture]
	public class GeneratorTest
	{
		private Mock<ICounterDao<TestCounter>> counterDaoMock;
		private NumberSequenceGenerator<TestCounter> generator;

		[SetUp]
		public void Setup()
		{
			counterDaoMock = new Mock<ICounterDao<TestCounter>>();
			var formatterMock = new Mock<INumberSequenceFormatter>();

			formatterMock.Setup(f => f.Format(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<DateTime>()))
				.Returns((Func<string, ulong, DateTime, string>)FakeFormat);

			generator = new NumberSequenceGenerator<TestCounter>(counterDaoMock.Object, formatterMock.Object);
		}

		[TestCase(null, 0u, "F", Result = "F|1")]
		[TestCase(0u, 0u, "F", Result = "F|0")]
		[TestCase(1u, 0u, "F", Result = "F|1")]
		[TestCase(22u, 0u, "F", Result = "F|22")]
		[TestCase(null, 3u, "F", Result = "F|4")]
		[TestCase(0u, 3u, "F", Result = "F|3")]
		[TestCase(1u, 3u, "F", Result = "F|4")]
		[TestCase(22u, 3u, "F", Result = "F|25")]
		[TestCase(null, 3u, "GG", Result = "GG|4")]
		[TestCase(0u, 3u, "GG", Result = "GG|3")]
		[TestCase(1u, 3u, "GG", Result = "GG|4")]
		[TestCase(22u, 3u, "GG", Result = "GG|25")]
		public string GenerateNum_DateTimeIsNow(uint? initialValue, uint offset, string format)
		{
			counterDaoMock.Setup(dao => dao.GetCounterWithLock(1))
				.Returns(new TestCounter { Number = 1, Format = format, Value = initialValue });

			var num = generator.GenerateNum(1, DateTime.Now, offset);

			var yearIndexOf = num.LastIndexOf('|');

			Assert.AreEqual(DateTime.Now.Year.ToString(), num.Substring(yearIndexOf + 1));

			return num.Substring(0, yearIndexOf);
		}

		[TestCase("CF", "UU", Result = "UU")]
		[TestCase("CF", "", Result = "")]
		[TestCase("CF", null, Result = "CF")]
		public string GenerateNum_GetSpecificFormat(string counterFormat, string specificFormat)
		{
			counterDaoMock.Setup(dao => dao.GetCounterWithLock(1))
				.Returns(new TestCounter { Number = 1, Format = counterFormat });

			var num = generator.GenerateNum(1, specificFormat);

			var formatIndexOf = num.IndexOf('|');

			return num.Substring(0, formatIndexOf);
		}

		private string FakeFormat(string format, ulong num, DateTime date)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}", format, num, date.Year);
		}
	}
}