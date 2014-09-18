using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using dnEditor.Handlers;
using dnEditor.Misc;
using dnEditor.Properties;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public partial class MainForm : Form, ITreeView, ITreeMenu
    {
        public static DataGridView DgBody;
        public static CurrentAssembly CurrentAssembly;
        public static int EditedInstructionIndex;
        public static Instruction NewInstruction;
        public static TreeView TreeView;
        public static ToolStrip ToolStrip;
        public static ContextMenuStrip InsructionMenuStrip;
        public static ContextMenuStrip TreeMenuStrip;

        private readonly List<Instruction> _copiedInstructions = new List<Instruction>();

        private readonly TreeViewHandler _treeViewHandler;
        private EditInstructionMode _editInstructionMode;

        public MainForm()
        {
            InitializeComponent();
            _treeViewHandler = new TreeViewHandler(treeView1, treeMenu);

            DgBody = dgBody;
            TreeView = treeView1;
            ToolStrip = toolStrip1;
            InsructionMenuStrip = instructionMenu;
            TreeMenuStrip = treeMenu;
            txtMagicRegex.Text = Settings.Default.MagicRegex;

            InitializeBody();

            cbSearchType.SelectedIndex = 0;
        }

        private void InitializeBody()
        {
            DataGridViewHandler.InitializeBody();
        }

        private void EditInstructionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (NewInstruction == null) return;

            if (!CurrentAssembly.Method.NewMethod.HasBody)
                CurrentAssembly.Method.NewMethod.Body = new CilBody();

            switch (_editInstructionMode)
            {
                case EditInstructionMode.Add:
                    CurrentAssembly.Method.NewMethod.Body.Instructions.Add(NewInstruction);
                    break;
                case EditInstructionMode.Edit:
                    CurrentAssembly.Method.NewMethod.Body.Instructions[EditedInstructionIndex] = NewInstruction;
                    break;
                case EditInstructionMode.InsertAfter:
                    CurrentAssembly.Method.NewMethod.Body.Instructions.Insert(EditedInstructionIndex + 1, NewInstruction);
                    break;
                case EditInstructionMode.InsertBefore:
                    CurrentAssembly.Method.NewMethod.Body.Instructions.Insert(EditedInstructionIndex, NewInstruction);
                    break;
            }

            CurrentAssembly.Method.NewMethod.Body.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.NewMethod);
        }

        private void NewInstructionEditor(EditInstructionMode mode)
        {
            NewInstruction = null;
            _editInstructionMode = mode;
            var selectedRows = dgBody.SelectedRows;

            if (selectedRows.Count > 0)
                EditedInstructionIndex = selectedRows[0].Index;

            if (mode == EditInstructionMode.Edit)
            {
                var form =
                    new EditInstructionForm(
                        CurrentAssembly.Method.NewMethod.Body.Instructions[dgBody.SelectedRows[0].Index]);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearchType.Text.Trim() == "" || txtSearch.Text.Trim() == "") return;

            SearchType searchType;
            TreeNode searchNode;

            switch (cbSearchType.Text)
            {
                case "String":
                    searchType = SearchType.String;
                    searchNode = _treeViewHandler.CurrentMethod;
                    break;

                case "OpCode":
                    searchType = SearchType.OpCode;
                    searchNode = _treeViewHandler.CurrentMethod;
                    break;

                case "Operand":
                    searchType = SearchType.Operand;
                    searchNode = _treeViewHandler.CurrentMethod;
                    break;

                default:
                    searchType = SearchType.Any;
                    searchNode = _treeViewHandler.CurrentModule;
                    break;
            }

            if (searchNode == null) return;

            var searchHandler = new SearchHandler(searchNode, txtSearch.Text, searchType);
            searchHandler.SearchFinished += DataGridViewHandler.SearchFinished;
            searchHandler.Search();
        }

        private void txtMagicRegex_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.MagicRegex = txtMagicRegex.Text;
            Settings.Default.Save();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && _treeViewHandler.SelectedNode != null &&
                _treeViewHandler.SelectedNode.Tag is AssemblyDef)
            {
                closeToolStripMenuItem_Click(sender, e);
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
                Filter = ".NET Assemblies (*.exe;*.dll)|*.exe;*.dll"
            };

            if (dialog.ShowDialog() != DialogResult.OK || !File.Exists(dialog.FileName))
                return;

            Functions.OpenFile(_treeViewHandler, dialog.FileName, ref CurrentAssembly);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"dnEditor is a .NET assembly editor based on dnlib.

