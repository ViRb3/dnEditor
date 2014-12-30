using System;
using System.Windows.Forms;
using dnEditor.Handlers;
using dnEditor.Misc;
using dnlib.DotNet;

namespace dnEditor.Forms
{
    public partial class PickReferenceForm : Form, ITreeView, ITreeMenu
    {
        private readonly string _reference;
        private readonly TreeViewHandler _treeViewHandler;
        private CurrentAssembly _currentAssembly = MainForm.CurrentAssembly;

        public PickReferenceForm(string reference)
        {
            InitializeComponent();
            _treeViewHandler = new TreeViewHandler(treeView1, treeMenu);

            treeView1.AllowDrop = true;

            _reference = reference;

            //TODO: Add detection to avoid useless assignment
            EditInstructionForm.SelectedReference = null;
            EditVariableForm.SelectedReference = null;
            EditExceptionHandlerForm.SelectedReference = null;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //TODO: Add detection to remove avoid assignment
            EditInstructionForm.SelectedReference = treeView1.SelectedNode.Tag;
            EditVariableForm.SelectedReference = treeView1.SelectedNode.Tag;
            EditExceptionHandlerForm.SelectedReference = treeView1.SelectedNode.Tag;

            Close();
        }

        #region TreeView Events

        public void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeViewHandler.SelectedNode = null;
            TreeNode assemblyNode = e.Node.FirstParentNode();

            if (_currentAssembly == null || _currentAssembly.ManifestModule != assemblyNode.Tag as ModuleDefMD)
            {
                _currentAssembly = new CurrentAssembly(assemblyNode.Tag as ModuleDefMD)
                {
                    Path = assemblyNode.ToolTipText
                };
            }

            if (e.Node.Tag is MethodDef && _reference == "Method")
                btnSelect.Enabled = true;
            else if (e.Node.Tag is FieldDef && _reference == "Field")
                btnSelect.Enabled = true;
            else if (e.Node.Tag is TypeDef && _reference == "Type")
                btnSelect.Enabled = true;
            else
                btnSelect.Enabled = false;

            if (e.Button == MouseButtons.Right)
            {
                _treeViewHandler.SelectedNode = e.Node;
            }
        }

        public void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeViewHandler.treeView_NodeMouseDoubleClick(sender, e, ref _currentAssembly);
        }

        public void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            _treeViewHandler.treeView1_NodeMouseHover(sender, e);
        }

        public void treeView_DragDrop(object sender, DragEventArgs e)
        {
            string result = _treeViewHandler.DragDrop(sender, e);
            if (!string.IsNullOrEmpty(result))
            {
                Functions.OpenFile(_treeViewHandler, result, ref _currentAssembly);
            }
        }

        public void treeView_DragEnter(object sender, DragEventArgs e)
        {
            _treeViewHandler.DragEnter(sender, e);
        }

        public void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (MainForm.HandleExpand)
                VirtualNodeUtilities.ExpandHandler(e.Node, _treeViewHandler);
        }

        private void PickReferenceForm_Shown(object sender, EventArgs e)
        {
            Functions.OpenFile(_treeViewHandler, _currentAssembly.Path, ref _currentAssembly);
        }

        #endregion TreeView Events

        #region TreeMenuStrip

        public void treeMenu_Opened(object sender, EventArgs e)
        {
            _treeViewHandler.treeMenu_Opened(sender, e);
        }

        public void goToEntryPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.goToEntryPointToolStripMenuItem_Click(sender, e);
        }

        public void goToModuleCCtorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _treeViewHandler.goToModuleCCtorToolStripMenuItem_Click(sender, e);
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
            _treeViewHandler.closeToolStripMenuItem_Click(sender, e, ref _currentAssembly);
        }

        #endregion TreeMenuStrip
    }
}