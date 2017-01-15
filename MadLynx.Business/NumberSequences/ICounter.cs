using System;

namespace MadLynx.Business.NumberSequences
{
	public interface ICounter
	{
		ulong Number { get; }

		ulong? Value { get; set; }

		string Format { get; }

		ResetMode ResetMode { get; }

		DateTime LastResetTime { get; set; }
	}
}
