namespace MadLynx.Business.NumberSequences
{
	public interface IFormatHandler
	{
		void Handle(FormatterContext context, FormatterBuildingContext buildingContext);
	}
}
