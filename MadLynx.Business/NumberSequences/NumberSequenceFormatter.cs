using System;
using System.Collections.Generic;
using System.Text;
using MadLynx.Business.NumberSequences.Handlers;

namespace MadLynx.Business.NumberSequences
{
	/// <summary>
	/// Number sequence formatting engine
	/// </summary>
	public class NumberSequenceFormatter : INumberSequenceFormatter
	{
		public static readonly Dictionary<char, IFormatHandler> DefaultFormatHandlers = new Dictionary<char, IFormatHandler>
		{
			{ DigitsMask, FormatHandlers.DigitsHandler },
			{ LettersMask, FormatHandlers.LettersHandler },
			{ 'y', FormatHandlers.YearHandler },
			{ 'm', FormatHandlers.MonthHandler },
			{ 'd', FormatHandlers.DayHandler }
		};

		private const char DigitsMask = '#';
		private const char LettersMask = '&';
		private static readonly char[] FormattingChars = { DigitsMask, LettersMask };
		private readonly Dictionary<char, IFormatHandler> formatHandlers;

		public NumberSequenceFormatter()
			: this(DefaultFormatHandlers)
		{
		}

		public NumberSequenceFormatter(Dictionary<char, IFormatHandler> formatHandlers)
		{
			if (formatHandlers == null)
				throw new ArgumentNullException("formatHandlers");

			this.formatHandlers = formatHandlers;
		}

		/// <summary>
		/// Generate number with specific <paramref name="format"/> using <paramref name="num"/> and <paramref name="date"/> to fill it
		/// </summary>
		public string Format(string format, ulong num, DateTime date)
		{
			if (String.IsNullOrEmpty(format))
				return num.ToString();

			if (format.IndexOf(LettersMask) != -1)
				return InsertFormatLetters(format, num, date);

			return InsertFormatInternal(new FormatterContext(format, num, num.ToString(), date));
		}

		/// <summary>
		/// Change <paramref name="num"/> to special format, converting '#' to digits, and '&amp;' to letters
		/// </summary>
		private string InsertFormatLetters(string format, ulong num, DateTime date)
		{
			var origNum = num;
			var tmpNum = new StringBuilder(format.Length);

			for (var i = format.IndexOfAny(FormattingChars); i != -1; i = format.IndexOfAny(FormattingChars, i + 1))
			{
				tmpNum.Append(format[i]);
			}

			var index = tmpNum.Length - 1;
			while ((num != 0) && (index >= 0))
			{
				var patternChar = tmpNum[index];
				switch (patternChar)
				{
					case DigitsMask:
						patternChar = (char)((num % 10) + FormatHandlers.FirstDigit);
						num /= 10;
						break;
					case LettersMask:
						patternChar = (char)((num % 26) + FormatHandlers.FirstLetter);
						num /= 26;
						break;
				}

				tmpNum[index--] = patternChar;
			}

			if (num != 0)
				throw new IncorrectFormatException(format, origNum.ToString(), "Format is too short for the given number.");

			return InsertFormatInternal(new FormatterContext(format, origNum, tmpNum.ToString(index + 1, tmpNum.Length - index - 1), date));
		}

		/// <summary>
		/// Generate number with specific <paramref name="context.Format"/> using
		/// <paramref name="context.NumberText"/> and <paramref name="context.DateTime"/> to fill it
		/// </summary>
		/// <remarks>Allowed formats: [#&amp;]+, (y), (yy), (yyyy), (mm), (dd), </remarks>
		private string InsertFormatInternal(FormatterContext context)
		{
			var buildingContext = new FormatterBuildingContext(new StringBuilder(context.Format));
			buildingContext.InternalNumberLength = context.NumberText.Length;

			int delta;
			for (var index = buildingContext.NumberBuilder.Length - 1; index >= 0; index -= delta)
			{
				buildingContext.PatternChar = buildingContext.NumberBuilder[index];

				IFormatHandler handler;
				if (formatHandlers.TryGetValue(buildingContext.PatternChar, out handler))
				{
					buildingContext.ActualPosition = index;
					delta = ParseRepeatPattern(buildingContext);
					buildingContext.ConsecutiveChars = delta;

					handler.Handle(context, buildingContext);
				}
				else
				{
					delta = 1;
				}
			}

			if (buildingContext.InternalNumberLength > 0)
				throw new IncorrectFormatException(context.Format, context.NumberText, "Format is too short for the given number.");

			if (buildingContext.NumberBuilder.Length != context.Format.Length)
				throw new InvalidOperationException("Generated number has a different length than given format!");

			return buildingContext.NumberBuilder.Replace(FormatHandlers.CharToRemove.ToString(), String.Empty).ToString();
		}

		/// <summary>
		/// Return number of repeated <paramref name="buildingContext.PatternChar"/>
		/// </summary>
		private static int ParseRepeatPattern(FormatterBuildingContext buildingContext)
		{
			var index = buildingContext.ActualPosition - 1;
			var patternChar = buildingContext.PatternChar;
			while ((index >= 0) && (buildingContext.NumberBuilder[index] == patternChar))
			{
				index--;
			}

			return buildingContext.ActualPosition - index;
		}
	}
}
