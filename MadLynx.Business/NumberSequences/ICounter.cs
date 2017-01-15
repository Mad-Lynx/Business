namespace MadLynx.Business.NumberSequences
{
	public interface ICounter
	{
		ulong Number { get; set; }

		ulong? Value { get; set; }

		string Format { get; set; }
	}
}
