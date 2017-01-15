namespace MadLynx.Business.NumberSequences
{
	public interface ICounterDao<TCounter> where TCounter : ICounter
	{
		TCounter GetCounter(ulong counterNumber);

		TCounter GetCounterWithLock(ulong counterNumber);

		void UpdateCounter(TCounter counter);
	}
}
