using System.Windows.Forms;
using dnEditor.Handlers;
using dnlib.DotNet;

namespace dnEditor.Forms
{
    public partial class PickReferenceForm : Form
    {
        private readonly string _reference;

        public PickReferenceForm(string reference)
        {
            InitializeComponent();
            treeView1.AfterExpand += TreeViewHandler.treeView1_AfterExpand;

            TreeViewHandler.LoadAssembly(treeView1, MainForm.CurrentAssembly.Assembly, true);

            _reference = reference;
            EditInstructionForm.SelectedReference = null;
        }

        private void btnSelect_Click(object sender, System.EventArgs e)
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
    }
}
