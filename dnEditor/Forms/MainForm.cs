using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using dnEditor.Handlers;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public partial class MainForm : Form
    {
        public static DataGridView DgBody;
        public static CurrentAssembly CurrentAssembly;
        public static int EditedInstructionIndex;
        public static Instruction NewInstruction;
        public static TreeView TreeView;
        public static ToolStrip ToolStrip;
        public new static ContextMenuStrip ContextMenuStrip;

        private readonly List<Instruction> _copiedInstructions = new List<Instruction>();
        private EditInstructionMode _editInstructionMode;

        public MainForm()
        {
            InitializeComponent();

            DgBody = dgBody;
            TreeView = treeView1;
            ToolStrip = toolStrip1;
            ContextMenuStrip = instructionMenu;
            InitializeBody();
        }

        private void InitializeBody()
        {
            DataGridViewHandler.InitializeBody();
        }

        private void LoadAssembly(bool clear)
        {
            TreeViewHandler.LoadAssembly(treeView1, CurrentAssembly.Assembly, clear);
        }

        #region TreeView Events

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            CurrentAssembly result = TreeViewHandler.DragDrop(sender, e);
            if (result != null && result.Assembly != null)
            {
                CurrentAssembly = result;
                LoadAssembly(false);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            TreeViewHandler.DragEnter(sender, e);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is MethodDef)
                DataGridViewHandler.ReadMethod(e.Node.Tag as MethodDef);
            else
                dgBody.Rows.Clear();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!(e.Node.Tag is AssemblyRef)) return;

            var assemblyRef = e.Node.Tag as AssemblyRef;
            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            string directory = Directory.GetParent(CurrentAssembly.Path).FullName;

            var paths = new List<string>
            {
                Path.Combine(directory, assemblyRef.Name + ".dll"),
                Path.Combine(directory, assemblyRef.Name + ".exe"),
            };

            var paths2 = new List<string>
            {
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".exe"),
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".dll"),
            };


            if (paths.Where(File.Exists).Count() == 1)
            {
                OpenFile(paths.First(File.Exists));
                return;
            }
            if (paths2.Where(File.Exists).Count() == 1)
            {
                OpenFile(paths2.First(File.Exists));
                return;
            }

            if (MessageBox.Show("Could not automatically find reference file. Browse for it?", "Error",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            var dialog = new OpenFileDialog
            {
                Title = string.Format("Browse for the reference \"{0}\"", assemblyRef.Name),
                Filter = "Executable Files (*.exe)|*.exe|Library Files (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() != DialogResult.OK && File.Exists(dialog.FileName))
                return;

            OpenFile(dialog.FileName);
        }

        #endregion TreeView Events

        #region DataGridView Events

        private void dgBody_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                NewInstructionEditor(EditInstructionMode.Edit);
            }
        }

        private void dgBody_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            foreach (DataGridViewRow selectedRow in dgBody.SelectedRows)
            {
                if (selectedRow.Index == e.RowIndex) return;
            }

            foreach (DataGridViewRow row in dgBody.Rows)
                row.Selected = false;

            dgBody.Rows[e.RowIndex].Selected = true;
        }

        #endregion DataGridView Events 

        private void EditInstructionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (NewInstruction == null) return;

            switch (_editInstructionMode)
            {
                case EditInstructionMode.Edit:
                    CurrentAssembly.Method.Method.Body.Instructions[EditedInstructionIndex] = NewInstruction;
                    break;
                case EditInstructionMode.InsertAfter:
                    CurrentAssembly.Method.Method.Body.Instructions.Insert(EditedInstructionIndex + 1, NewInstruction);
                    break;
                case EditInstructionMode.InsertBefore:
                    CurrentAssembly.Method.Method.Body.Instructions.Insert(EditedInstructionIndex, NewInstruction);
                    break;
            }

            CurrentAssembly.Method.Method.Body.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private bool OpenFile(string file, bool clear = false)
        {
            CurrentAssembly = new CurrentAssembly(file);
            if (CurrentAssembly.Assembly == null) return false;

            LoadAssembly(clear);
            return true;
        }        

        private void NewInstructionEditor(EditInstructionMode mode)
        {
            NewInstruction = null;
            _editInstructionMode = mode;
            EditedInstructionIndex = dgBody.SelectedRows[0].Index;

            if (mode == EditInstructionMode.Edit)
            {
                var form =
                    new EditInstructionForm(
                        CurrentAssembly.Method.Method.Body.Instructions[dgBody.SelectedRows[0].Index]);
                form.FormClosed += EditInstructionForm_FormClosed;
                form.ShowDialog();
            }
            else
            {
                var form = new EditInstructionForm();
                form.FormClosed += EditInstructionForm_FormClosed;
                form.ShowDialog();
            }
        }

        #region ToolStrip

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentAssembly == null) return;

            var dialog = new SaveFileDialog
            {
                Title = "Choose a location to write the new assembly...",
                Filter = "Executable Files (*.exe)|*.exe|DLL Files (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            CurrentAssembly.Assembly.Write(dialog.FileName);

            MessageBox.Show("Assembly written to:" + Environment.NewLine + dialog.FileName, "Success");
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose an assembly to open...",
                Filter = "Executable Files (*.exe)|*.exe|DLL Files (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() != DialogResult.OK || !File.Exists(dialog.FileName))
                return;

            OpenFile(dialog.FileName);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"dnEditor is a dotNET assembly editor based on dnlib.

Coded, maintained and organized by ViRb3

GitHub project page: https://github.com/ViRb3/dnEditor

Copyright (C) 2014-2015 ViRb3/darkunited (darkunited@hotmail.co.uk)
Licenses can be found in the root directory of the project.", "About dnEditor");
        }

        #endregion ToolStrip

        #region ContextToolStrip

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInstructionEditor(EditInstructionMode.Edit);
        }

        private void insertBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInstructionEditor(EditInstructionMode.InsertBefore);
        }

        private void insertAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInstructionEditor(EditInstructionMode.InsertAfter);
        }

        private void nopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection collection = dgBody.SelectedRows;

            foreach (DataGridViewRow row in collection)
            {
                int instructionIndex = CurrentAssembly.Method.Method.Body.Instructions.IndexOf(row.Tag as Instruction);
                CurrentAssembly.Method.Method.Body.Instructions.RemoveAt(instructionIndex);
                CurrentAssembly.Method.Method.Body.Instructions.Insert(instructionIndex, OpCodes.Nop.ToInstruction());
            }

            CurrentAssembly.Method.Method.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _copiedInstructions.Clear();

            foreach (DataGridViewRow selectedRow in dgBody.SelectedRows)
            {
                _copiedInstructions.Add(selectedRow.Tag as Instruction);
                CurrentAssembly.Method.Method.Body.Instructions.RemoveAt(selectedRow.Index);
            }

            CurrentAssembly.Method.Method.Body.Instructions.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _copiedInstructions.Clear();

            foreach (DataGridViewRow selectedRow in dgBody.SelectedRows)
            {
                _copiedInstructions.Add(selectedRow.Tag as Instruction);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int instructionIndex = dgBody.SelectedRows[0].Index + 1;

            foreach (Instruction instruction in _copiedInstructions)
            {
                CurrentAssembly.Method.Method.Body.Instructions.Insert(instructionIndex, instruction);
            }

            CurrentAssembly.Method.Method.Body.Instructions.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection collection = dgBody.SelectedRows;

            foreach (DataGridViewRow row in collection)
            {
                CurrentAssembly.Method.Method.Body.Instructions.Remove(row.Tag as Instruction);
            }

            CurrentAssembly.Method.Method.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        #endregion ContextToolStrip       

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeViewHandler.treeView1_AfterExpand(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearchType.Text.Trim() == "" || txtSearch.Text.Trim() == "") return;

            //TODO: Implement
        }

        private void txtSearchCase_Click(object sender, EventArgs e)
        {
            if (txtSearchCase.Text == "[X] Case sensitive")
            {
                txtSearchCase.Text = "[ ] Not case sensitive";
                txtSearchCase.Tag = 0;
            }
            else
            {
                txtSearchCase.Text = "[X] Case sensitive";
                txtSearchCase.Tag = 1;
            }
        }
    }
}