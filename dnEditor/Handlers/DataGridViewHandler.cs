using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Handlers
{
    public static class DataGridViewHandler
    {
        private static readonly DataGridView DgBody = MainForm.DgBody;

        public static void InitializeBody()
        {
            DgBody.Columns["index"].DefaultCellStyle.ForeColor = Color.Blue;
            DgBody.Columns["opcode"].DefaultCellStyle.ForeColor = Color.Green;
            DgBody.DefaultCellStyle.BackColor = ColorRules.DefaultColor;
        }

        public static void ReadMethod(MethodDef method)
        {
            if (!method.HasBody || !method.Body.HasInstructions) return;

            DgBody.Rows.Clear();

            int i = 0;
            foreach (Instruction instruction in method.Body.Instructions)
            {
                var row = new List<object>();

                row.Add(i++);
                row.Add(Functions.GetAddress(instruction));
                row.Add(instruction.OpCode);

                if (instruction.Operand == null)
                {
                    row.Add(String.Empty);
                }
                else if ((instruction.OpCode.FlowControl == FlowControl.Cond_Branch ||
                          instruction.OpCode.FlowControl == FlowControl.Branch) && instruction.OpCode != OpCodes.Switch &&
                         instruction.Operand is Instruction)
                {
                    var jumpInstruction = instruction.Operand as Instruction;
                    row.Add(string.Format("{0} -> {1} {2}",
                        method.Body.Instructions.IndexOf(jumpInstruction), jumpInstruction.OpCode,
                        jumpInstruction.Operand));
                }
                else row.Add(instruction.Operand);

                for (int j = 0; j < row.Count; j++)
                {
                    row[j] = "   " + row[j];
                }

                DgBody.Rows.Add(row.ToArray());
            }

            MainForm.CurrentAssembly.Method.Method = method;
            ColorRules.MarkBlocks(DgBody);
            ColorRules.ApplyColors(DgBody);
        }

        public static void InsertRow(Instruction instruction, int index)
        {
                var row = new List<object>();

                row.Add(index);
                row.Add(Functions.GetAddress(instruction));
                row.Add(instruction.OpCode);

                if (instruction.Operand == null)
                {
                    row.Add(String.Empty);
                }
                else if ((instruction.OpCode.FlowControl == FlowControl.Cond_Branch ||
                          instruction.OpCode.FlowControl == FlowControl.Branch) && instruction.OpCode != OpCodes.Switch &&
                         instruction.Operand is Instruction)
                {
                    var jumpInstruction = instruction.Operand as Instruction;
                    row.Add(string.Format("{0} -> {1} {2}",
                        MainForm.CurrentAssembly.Method.Method.Body.Instructions.IndexOf(jumpInstruction), jumpInstruction.OpCode,
                        jumpInstruction.Operand));
                }
                else row.Add(instruction.Operand);

                for (int j = 0; j < row.Count; j++)
                {
                    row[j] = "   " + row[j];
                }

                DgBody.Rows.Insert(index, row.ToArray());
        }
    }
}