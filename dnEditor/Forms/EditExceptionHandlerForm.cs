using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Forms
{
    public enum EditExceptionHandlerMode
    {
        Add,
        Edit
    }

    public partial class EditExceptionHandlerForm : Form
    {
        public static object SelectedReference;
        private readonly ExceptionHandler _exceptionHandler;

        public EditExceptionHandlerForm()
        {
            InitializeComponent();
            cbHandlerType.SelectedIndex = 0;

            ProcessComboBoxes();
        }

        public EditExceptionHandlerForm(ExceptionHandler exceptionHandler)
        {
            InitializeComponent();
            cbHandlerType.SelectedIndex = 0;

            _exceptionHandler = exceptionHandler;

            ProcessComboBoxes();
            RestoreExceptionHandler();
        }

        private void ProcessComboBoxes()
        {
            ListInstructions(cbTryStart);
            ListInstructions(cbTryEnd);
            ListInstructions(cbHandlerStart);
            ListInstructions(cbHandlerEnd);
            ListInstructions(cbFilterStart);
        }

        private void ListInstructions(ComboBox comboBox)
        {
            List<Instruction> instructions = MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToList();

            for (int i = 0; i < instructions.Count; i++)
            {
                comboBox.Items.Add(Functions.FormatFullInstruction(instructions, i));
            }
        }

        private void RestoreExceptionHandler()
        {
            switch (_exceptionHandler.HandlerType)
            {
                case ExceptionHandlerType.Catch:
                    cbHandlerType.SelectedItem = cbHandlerType.GetItemByText("Catch");
                    break;
                case ExceptionHandlerType.Duplicated:
                    cbHandlerType.SelectedItem = cbHandlerType.GetItemByText("Duplicated");
                    break;
                case ExceptionHandlerType.Fault:
                    cbHandlerType.SelectedItem = cbHandlerType.GetItemByText("Fault");
                    break;
                case ExceptionHandlerType.Filter:
                    cbHandlerType.SelectedItem = cbHandlerType.GetItemByText("Filter");
                    break;
                case ExceptionHandlerType.Finally:
                    cbHandlerType.SelectedItem = cbHandlerType.GetItemByText("Finally");
                    break;
            }

            cbTryStart.SelectedIndex =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(_exceptionHandler.TryStart);

            if (_exceptionHandler.TryEnd == null)
            {
                cbTryEnd.SelectedIndex = cbTryEnd.Items.Count - 1;
            }
            else
            {
                cbTryEnd.SelectedIndex =
                    MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(_exceptionHandler.TryEnd);
            }

            cbHandlerStart.SelectedIndex =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(_exceptionHandler.HandlerStart);

            if (_exceptionHandler.HandlerEnd == null)
            {
                cbHandlerEnd.SelectedIndex = cbHandlerEnd.Items.Count - 1;
            }
            else
            {
                cbHandlerEnd.SelectedIndex =
                    MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(_exceptionHandler.HandlerEnd);
            }

            if (_exceptionHandler.CatchType != null)
            {
                cbCatchType.Items.Add(_exceptionHandler.CatchType);
                cbCatchType.SelectedIndex = 0;
            }

            if (_exceptionHandler.FilterStart != null)
            {
                cbFilterStart.SelectedIndex =
                    MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.IndexOf(_exceptionHandler.FilterStart);
            }
        }

        private bool SelectionsOk()
        {
            if (cbTryStart.SelectedItem == null || cbTryEnd.SelectedItem == null ||
                cbHandlerStart.SelectedItem == null || cbHandlerEnd.SelectedItem == null)
                return false;

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!SelectionsOk()) return;

            var exceptionHandler = new ExceptionHandler();

            switch (cbHandlerType.Text)
            {
                case "Catch":
                    exceptionHandler.HandlerType = ExceptionHandlerType.Catch;
                    break;
                case "Duplicated":
                    exceptionHandler.HandlerType = ExceptionHandlerType.Duplicated;
                    break;
                case "Fault":
                    exceptionHandler.HandlerType = ExceptionHandlerType.Fault;
                    break;
                case "Filter":
                    exceptionHandler.HandlerType = ExceptionHandlerType.Filter;
                    break;
                case "Finally":
                    exceptionHandler.HandlerType = ExceptionHandlerType.Finally;
                    break;
            }

            exceptionHandler.TryStart =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbTryStart.SelectedIndex];
            exceptionHandler.TryEnd =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbTryEnd.SelectedIndex];
            exceptionHandler.HandlerStart =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbHandlerStart.SelectedIndex];
            exceptionHandler.HandlerEnd =
                MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbHandlerEnd.SelectedIndex];

            if (exceptionHandler.HandlerType == ExceptionHandlerType.Catch)
            {
                if (cbCatchType.SelectedItem == null)
                    return;

                exceptionHandler.CatchType = (ITypeDefOrRef) cbCatchType.SelectedItem;
            }
            else if (exceptionHandler.HandlerType == ExceptionHandlerType.Filter)
            {
                if (cbFilterStart.SelectedItem == null)
                    return;

                exceptionHandler.FilterStart =
                    MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbFilterStart.SelectedIndex];
            }

            MainForm.NewExceptionHandler = exceptionHandler;

            Close();
        }

        private void btnCatchType_Click(object sender, EventArgs e)
        {
            cbCatchType.Enabled = false;
            cbCatchType.DropDownStyle = ComboBoxStyle.Simple;
            var form = new PickReferenceForm("Type");
            form.FormClosed += form_FormClosed;
            form.ShowDialog();
        }

        private void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbCatchType.Items.Clear();

            SelectedReference =
                MainForm.CurrentAssembly.Assembly.ManifestModule.Import(SelectedReference as ITypeDefOrRef);
            cbCatchType.Items.Add(SelectedReference);

            cbCatchType.SelectedIndex = 0;
        }

        private void cbHandlerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFilterStart.Enabled = cbHandlerType.Text == "Filter";
            btnCatchType.Enabled = cbHandlerType.Text == "Catch";
        }
    }
}