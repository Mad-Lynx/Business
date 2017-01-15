using MadLynx.Business.NumberSequences;

namespace MadLynx.Business.Tests.NumberSequences
{
	public class TestCounter : ICounter
	{
		public ulong Number { get; set; }

		public ulong? Value { get; set; }

		public string Format { get; set; }

		public TestCounter Clone()
		{
			return new TestCounter
			{
				Number = Number,
				Value = Value,
				Format = Format,
			};
		}
	}
}
