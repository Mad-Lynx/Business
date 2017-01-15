namespace MadLynx.Business.NumberSequences
{
	public enum ResetMode
	{
		/// <summary>
		/// Sequence will not be reset
		/// </summary>
		Never = 0,

		/// <summary>
		/// Sequence will be reset each year
		/// </summary>
		Yearly = 1,

		/// <summary>
		/// Sequence will be reset every month
		/// </summary>
		Monthly = 2,

		/// <summary>
		/// Sequence will be reset every day
		/// </summary>
		Daily = 3,
	}
}
