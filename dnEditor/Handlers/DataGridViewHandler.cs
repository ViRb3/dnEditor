using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Handlers
{
    public static class DataGridViewHandler
    {
        public static void InitializeBody()
        {
            MainForm.DgBody.Columns["index"].DefaultCellStyle.ForeColor = Color.Blue;
            MainForm.DgBody.Columns["opcode"].DefaultCellStyle.ForeColor = Color.Green;
            MainForm.DgBody.DefaultCellStyle.BackColor = ColorRules.DefaultColor;
        }

        public static void ReadMethod(MethodDef method)
        {
            MainForm.DgBody.Rows.Clear();

            if (!method.HasBody || !method.Body.HasInstructions) return;

            int i = 0;
            var rows = new List<DataGridViewRow>();

            foreach (Instruction instruction in method.Body.Instructions)
            {
                var cells = new List<object>();

                cells.Add(i++); // Column 1
                cells.Add(Functions.GetAddress(instruction)); // Column 2
                cells.Add(instruction.OpCode); // Column 3

                #region Column 4

                if (instruction.Operand is Instruction)
                    cells.Add(string.Format("{0}",
                        Functions.FormatFullInstruction(method.Body.Instructions.ToList(),
                            method.Body.Instructions.IndexOf(instruction))));
                else
                    cells.Add(Functions.GetOperandText(method.Body.Instructions.ToList(),
                        method.Body.Instructions.ToList().IndexOf(instruction)));

                #endregion Column 4

                #region Application

                for (int j = 0; j < cells.Count; j++)
                {
                    cells[j] = "   " + cells[j];
                }

                var row = new DataGridViewRow();

                for (int j = 0; j < 4; j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell());
                }

                for (int j = 0; j < cells.Count; j++)
                {
                    row.Cells[j].Value = cells[j];
                }

                #endregion Application

                string definition;
                Functions.OpCodeDictionary.TryGetValue(Functions.GetOpCode(instruction.OpCode.ToString().Trim()),
                    out definition);

                if (definition != null)
                    row.Cells[2].ToolTipText = definition;

                row.Tag = instruction;
                row.Height = 16;

                row.ContextMenuStrip = MainForm.InsructionMenuStrip;

                rows.Add(row);
            }

            MainForm.DgBody.Rows.AddRange(rows.ToArray());

            MainForm.CurrentAssembly.Method.NewMethod = method;
            ColorRules.MarkBlocks(MainForm.DgBody);
            ColorRules.ApplyColors(MainForm.DgBody);
        }

        public static void ClearRows()
        {
            MainForm.DgBody.Rows.Clear();
        }

        public static void SearchFinished(object result)
        {
            if (result is int) // row index
            {
                int i = int.Parse(result.ToString());

                MainForm.DgBody.ClearSelection();
                MainForm.DgBody.FirstDisplayedScrollingRowIndex = i;
                MainForm.DgBody.Rows[i].Selected = true;
            }
        }
    }
}