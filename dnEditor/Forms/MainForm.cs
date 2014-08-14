using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
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
        public static int EditedInstruction;
        public static Instruction NewInstruction;

        public MainForm()
        {
            InitializeComponent();

            DgBody = dgBody;
            InitializeBody();

            dgBody.RowTemplate.ContextMenuStrip = instructionMenu;
        }

        private void InitializeBody()
        {
            DataGridViewHandler.InitializeBody();
        }

        private void LoadAssembly(bool clear)
        {
            TreeViewHandler.LoadAssembly(treeView1, CurrentAssembly.Assembly, clear);
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            CurrentAssembly result = TreeViewHandler.DragDrop(sender, e);
            if (result != null)
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

        private void dgBody_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                NewInstruction = null;
                EditedInstruction = e.RowIndex;
                var form = new EditInstructionForm(dgBody.Rows[e.RowIndex].Cells[2].Value.ToString().Trim());
                form.Show();
                form.FormClosed += EditInstructionForm_FormClosed;
            }
        }

        private void EditInstructionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (NewInstruction == null) return;

            CurrentAssembly.Method.Method.Body.Instructions[EditedInstruction] = NewInstruction;
            CurrentAssembly.Method.Method.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Choose a location to write the new assembly...",
                Filter = "Executable Files (*.exe)|*.exe"
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
                Filter = "Executable Files (*.exe)|*.exe"
            };

            if (dialog.ShowDialog() != DialogResult.OK || !File.Exists(dialog.FileName))
                return;

            OpenFile(dialog.FileName);
        }

        private void OpenFile(string file, bool clear = false)
        {
            CurrentAssembly = new CurrentAssembly(file);
            LoadAssembly(clear);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!(e.Node.Tag is AssemblyRef)) return;

            var assemblyRef = e.Node.Tag as AssemblyRef;
            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();

            var paths = new List<string>
            {
                Path.Combine(Environment.CurrentDirectory, assemblyRef.Name + ".dll"),
                Path.Combine(Environment.CurrentDirectory, assemblyRef.Name + ".exe"),
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".exe"),
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".dll"),
            };

            int result = 0;

            if (File.Exists(paths[0]))
            {
                OpenFile(paths[0]);
                result++;
            }
            if (File.Exists(paths[1]))
            {
                OpenFile(paths[1]);
                result++;
                return;
            }

            if (File.Exists(paths[2]))
            {
                OpenFile(paths[2]);
                result++;
            }
            if (File.Exists(paths[3]))
            {
                OpenFile(paths[3]);
                result++;
            }

            if (result > 0) return;

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

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeViewHandler.treeView1_AfterExpand(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentAssembly.Method.Method.Body.Instructions.Remove(dgBody.SelectedRows[0].Tag as Instruction);
            CurrentAssembly.Method.Method.Body.UpdateInstructionOffsets();

            DataGridViewHandler.ReadMethod(CurrentAssembly.Method.Method);
        }

        private void instructionMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dgBody_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            foreach (DataGridViewRow row in dgBody.Rows)
                row.Selected = false;

            dgBody.Rows[e.RowIndex].Selected = true;

        }
    }
}