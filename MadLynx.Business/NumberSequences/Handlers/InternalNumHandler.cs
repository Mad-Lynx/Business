namespace MadLynx.Business.NumberSequences.Handlers
{
	internal sealed class InternalNumHandler : IFormatHandler
	{
		private readonly char filler;

		public InternalNumHandler(char filler)
		{
			this.filler = filler;
		}

		public void Handle(FormatterContext context, FormatterBuildingContext buildingContext)
		{
			var size = buildingContext.ConsecutiveChars;
			var startIndex = buildingContext.ActualPosition;
			var numberLength = buildingContext.InternalNumberLength;

			var index = startIndex;
			while ((numberLength > 0) && (index > startIndex - size))
			{
				buildingContext.NumberBuilder[index--] = context.NumberText[--numberLength];
			}

			buildingContext.InternalNumberLength = numberLength;

			while (index > startIndex - size)
			{
				buildingContext.NumberBuilder[index--] = filler;
			}
		}
	}
}
