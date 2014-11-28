using System.Text.RegularExpressions;
using dnEditor.Forms;
using dnEditor.Properties;

namespace dnEditor.Misc
{
    public static class ObfuscationHelper
    {
        public static int MaxTreeNodeTextLength
        {
            get { return 85; }
        }

        public static int MaxOperandTextLength
        {
            get { return 150; }
        }

        public static string ShortenTreeNodeText(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (!string.IsNullOrEmpty(Settings.Default.MagicRegex))
                text = Regex.Replace(text, Settings.Default.MagicRegex, "");

            if (text.Length > MaxTreeNodeTextLength)
                return text.Substring(0, MaxTreeNodeTextLength) + " ...";

            return text;
        }

        public static string ShortenOperandText(this string text)
        {
            if (!string.IsNullOrEmpty(Settings.Default.MagicRegex))
                text = Regex.Replace(text, Settings.Default.MagicRegex, "");

            if (text.Length > MaxOperandTextLength)
                return text.Substring(0, MaxOperandTextLength) + " ...";

            return text;
        }
    }
}
