using System;
using System.Windows.Forms;
using dnEditor.Handlers;
using dnEditor.Misc;
using dnlib.DotNet;

namespace dnEditor.Forms
{
    public partial class PickReferenceForm : Form
    {
        private readonly string _reference;

        public PickReferenceForm(string reference)
        {
            InitializeComponent();
            treeView1.AllowDrop = true;

            _reference = reference;
            EditInstructionForm.SelectedReference = null;
        }

        private void PickReferenceForm_Shown(object sender, EventArgs e)
        {
            TreeViewHandler.LoadAssembly(treeView1, MainForm.CurrentAssembly.Assembly, true);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            EditInstructionForm.SelectedReference = treeView1.SelectedNode.Tag;
            Close();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is MethodDef && _reference == "Method")
                btnSelect.Enabled = true;
            else if (e.Node.Tag is FieldDef && _reference == "Field")
                btnSelect.Enabled = true;
            else if (e.Node.Tag is TypeDef && _reference == "Type")
                btnSelect.Enabled = true;
            else
                btnSelect.Enabled = false;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            CurrentAssembly result = TreeViewHandler.DragDrop(sender, e);
            if (result != null)
            {
                TreeViewHandler.LoadAssembly(treeView1, result.Assembly, false);
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            TreeViewHandler.DragEnter(sender, e);
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            VirtualNodeUtilities.ExpandHandler(e.Node);
        }
    }
}