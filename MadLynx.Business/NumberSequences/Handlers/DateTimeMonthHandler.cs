namespace MadLynx.Business.NumberSequences.Handlers
{
	public sealed class DateTimeMonthHandler : BaseFormatHandler
	{
		internal DateTimeMonthHandler()
		{
		}

		public override void Handle(FormatterContext context, FormatterBuildingContext buildingContext)
		{
			if (buildingContext.ConsecutiveChars == 2)
				FormatDigits(buildingContext.NumberBuilder, context.DateTime.Month, buildingContext.ConsecutiveChars, buildingContext.ActualPosition);
		}
	}
}
