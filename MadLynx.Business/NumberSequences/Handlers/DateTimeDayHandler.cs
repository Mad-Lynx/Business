namespace MadLynx.Business.NumberSequences.Handlers
{
	public sealed class DateTimeDayHandler : BaseFormatHandler
	{
		internal DateTimeDayHandler()
		{
		}

		public override void Handle(FormatterContext context, FormatterBuildingContext buildingContext)
		{
			if (buildingContext.ConsecutiveChars == 2)
				FormatDigits(buildingContext.NumberBuilder, context.DateTime.Day, buildingContext.ConsecutiveChars, buildingContext.ActualPosition);
		}
	}
}
