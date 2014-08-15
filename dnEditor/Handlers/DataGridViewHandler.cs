using System;
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
        private static readonly DataGridView DgBody = MainForm.DgBody;

        public static void InitializeBody()
        {
            DgBody.Columns["index"].DefaultCellStyle.ForeColor = Color.Blue;
            DgBody.Columns["opcode"].DefaultCellStyle.ForeColor = Color.Green;
            DgBody.DefaultCellStyle.BackColor = ColorRules.DefaultColor;
        }

        public static void ReadMethod(MethodDef method)
        {
            DgBody.Rows.Clear();

            if (!method.HasBody || !method.Body.HasInstructions) return;

            int i = 0;
            foreach (Instruction instruction in method.Body.Instructions)
            {
                var row = new List<object>();

                row.Add(i++);
                row.Add(Functions.GetAddress(instruction));
                row.Add(instruction.OpCode);

                if (instruction.Operand is Instruction)
                    row.Add(string.Format("{0}", Functions.ResolveOperandInstructions(method.Body.Instructions.ToList(), method.Body.Instructions.IndexOf(instruction))));
                else 
                    row.Add(instruction.Operand);

                for (int j = 0; j < row.Count; j++)
                {
                    row[j] = "   " + row[j];
                }

                int index = DgBody.Rows.Add(row.ToArray());
                DgBody.Rows[index].Tag = instruction;
            }

            MainForm.CurrentAssembly.Method.Method = method;
            ColorRules.MarkBlocks(DgBody);
            ColorRules.ApplyColors(DgBody);

            foreach (DataGridViewRow row in DgBody.Rows)
            {
                string definition;
                Functions.OpCodeDictionary.TryGetValue(Functions.GetOpCode(row.Cells["opcode"].Value.ToString().Trim()),
                        out definition);

                if (definition != null)
                    row.Cells["opcode"].ToolTipText = definition;
            }
        }
    }
}