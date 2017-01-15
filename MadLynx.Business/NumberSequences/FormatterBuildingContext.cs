using System.Collections;
using System.Text;

namespace MadLynx.Business.NumberSequences
{
	public sealed class FormatterBuildingContext
	{
		public FormatterBuildingContext(StringBuilder numberBuilder)
		{
			NumberBuilder = numberBuilder;
			HandlerParameters = new Hashtable();
		}

		public StringBuilder NumberBuilder { get; private set; }

		public char PatternChar { get; internal set; }

		public int ConsecutiveChars { get; internal set; }

		public int ActualPosition { get; internal set; }

		public Hashtable HandlerParameters { get; private set; }

		internal int InternalNumberLength { get; set; }
	}
}