using System.Text;

namespace MadLynx.Business.Extensions
{
	public static class StringBuilderExtensions
	{
		public static string Substring(this StringBuilder sb, int startIndex, int length)
		{
			return sb.ToString(startIndex, length);
		}

		public static int IndexOf(this StringBuilder sb, char value)
		{
			return IndexOf(sb, value, 0);
		}

		public static int IndexOf(this StringBuilder sb, char value, int startIndex)
		{
			for (var i = startIndex; i < sb.Length; i++)
			{
				if (sb[i] == value)
					return i;
			}

			return -1;
		}

		public static int LastIndexOf(this StringBuilder sb, char value)
		{
			return LastIndexOf(sb, value, sb.Length - 1);
		}

		public static int LastIndexOf(this StringBuilder sb, char value, int startIndex)
		{
			for (var i = startIndex; i >= 0; i--)
			{
				if (sb[i] == value)
					return i;
			}

			return -1;
		}

		public static StringBuilder Poke(this StringBuilder sb, string value, int index)
		{
			var length = value.Length;
			for (var i = 0; i < length; i++)
			{
				sb[index + i] = value[i];
			}

			return sb;
		}

		public static StringBuilder Poke(this StringBuilder sb, StringBuilder value, int index)
		{
			var length = value.Length;
			for (var i = 0; i < length; i++)
			{
				sb[index + i] = value[i];
			}

			return sb;
		}
	}
}
