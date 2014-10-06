using System.Collections.Generic;
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
        private static int _currentRowIndex;
        private static MethodDef _currentMethod;
        public static void InitializeBody()
        {
            MainForm.DgBody.Columns.GetColumnFromText("Index").DefaultCellStyle.ForeColor = DefaultColors.IndexTextColor;
            MainForm.DgBody.Columns.GetColumnFromText("OpCode").DefaultCellStyle.ForeColor = DefaultColors.OpCodeTextColor;
            MainForm.DgBody.DefaultCellStyle.BackColor = DefaultColors.RowColor;

            MainForm.DgVariables.DefaultCellStyle.BackColor = DefaultColors.RowColor;
            MainForm.DgVariables.Columns.GetColumnFromText("Index").DefaultCellStyle.ForeColor = DefaultColors.IndexTextColor;
        }

        public static void ReadMethod(MethodDef method)
        {
            if (_currentMethod == method)
                _currentRowIndex = MainForm.DgBody.FirstDisplayedScrollingRowIndex;
            else
            {
                _currentMethod = method;
                _currentRowIndex = 0;
            }

            MainForm.DgBody.Rows.Clear();
            MainForm.CurrentAssembly.Method.NewMethod = method;

            if (!method.HasBody || !method.Body.HasInstructions) return;

            int i = 0;
            var rows = new List<DataGridViewRow>();

            VariableHandler.ClearVariables();

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
                    if (cells[j] == null || string.IsNullOrWhiteSpace(cells[j].ToString()))
                        continue;

                    cells[j] = string.Format("   {0}", cells[j]);
                }

                var row = new DataGridViewRow();

                for (int j = 0; j < cells.Count; j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell());
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

                row.ContextMenuStrip = MainForm.InstructionMenuStrip;

                rows.Add(row);
            }

            MainForm.DgBody.Rows.AddRange(rows.ToArray());

            if (_currentRowIndex > 0)
                if (MainForm.DgBody.Rows.Count < _currentRowIndex)
                    MainForm.DgBody.FirstDisplayedScrollingRowIndex = MainForm.DgBody.RowCount - 1;
                else
                    MainForm.DgBody.FirstDisplayedScrollingRowIndex = _currentRowIndex;


            ColorRules.MarkBlocks(MainForm.DgBody);
            ColorRules.ApplyColors(MainForm.DgBody);

            VariableHandler.ReadVariables(method);
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

    static class VariableHandler
    {
        public static void ReadVariables(MethodDef method)
        {
            if (method.Body.HasVariables)
            {
                int i = 0;
                var rows = new List<DataGridViewRow>();

                foreach (Local local in method.Body.Variables)
                {
                    var cells = new List<object>();
                    cells.Add(i++);
                    cells.Add(local.Name);
                    cells.Add(local.Type.GetFullName());

                    for (int j = 0; j < cells.Count; j++)
                    {
                        if (cells[j] == null || string.IsNullOrWhiteSpace(cells[j].ToString()))
                            continue;

                        cells[j] = string.Format("   {0}", cells[j]);
                    }

                    var row = new DataGridViewRow();

                    for (int j = 0; j < cells.Count; j++)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell());
                        row.Cells[j].Value = cells[j];
                    }

                    row.Tag = local;
                    row.Height = 16;

                    rows.Add(row);
                    row.ContextMenuStrip = MainForm.VariableMenu;
                }

                MainForm.DgVariables.Rows.AddRange(rows.ToArray());
            }
        }

        public static void ClearVariables()
        {
            MainForm.DgVariables.Rows.Clear();
        }
    }
}