using System;
using System.Diagnostics;
using MadLynx.Business.NumberSequences;
using NUnit.Framework;

namespace MadLynx.Business.Tests.NumberSequences
{
	[TestFixture]
	public class FormatterTest
	{
		private INumberSequenceFormatter formatter;

		[SetUp]
		public void OnSetUp()
		{
			formatter = new NumberSequenceFormatter();
		}

		//// various date formats
		[TestCase("F#####/mm/yyyy", 396u, Result = "F00396/07/1492")]
		[TestCase("F#####/mm/yy", 396u, Result = "F00396/07/92")]
		[TestCase("F#####/mm/y", 396u, Result = "F00396/07/2")]
		[TestCase("F#####/dd/yy", 396u, Result = "F00396/15/92")]
		[TestCase("F#####/y/yy/yyy/yyyy", 396u, Result = "F00396/2/92/yyy/1492")]
		//// fill the number
		[TestCase("F/yyyy/#####", 0u, Result = "F/1492/00000")]
		[TestCase("F/yyyy/#####", 1u, Result = "F/1492/00001")]
		[TestCase("F/yyyy/#####", 396u, Result = "F/1492/00396")]
		[TestCase("F/yyyy/###", 396u, Result = "F/1492/396")]
		[TestCase("F/yyyy/##", 396u, Result = "", ExpectedException = typeof(IncorrectFormatException))]
		[TestCase("F/yyyy/#########", 396u, Result = "F/1492/000000396")]
		[TestCase("F/yyyy/###-##", 396u, Result = "F/1492/003-96", Description = "uzupełniamy wszystkie #")]
		[TestCase("F/yyyy/##################", 396u, Result = "F/1492/000000000000000396", Description = "Obsługa powyżej 16 #")]
		//// fill the number with literal format
		[TestCase("F/##&&-#&", 321u, Result = "F/00AB-2J")]
		[TestCase("F/##&&&#", 30u, Result = "F/00AAD0")]
		[TestCase("F/##&", 30u, Result = "F/01E")]
		[TestCase("F/&&&", 2u, Result = "F/AAC")]
		[TestCase("F/&&&", 0u, Result = "F/AAA")]
		[TestCase("F/##&", 2600u, Result = "", ExpectedException = typeof(IncorrectFormatException))]
		//// mix
		// TODO: make more test case
		[TestCase("F/yyyy/###hh", 396u, Result = "F/1492/396hh")]
		public string TestNumberFillsCorrectly(string format, uint num)
		{
			var date = new DateTime(1492, 7, 15);

			return formatter.Format(format, num, date);
		}

		[Test]
		[Category("Smoke")]
		public void TestSpeed()
		{
			var dateTime = DateTime.Now;

			// Stopwatch[normal]: 710ms
			SpeedTest(
				"normal",
				i =>
				{
					var num = formatter.Format("FV/########/dd/mm/y/yy/yyy/yyyy", i, dateTime);
				},
				200000);

			// Stopwatch[literal]: 790ms
			SpeedTest(
				"literal",
				i =>
				{
					var num = formatter.Format("FV/###&#&#&/dd/#&/yyyy", i, dateTime);
				},
				200000);
		}

		private static void SpeedTest(string name, Action<uint> action, int repeat = 1)
		{
			var st = new Stopwatch();
			st.Start();
			for (var i = 0u; i < repeat; i++)
			{
				action(i);
			}

			st.Stop();

			Console.WriteLine("Stopwatch[{0}]: {1}ms", name, st.Elapsed.TotalMilliseconds);
		}
	}
}
