using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public partial class MultipleInstructionsSelectForm : Form
    {
        public MultipleInstructionsSelectForm(Instruction[] instructions, Instruction[] selectedInstructions = null)
        {
            InitializeComponent();
            Initialize(instructions, selectedInstructions);
        }

        private void Initialize(IEnumerable<Instruction> instructions, IEnumerable<Instruction> selectedInstructions)
        {
            foreach (Instruction instruction in instructions)
            {
                string row = Functions.FormatInstruction(instructions.ToList(), instructions.ToList().IndexOf(instruction));

                var item = new DataGridViewRow();
                var cell = new DataGridViewTextBoxCell
                {
                    Value = row
                };

                item.Cells.Add(cell);
                item.Tag = instruction;
                rightGridView.Rows.Add(item);
            }

            if (selectedInstructions == null) return;

            foreach (Instruction selectedInstruction in selectedInstructions)
            {
                DataGridViewRow item =
                    rightGridView.Rows.Cast<DataGridViewRow>().First(r => r.Tag as Instruction == selectedInstruction);

                rightGridView.Rows.Remove(item);
                leftGridView.Rows.Add(item);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = rightGridView.SelectedRows[0];
            rightGridView.Rows.Remove(row);
            leftGridView.Rows.Add(row);
        }
    }
}
