using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace dnEditor.Misc
{
    public static class DefaultColors
    {
        public static Color IndexTextColor = Color.Blue;
        public static Color OpCodeTextColor = Color.Green;
        public static Color OperandTextColor = Color.Black;
        public static Color RowBlockColor = Color.PaleGoldenrod;
        public static Color RowColor = Color.LightGoldenrodYellow;
    }

    public class ColoredRow
    {
        public Color OpCodeBackground = DefaultColors.RowColor;
        public Color OpCodeText = DefaultColors.OpCodeTextColor;
        public Color OperandText = DefaultColors.OperandTextColor;
    }
}
