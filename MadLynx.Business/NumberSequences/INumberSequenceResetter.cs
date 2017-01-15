using System;

namespace MadLynx.Business.NumberSequences
{
	public interface INumberSequenceResetter
	{
		void ForceReset(ulong counterNumber, DateTime time);

		bool TryReset(ulong counterNumber, DateTime time);
	}
}