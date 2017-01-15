using System.Text;

namespace MadLynx.Business.NumberSequences.Handlers
{
	public abstract class BaseFormatHandler : IFormatHandler
	{
		private const char ZeroDigit = '0';

		public abstract void Handle(FormatterContext context, FormatterBuildingContext buildingContext);

		/// <summary>
		/// Write <paramref name="num"/> to <paramref name="outputBuffer"/>. Trim to <paramref name="size"/> and add padding ('0') if necessary
		/// </summary>
		protected static void FormatDigits(StringBuilder outputBuffer, int num, int size, int startIndex)
		{
			var index = startIndex;
			while ((num != 0) && (index > startIndex - size))
			{
				outputBuffer[index--] = (char)((num % 10) + ZeroDigit);
				num /= 10;
			}

			while (index > startIndex - size)
			{
				outputBuffer[index--] = ZeroDigit;
			}
		}
	}
}
