using System;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public enum EditVariableMode
    {
        Add,
        Edit,
        InsertBefore,
        InsertAfter
    }

    public partial class EditVariableForm : Form
    {
        public static object SelectedReference;

        public EditVariableForm(Local local)
        {
            InitializeComponent();
            cbTypeSpecification.SelectedIndex = 0;

            RestoreVariable(local);
        }

        public EditVariableForm()
        {
            InitializeComponent();
            cbTypeSpecification.SelectedIndex = 0;
        }

        private void RestoreVariable(Local local)
        {
            if (local.Name != null && !string.IsNullOrWhiteSpace(local.Name))
            {
                txtVariableName.Text = local.Name;
            }

            if (local.Type != null)
            {
                cbVariableType.Enabled = false;
                cbVariableType.DropDownStyle = ComboBoxStyle.Simple;
                SelectedReference = local.Type;
                cbVariableType.Items.Add(SelectedReference);
                cbVariableType.SelectedIndex = 0;

                if (local.Type.IsSZArray)
                    cbTypeSpecification.SelectItemByText("Array");
                else if (local.Type.IsArray)
                    cbTypeSpecification.SelectItemByText("Multi-dimensional array");
                else if (local.Type.IsByRef)
                    cbTypeSpecification.SelectItemByText("Reference");
                else if (local.Type.IsPointer)
                    cbTypeSpecification.SelectItemByText("Pointer");
                else
                    cbTypeSpecification.SelectItemByText("None");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var variable = new Local(cbVariableType.SelectedItem as TypeSig);

            if (variable.Type == null)
                return;

            while (variable.Type.Next != null &&
                   ((variable.Type.IsArray || variable.Type.IsByRef || variable.Type.IsPointer ||
                     variable.Type.IsSZArray)))
            {
                variable.Type = variable.Type.Next;
            }

            switch (cbTypeSpecification.SelectedItem.ToString())
            {
                case "Array":
                    variable.Type = new SZArraySig(variable.Type);
                    break;
                case "Multi-dimensional array":
                    variable.Type = new ArraySig(variable.Type);
                    break;
                case "Reference":
                    variable.Type = new ByRefSig(variable.Type);
                    break;
                case "Pointer":
                    variable.Type = new PtrSig(variable.Type);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(txtVariableName.Text))
            {
                variable.Name = txtVariableName.Text;
            }

            MainForm.NewVariable = variable;

            Close();
        }

        private void btnVariableType_Click(object sender, EventArgs e)
        {
            cbVariableType.Enabled = false;
            cbVariableType.DropDownStyle = ComboBoxStyle.Simple;
            var form = new PickReferenceForm("Type");
            form.FormClosed += form_FormClosed;
            form.ShowDialog();
        }

        private void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbVariableType.Items.Clear();

            SelectedReference =
                MainForm.CurrentAssembly.ManifestModule.Import((SelectedReference as ITypeDefOrRef).ToTypeSig());
            cbVariableType.Items.Add(SelectedReference);

            cbVariableType.SelectedIndex = 0;
        }
    }
}