Coded, maintained and organized by ViRb3

GitHub project page: https://github.com/ViRb3/dnEditor

Copyright (C) 2014-2015 ViRb3
Licenses can be found in the root directory of the project.", "About dnEditor");
        }

        #endregion ToolStrip

        #region InstructionMenuStrip

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
                int instructionIndex = CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(row.Tag as Instruction);
                CurrentAssembly.Method.NewMethod.Body.Instructions.RemoveAt(instructionIndex);
                CurrentAssembly.Method.NewMethod.Body.Instructions.Insert(instructionIndex, OpCodes.Nop.ToInstruction());
            }

            CurrentAssembly.Method.NewMethod.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.NewMethod);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _copiedInstructions.Clear();

            foreach (DataGridViewRow selectedRow in dgBody.SelectedRows)
            {
                _copiedInstructions.Add(selectedRow.Tag as Instruction);
                CurrentAssembly.Method.NewMethod.Body.Instructions.RemoveAt(selectedRow.Index);
            }

            CurrentAssembly.Method.NewMethod.Body.Instructions.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.NewMethod);
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
                CurrentAssembly.Method.NewMethod.Body.Instructions.Insert(instructionIndex, instruction);
            }

            CurrentAssembly.Method.NewMethod.Body.Instructions.UpdateInstructionOffsets();
            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.NewMethod);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection collection = dgBody.SelectedRows;

            foreach (DataGridViewRow row in collection)
            {
                CurrentAssembly.Method.NewMethod.Body.Instructions.Remove(row.Tag as Instruction);
            }

            CurrentAssembly.Method.NewMethod.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.NewMethod);
        }

        #endregion InstructionMenuStrip

        #region TreeMenuStrip

        public void treeMenu_Opened(object sender, EventArgs e)
        {
            _treeViewHandler.treeMenu_Opened(sender, e);
        }

        public void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.expandToolStripMenuItem_Click(sender, e);
        }

        public void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.collapseToolStripMenuItem_Click(sender, e);
        }

        public void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.collapseAllToolStripMenuItem_Click(sender, e);
        }

        public void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.closeToolStripMenuItem_Click(sender, e, ref CurrentAssembly);
        }

        #endregion TreeMenuStrip

        #region EmptyInstructionsMenu

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInstructionEditor(EditInstructionMode.Add);
        }

        private void emptyBodyMenu_Opened(object sender, EventArgs e)
        {
            if (CurrentAssembly == null || CurrentAssembly.Method == null || CurrentAssembly.Method.NewMethod == null)
            {
                emptyBodyMenu.Items[0].Enabled = false;
                return;
            }

            emptyBodyMenu.Items[0].Enabled = true;
        }

        #endregion EmptyInstructionsMenu

        #region TreeView Events

        public void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            _treeViewHandler.treeView1_AfterExpand(sender, e);
        }

        public void treeView_DragDrop(object sender, DragEventArgs e)
        {
            string result = _treeViewHandler.DragDrop(sender, e);
            if (!string.IsNullOrEmpty(result))
            {
                Functions.OpenFile(_treeViewHandler, result, ref CurrentAssembly);
            }
        }

        public void treeView_DragEnter(object sender, DragEventArgs e)
        {
            _treeViewHandler.DragEnter(sender, e);
        }

        public void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeViewHandler.treeView_NodeMouseClick(sender, e, ref CurrentAssembly);
        }

        public void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeViewHandler.treeView_NodeMouseDoubleClick(sender, e, ref CurrentAssembly);
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

        private void dgBody_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                emptyBodyMenu.Show();
            }
        }

        #endregion DataGridView Events 
    }
}