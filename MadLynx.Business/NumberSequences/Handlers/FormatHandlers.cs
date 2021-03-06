﻿namespace MadLynx.Business.NumberSequences.Handlers
{
	public static class FormatHandlers
	{
		/// <summary>
		/// Handler for days. Use with [dd] placeholder
		/// </summary>
		public static readonly IFormatHandler DayHandler = new DateTimeDayHandler();

		/// <summary>
		/// Handler for months. Use with [mm] placeholder
		/// </summary>
		public static readonly IFormatHandler MonthHandler = new DateTimeMonthHandler();

		/// <summary>
		/// Handler for years. Use with [y], [yy] and [yyyy] placeholders
		/// </summary>
		public static readonly IFormatHandler YearHandler = new DateTimeYearHandler();

		/// <summary>
		/// Handler for normal digits. Use with [#] placeholder
		/// </summary>
		public static readonly IFormatHandler DigitsHandler = new InternalNumHandler(FirstDigit);

		/// <summary>
		/// Handler for numbers with letters. Use with [&amp;] placeholder
		/// </summary>
		public static readonly IFormatHandler LettersHandler = new InternalNumHandler(FirstLetter);

		/// <summary>
		/// Handler for normal digits, but it will remove 'leading zeros'. May be use with [%] placeholder
		/// </summary>
		/// <remarks>
		/// This handler is not part of default handler set
		/// </remarks>
		public static readonly IFormatHandler BlankHandler = new InternalNumHandler(CharToRemove);

		internal const char FirstDigit = '0';

		internal const char FirstLetter = 'A';

		internal const char CharToRemove = '\0';
	}
}
