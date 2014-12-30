using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    public static class ColorRules
    {
        public static Dictionary<OpCode, ColoredRow> Rules = new Dictionary<OpCode, ColoredRow>();

        static ColorRules()
        {
            List<OpCode> calls =
                Functions.OpCodes.Where(opcode => opcode.OperandType == OperandType.InlineMethod ||
                                                  opcode.OperandType == OperandType.InlineSig).ToList();

            foreach (OpCode opCode in calls)
            {
                var row = new ColoredRow
                {
                    OpCodeText = Color.MediumBlue,
                    OperandText = Color.MediumBlue
                };

                Rules.Add(opCode, row);
            }

            var stringRow = new ColoredRow
            {
                OpCodeText = Color.DarkRed,
                OperandText = Color.DarkRed
            };

            Rules.Add(OpCodes.Ldstr, stringRow);

            var retRow = new ColoredRow
            {
                OpCodeBackground = Color.Yellow
            };

            Rules.Add(OpCodes.Ret, retRow);
            Rules.Add(OpCodes.Rethrow, retRow);

            List<OpCode> jumps =
                Functions.OpCodes.Where(opcode => opcode.OperandType == OperandType.ShortInlineBrTarget ||
                                                  opcode.OperandType == OperandType.InlineBrTarget).ToList();

            foreach (OpCode opCode in jumps)
            {
                var row = new ColoredRow
                {
                    OpCodeText = Color.DarkMagenta,
                    OperandText = Color.DarkMagenta
                };

                Rules.Add(opCode, row);
            }
        }

        public static void ApplyColors(DataGridView dgView)
        {
            if (dgView == null || dgView.ColumnCount == 0 || dgView.RowCount == 0) return;

            foreach (DataGridViewRow row in dgView.Rows)
            {
                string opcode = row.Cells["opcode"].Value.ToString().Trim(' ');

                if (Rules.Keys.Count(key => key.Name == opcode) > 0)
                {
                    OpCode opCode = Rules.Keys.First(key => key.Name == opcode);

                    ColoredRow coloredRow = Rules.First(o => o.Key == opCode).Value;

                    /* Color darkening for instruction blocks
 
                    if (row.DefaultCellStyle.BackColor == BlockColor)
                    {
                        newColor = Color.FromArgb(newColor.R - 17 < 0 ? newColor.R : newColor.R - 17,
                            newColor.G - 23 < 0 ? newColor.G : newColor.G - 23,
                            newColor.B - 85 < 0 ? newColor.B : newColor.B - 85);
                    }*/

                    row.Cells[2].Style.ForeColor = coloredRow.OpCodeText;

                    if (coloredRow.OpCodeBackground != DefaultColors.RowColor &&
                        coloredRow.OpCodeBackground != DefaultColors.RowBlockColor)
                    {
                        row.Cells[2].Style.BackColor = coloredRow.OpCodeBackground;
                    }

                    row.Cells[3].Style.ForeColor = coloredRow.OperandText;
                }
            }
        }

        public static void MarkBlocks(DataGridView dgView)
        {
            if (dgView == null || dgView.ColumnCount == 0 || dgView.RowCount == 0) return;

            List<InstructionBlock> list = InstructionBlock.Find(MainForm.CurrentAssembly.Method);

            bool changeColor = false;
            foreach (InstructionBlock ib in list)
            {
                if (changeColor)
                {
                    for (int i = ib.StartIndex; i <= ib.EndIndex; i++)
                    {
                        dgView.Rows[i].DefaultCellStyle.BackColor = DefaultColors.RowBlockColor;
                    }
                }
                changeColor = !changeColor;
            }
        }
    }
}