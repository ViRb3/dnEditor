using System;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public static class InsUtils
    {
        public static string GetOperandText(this Instruction instruction)
        {
            string text = "";

            object operand = instruction.Operand;

            if (operand == null)
            {
                return text;
            }

            switch (operand.GetType().FullName)
            {
                case "System.String":
                    text = String.Format("\"{0}\"", operand);
                    break;
                case "System.Int32":
                case "System.Int16":
                case "System.Int64":
                    long l = Convert.ToInt64(operand);
                    if (l < 100)
                        text = l.ToString();
                    else
                        text = String.Format("0x{0:x}", l);
                    break;
                case "System.UInt32":
                case "System.UInt16":
                case "System.UInt64":
                    ulong ul = Convert.ToUInt64(operand);
                    if (ul < 100)
                        text = ul.ToString();
                    else
                        text = String.Format("0x{0:x}", ul);
                    break;
                case "System.Decimal":
                    text = operand.ToString();
                    break;
                case "System.Double":
                    text = operand.ToString();
                    break;
                case "System.Byte":
                case "System.SByte":
                    text = String.Format("0x{0:x}", operand);
                    break;
                case "System.Boolean":
                    text = operand.ToString();
                    break;
                case "System.Char":
                    text = String.Format("'{0}'", operand);
                    break;
                case "System.DateTime":
                    text = operand.ToString();
                    break;
                default:
                    text = operand.ToString();
                    break;
            }

            return text;
        }
    }
}