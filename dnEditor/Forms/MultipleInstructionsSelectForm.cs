using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public partial class MultipleInstructionsSelectForm : Form
    {
        private void MultipleInstructionsSelectForm_Load(object sender, EventArgs e)
        {
            EditInstructionForm.SelectedReference = null;
        }

        public MultipleInstructionsSelectForm(Instruction[] instructions, Instruction[] selectedInstructions = null)
        {
            InitializeComponent();
            Initialize(instructions, selectedInstructions);
        }

        private DataGridViewRow NewInstructionRow(IEnumerable<Instruction> instructions, Instruction instruction)
        {
            string row = Functions.FormatFullInstruction(instructions.ToList(), instructions.ToList().IndexOf(instruction));

            var item = new DataGridViewRow();
            var cell = new DataGridViewTextBoxCell
            {
                Value = row
            };

            item.Cells.Add(cell);
            item.Tag = instruction;

            return item;
        }

        private DataGridViewRow NewInstructionRow(string value, Instruction instruction)
        {
            string row = value;

            var item = new DataGridViewRow();
            var cell = new DataGridViewTextBoxCell
            {
                Value = row
            };

            item.Cells.Add(cell);
            item.Tag = instruction;

            return item;
        }

        private void Initialize(IEnumerable<Instruction> instructions, IEnumerable<Instruction> selectedInstructions)
        {
            foreach (Instruction instruction in instructions)
            {
                rightGridView.Rows.Add(NewInstructionRow(instructions, instruction));
            }

            if (selectedInstructions == null) return;

            foreach (Instruction selectedInstruction in selectedInstructions)
            {
                DataGridViewRow item =
                    rightGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Tag as Instruction == selectedInstruction);

                if (item == null) return;

                leftGridView.Rows.Add(NewInstructionRow(item.Cells[0].Value.ToString(), item.Tag as Instruction));
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (rightGridView.SelectedRows.Count < 1) return;

            List<DataGridViewRow> rowList = rightGridView.SelectedRows.Cast<DataGridViewRow>().ToList();
            rowList.Reverse();

            foreach (DataGridViewRow selectedRow in rowList)
            {
                leftGridView.Rows.Add(NewInstructionRow(selectedRow.Cells[0].Value.ToString(),
                    selectedRow.Tag as Instruction));
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (leftGridView.SelectedRows.Count < 1) return;

            foreach (DataGridViewRow selectedRow in leftGridView.SelectedRows)
            {
                leftGridView.Rows.Remove(selectedRow);
            }
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            if (leftGridView.SelectedRows.Count < 1) return;

            DataGridViewRow selectedRow = leftGridView.SelectedRows[0];

            leftGridView.Rows.Remove(selectedRow);
            leftGridView.Rows.Insert(0, selectedRow);

            leftGridView.ClearSelection();
            leftGridView.Rows[0].Selected = true;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (leftGridView.SelectedRows.Count < 1) return;

            DataGridViewRow row = leftGridView.SelectedRows[0];
            int index = leftGridView.SelectedRows[0].Index;
            leftGridView.Rows.Remove(row);
            leftGridView.Rows.Insert(index - 1, row);

            leftGridView.ClearSelection();
            leftGridView.Rows[index - 1].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (leftGridView.SelectedRows.Count < 1) return;

            DataGridViewRow row = leftGridView.SelectedRows[0];
            int index = leftGridView.SelectedRows[0].Index;
            leftGridView.Rows.Remove(row);
            leftGridView.Rows.Insert(index + 1, row);

            leftGridView.ClearSelection();
            leftGridView.Rows[index + 1].Selected = true;
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            if (leftGridView.SelectedRows.Count < 1) return;

            DataGridViewRow selectedRow = leftGridView.SelectedRows[0];

            leftGridView.Rows.Remove(selectedRow);
            leftGridView.Rows.Insert(leftGridView.Rows.Count, selectedRow);

            leftGridView.ClearSelection();
            leftGridView.Rows[leftGridView.Rows.Count - 1].Selected = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Instruction[] instructions = leftGridView.Rows.Cast<DataGridViewRow>().Select(row => (row.Tag as Instruction)).ToArray();
            EditInstructionForm.SelectedReference = instructions;
            this.Close();
        }

        private void leftGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            leftGridView.Rows.RemoveAt(e.RowIndex);
        }

        private void rightGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = rightGridView.Rows[e.RowIndex];

                leftGridView.Rows.Add(NewInstructionRow(row.Cells[0].Value.ToString(),
                    row.Tag as Instruction));
        }
    }
}