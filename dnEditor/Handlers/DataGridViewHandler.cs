using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    public static class DataGridViewHandler
    {
        private static int _currentRowIndex;
        private static int _currentSelectedRowIndex;

        private static int _currentVariableRowIndex;
        private static int _currentVariableSelectedRowIndex;

        private static MethodDef _currentMethod;

        public static void InitializeBody()
        {
            MainForm.DgBody.Columns.GetColumnFromText("Index").DefaultCellStyle.ForeColor = DefaultColors.IndexTextColor;
            MainForm.DgBody.Columns.GetColumnFromText("OpCode").DefaultCellStyle.ForeColor =
                DefaultColors.OpCodeTextColor;
            MainForm.DgBody.DefaultCellStyle.BackColor = DefaultColors.RowColor;

            MainForm.DgVariables.DefaultCellStyle.BackColor = DefaultColors.RowColor;
            MainForm.DgVariables.Columns.GetColumnFromText("Index").DefaultCellStyle.ForeColor =
                DefaultColors.IndexTextColor;
        }

        public static void ReadMethod(MethodDef method)
        {
            if (_currentMethod == method)
            {
                _currentRowIndex = MainForm.DgBody.FirstDisplayedScrollingRowIndex;

                if (MainForm.DgBody.SelectedRows.Count > 0)
                    _currentSelectedRowIndex = MainForm.DgBody.SelectedRows.TopmostRow().Index;

                _currentVariableRowIndex = MainForm.DgVariables.FirstDisplayedScrollingRowIndex;

                if (MainForm.DgVariables.SelectedRows.Count > 0)
                    _currentVariableSelectedRowIndex = MainForm.DgVariables.SelectedRows.TopmostRow().Index;
            }
            else
            {
                _currentMethod = method;

                _currentRowIndex = 0;
                _currentSelectedRowIndex = 0;

                _currentVariableRowIndex = 0;
                _currentVariableSelectedRowIndex = 0;
            }

            ClearInstructions();
            VariableHandler.ClearVariables();

            MainForm.CurrentAssembly.Method.NewMethod = method;

            if (!method.HasBody) return;

            ReadInstructions(method);
            VariableHandler.ReadVariables(method);
            ExceptionHandler.ReadExceptionHandlers(method);

            RestoreInstructionSelection();
            RestoreVariableSelection();
        }

        private static void ReadInstructions(MethodDef method)
        {
            if (!method.Body.HasInstructions) return;

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

            ColorRules.MarkBlocks(MainForm.DgBody);
            ColorRules.ApplyColors(MainForm.DgBody);
        }

        public static void ClearInstructions()
        {
            MainForm.DgBody.Rows.Clear();
        }

        private static void RestoreInstructionSelection()
        {
            if (MainForm.DgBody.RowCount < 1) return;

            if (_currentRowIndex > 0)
                if (MainForm.DgBody.Rows.Count <= _currentRowIndex)
                    MainForm.DgBody.FirstDisplayedScrollingRowIndex = MainForm.DgBody.RowCount - 1;
                else
                    MainForm.DgBody.FirstDisplayedScrollingRowIndex = _currentRowIndex;

            if (_currentSelectedRowIndex > 0)
            {
                MainForm.DgBody.ClearSelection();

                if (MainForm.DgBody.Rows.Count <= _currentSelectedRowIndex)
                    MainForm.DgBody.Rows[MainForm.DgBody.RowCount - 1].Selected = true;
                else
                    MainForm.DgBody.Rows[_currentSelectedRowIndex].Selected = true;
            }
        }

        private static void RestoreVariableSelection()
        {
            if (MainForm.DgVariables.RowCount < 1) return;

            if (_currentVariableRowIndex > 0)
                if (MainForm.DgVariables.Rows.Count <= _currentVariableRowIndex)
                    MainForm.DgVariables.FirstDisplayedScrollingRowIndex = MainForm.DgVariables.RowCount - 1;
                else
                    MainForm.DgVariables.FirstDisplayedScrollingRowIndex = _currentVariableRowIndex;

            if (_currentVariableSelectedRowIndex > 0)
            {
                MainForm.DgVariables.ClearSelection();

                if (MainForm.DgVariables.Rows.Count <= _currentVariableSelectedRowIndex)
                    MainForm.DgVariables.Rows[MainForm.DgVariables.RowCount - 1].Selected = true;
                else
                    MainForm.DgVariables.Rows[_currentVariableSelectedRowIndex].Selected = true;
            }
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

    internal static class VariableHandler
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

    internal static class ExceptionHandler
    {
        public static void ReadExceptionHandlers(MethodDef method)
        {
            if (method.Body.HasExceptionHandlers)
            {
                int i = 0;
                var rows = new List<DataGridViewRow>();

                foreach (dnlib.DotNet.Emit.ExceptionHandler exceptionHandler in method.Body.ExceptionHandlers)
                {
                    var cells = new List<object>();

                    cells.Add(string.Empty); // Column 1
                    cells.Add(string.Format(".try{0}", i++)); // Column 2

                    int tryStart = method.Body.Instructions.IndexOf(exceptionHandler.TryStart);
                    int tryEnd;

                    if (exceptionHandler.TryEnd != null)
                        tryEnd = method.Body.Instructions.IndexOf(exceptionHandler.TryEnd);
                    else
                        tryEnd = method.Body.Instructions.Count - 1;

                    cells.Add(string.Format("{0} to {1}", tryStart, tryEnd));
                        // Column 3

                    int handlerStart = method.Body.Instructions.IndexOf(exceptionHandler.HandlerStart);
                    int handlerEnd;

                    if (exceptionHandler.HandlerEnd != null)
                        handlerEnd = method.Body.Instructions.IndexOf(exceptionHandler.HandlerEnd);
                    else
                        handlerEnd = method.Body.Instructions.Count - 1;

                    switch (exceptionHandler.HandlerType)
                    {
                        case ExceptionHandlerType.Catch:
                            cells.Add(string.Format("Catch handler {0} to {1}", handlerStart, handlerEnd)); // Column 3

                            if (exceptionHandler.CatchType != null)
                                cells[3] += string.Format(" [{0}]", exceptionHandler.CatchType.FullName);

                            break;
                            case ExceptionHandlerType.Duplicated:
                            cells.Add(string.Format("Duplicated handler {0} to {1}", handlerStart, handlerEnd)); // Column 3
                            break;
                            case ExceptionHandlerType.Fault:
                            cells.Add(string.Format("Fault handler {0} to {1}", handlerStart, handlerEnd)); // Column 3
                            break;
                            case ExceptionHandlerType.Filter:
                            cells.Add(string.Format("Filter handler {0} to {1}", handlerStart, handlerEnd)); // Column 3
                            break;
                            case ExceptionHandlerType.Finally:
                            cells.Add(string.Format("Finally handler {0} to {1}", handlerStart, handlerEnd)); // Column 3
                            break;
                    }

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

                    row.Tag = exceptionHandler;
                    row.Height = 16;

                    row.ContextMenuStrip = MainForm.ExceptionHandlerMenu;

                    rows.Add(row);
                }

                MainForm.DgBody.Rows.AddRange(rows.ToArray());
            }
        }
    }
}