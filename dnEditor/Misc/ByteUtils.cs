using System;
using System.Globalization;
using System.Text;

namespace dnEditor.Misc
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public class BytesUtils
    {
        public static string BytesToHexStringBlock(byte[] bytes)
        {
            if (bytes == null)
                return String.Empty;

            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:x02} ", bytes[i]);
                if ((i + 1)%16 == 0)
                {
                    sb.Append("\r\n");
                }
            }
            if (bytes.Length%16 != 0)
            {
                sb.Append("\r\n");
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] >= 0x20 && bytes[i] <= 0x7e)
                {
                    sb.AppendFormat("{0}", (char) bytes[i]);
                }
                else
                {
                    sb.Append(".");
                }
                if ((i + 1)%48 == 0)
                {
                    sb.Append("\r\n");
                }
            }
            return sb.ToString();
        }

        public static string BytesToHexString(byte[] bytes)
        {
            return BytesToHexString(bytes, false);
        }

        public static string BytesToHexString(byte[] bytes, bool space)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:x02}{1}", bytes[i], space ? " " : "");
            }
            return sb.ToString().ToUpperInvariant();
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            string hs = hexString.Replace(" ", "");
            int length = hs.Length/2;
            var bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                string s = hs.Substring(i*2, 2);
                bytes[i] = Byte.Parse(s, NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}