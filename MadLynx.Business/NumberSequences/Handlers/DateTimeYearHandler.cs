namespace MadLynx.Business.NumberSequences.Handlers
{
	public sealed class DateTimeYearHandler : BaseFormatHandler
	{
		internal DateTimeYearHandler()
		{
		}

		public override void Handle(FormatterContext context, FormatterBuildingContext buildingContext)
		{
			switch (buildingContext.ConsecutiveChars)
			{
				case 4:
				case 2:
				case 1:
					FormatDigits(buildingContext.NumberBuilder, context.DateTime.Year, buildingContext.ConsecutiveChars, buildingContext.ActualPosition);
					break;
			}
		}
	}
}
