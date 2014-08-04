using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dnEditor.Forms;

namespace dnEditor.Misc
{
    internal static class ColorRules
    {
        public static Color BlockColor = Color.PaleGoldenrod;
        public static Dictionary<string, Color> Rules = new Dictionary<string, Color>();
        public static Color DefaultColor = Color.LightGoldenrodYellow;

        static ColorRules()
        {
            Rules.Add("call", Color.LightCyan);
            Rules.Add("calli", Color.LightCyan);
            Rules.Add("callvirt", Color.LightCyan);
            Rules.Add("ldstr", Color.PeachPuff);
        }

        public static void ApplyColors(DataGridView dgView)
        {
            if (dgView == null || dgView.ColumnCount == 0 || dgView.RowCount == 0) return;

            foreach (DataGridViewRow row in dgView.Rows)
            {
                Color newColor = DefaultColor;
                string opcode = row.Cells["opcode"].Value.ToString().Trim(' ');

                if (Rules.ContainsKey(opcode))
                {
                    Rules.TryGetValue(opcode, out newColor);


                    if (row.DefaultCellStyle.BackColor == BlockColor)
                    {
                        newColor = Color.FromArgb(newColor.R - 17 < 0 ? newColor.R : newColor.R - 17,
                            newColor.G - 23 < 0 ? newColor.G : newColor.G - 23,
                            newColor.B - 85 < 0 ? newColor.B : newColor.B - 85);
                    }

                    row.DefaultCellStyle.BackColor = newColor;
                }  
            }
        }

        public static void MarkBlocks(DataGridView dgView)
        {
            if (dgView == null || dgView.ColumnCount == 0 || dgView.RowCount == 0) return;

            List<InstructionBlock> list = InstructionBlock.Find(MainForm.CurrentAssembly.Method.Method);

            bool changeColor = false;
            foreach (InstructionBlock ib in list)
            {
                if (changeColor)
                {
                    for (int i = ib.StartIndex; i <= ib.EndIndex; i++)
                    {
                        dgView.Rows[i].DefaultCellStyle.BackColor = BlockColor;
                    }
                }
                changeColor = !changeColor;
            }
        }
    }
}