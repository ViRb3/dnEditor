using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.Utils;

namespace dnEditor.Forms
{
    public partial class EditInstructionForm : Form
    {
        public static object SelectedReference;
        private readonly List<object> _addedOperands = new List<object>();
        private bool _enableOperandTypeChangedEvent = true;

        public EditInstructionForm(Instruction instruction)
        {
            InitializeComponent();
            cbOperandType.SelectedIndex = 0;

            ListOpCodes(cbOpCode, instruction);
        }

        private void ListOpCodes(ComboBox comboBox, Instruction inputInstruction)
        {
            foreach (OpCode opCode in Functions.OpCodes)
                comboBox.Items.Add(opCode);

            if (inputInstruction != null && inputInstruction.OpCode != null)
                RestoreInstruction(inputInstruction);
        }

        private void RestoreInstruction(Instruction instruction)
        {
            OpCode opCode = instruction.OpCode;
            OpCode item = cbOpCode.Items.Cast<OpCode>().FirstOrDefault(i => i.Name.ToLower() == opCode.Name.ToLower());

            if (item != null)
            {
                cbOpCode.SelectedIndex = cbOpCode.Items.IndexOf(item);
            }

            _enableOperandTypeChangedEvent = false;

            switch (opCode.OperandType)
            {
                case OperandType.InlineBrTarget:
                    cbOperandType.SelectedIndex = 9;
                    InstructionReference();
                    cbOperand.SelectedIndex =
                        _addedOperands.Cast<Instruction>().ToList().IndexOf(instruction.Operand as Instruction);
                    break;
                case OperandType.ShortInlineBrTarget:
                    cbOperandType.SelectedIndex = 9;
                    InstructionReference();
                    cbOperand.SelectedIndex =
                        _addedOperands.Cast<Instruction>().ToList().IndexOf(instruction.Operand as Instruction);
                    break;
                case OperandType.InlineString:
                    cbOperandType.SelectedIndex = 7;
                    cbOperand.Enabled = true;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    cbOperand.Text = instruction.Operand.ToString();
                    break;
                case OperandType.InlineNone:
                    cbOperandType.SelectedIndex = 0;
                    break;
                case OperandType.InlineI:
                    //TODO: Implement
                case OperandType.InlineI8:
                    //TODO: Implement
                case OperandType.ShortInlineI:
                    //TODO: Implement
                case OperandType.InlineR:
                    //TODO: Implement
                case OperandType.ShortInlineR:
                    //TODO: Implement
                case OperandType.InlineSwitch:
                    //TODO: Implement
                    break;
                case OperandType.InlineVar:
                    cbOperandType.SelectedIndex = 11;
                    VariableReference();
                    cbOperand.SelectedIndex = _addedOperands.Cast<Local>()
                        .ToList()
                        .IndexOf(instruction.Operand as Local);
                    break;
                case OperandType.ShortInlineVar:
                    cbOperandType.SelectedIndex = 11;
                    VariableReference();
                    cbOperand.SelectedIndex = _addedOperands.Cast<Local>()
                        .ToList()
                        .IndexOf(instruction.Operand as Local);
                    break;
                case OperandType.InlineField:
                    cbOperandType.SelectedIndex = 13;
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference =
                        MainForm.CurrentAssembly.Assembly.ManifestModule.Import(instruction.Operand as IField);
                    cbOperand.Items.Add(SelectedReference as IField);
                    cbOperand.SelectedIndex = 0;
                    break;
                case OperandType.InlineMethod:
                    cbOperandType.SelectedIndex = 14;
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference =
                        MainForm.CurrentAssembly.Assembly.ManifestModule.Import(instruction.Operand as IMethod);
                    cbOperand.Items.Add(SelectedReference as IMethod);
                    cbOperand.SelectedIndex = 0;
                    break;
                case OperandType.InlineType:
                    cbOperandType.SelectedIndex = 15;
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference =
                        MainForm.CurrentAssembly.Assembly.ManifestModule.Import(instruction.Operand as IType);
                    cbOperand.Items.Add(SelectedReference as ITypeDefOrRef);
                    cbOperand.SelectedIndex = 0;
                    break;
            }

            _enableOperandTypeChangedEvent = true;
        }

        private void cbOpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = cbOpCode.SelectedItem as OpCode;
            string definition = Functions.GetOpCodeDefinition(result.Name);

            lblOpCodeDescription.Text = definition;

            lblFlowControl.Text = result.FlowControl.ToString();
            lblOpCodeType.Text = result.OpCodeType.ToString();
            lblOperandType.Text = result.OperandType.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbOperandType.SelectedIndex == 0) // None
                {
                    #region None

                    MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction();

                    #endregion None
                }
                else if (cbOperandType.SelectedIndex > 0 && cbOperandType.SelectedIndex < 9)
                    // Byte, SByte, Int32, Int64, Single, Double, String, Verbatim String
                {
                    #region Value

                    ValueReference();

                    #endregion Value
                }
                else if (cbOperandType.SelectedIndex == 9) // Instruction ref
                {
                    #region Instruction

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Body.Instructions[cbOperand.SelectedIndex]);

                    #endregion Instruction
                }
                else if (cbOperandType.SelectedIndex == 10) // Multi instructions ref
                {
                    #region Multi instructions

                    //TODO: Implement

                    #endregion Instruction
                }
                else if (cbOperandType.SelectedIndex == 11) // Variable ref
                {
                    #region Variable

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Body.Variables[cbOperand.SelectedIndex]);

                    #endregion Variable
                }
                else if (cbOperandType.SelectedIndex == 12) // Parameter ref
                {
                    #region Parameter

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Parameters[cbOperand.SelectedIndex]);

                    #endregion Parameter
                }
                else if (cbOperandType.SelectedIndex == 13) // Field ref
                {
                    #region Field

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as IField);

                    #endregion Field
                }
                else if (cbOperandType.SelectedIndex == 14) // Method ref
                {
                    #region Method

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as IMethod);

                    #endregion Method
                }

                else if (cbOperandType.SelectedIndex == 15) // Type ref
                {
                    #region Type

                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as ITypeDefOrRef);

                    #endregion Type
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                                Environment.NewLine + ex.Message, "Error");
                return;
            }
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbOperandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_enableOperandTypeChangedEvent) return;

            cbOperand.Items.Clear();
            _addedOperands.Clear();

            if (cbOperandType.SelectedIndex == 0) // None
            {
                cbOperand.Enabled = false;
            }
            else if (cbOperandType.SelectedIndex > 0 && cbOperandType.SelectedIndex < 9) // Value
            {
                cbOperand.Enabled = true;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
            }
            else if (cbOperandType.SelectedIndex == 9) // Instruction ref
            {
                InstructionReference();
            }
            else if (cbOperandType.SelectedIndex == 10) // Multi instructions ref
            {
                var multiInstructionSelectForm =
                    new MultipleInstructionsSelectForm(
                        MainForm.CurrentAssembly.Method.Method.Body.Instructions.ToArray());

                multiInstructionSelectForm.Show();
            }
            else if (cbOperandType.SelectedIndex == 11) // Variable ref
            {
                VariableReference();
            }
            else if (cbOperandType.SelectedIndex == 12) // Parameter ref
            {
                ParameterReference();
            }
            else if (cbOperandType.SelectedIndex == 13) // Field ref
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                var form = new PickReferenceForm("Field");
                form.Show();
                form.FormClosed += form_FormClosedField;
            }
            else if (cbOperandType.SelectedIndex == 14) // Method ref
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                var form = new PickReferenceForm("Method");
                form.Show();
                form.FormClosed += form_FormClosedMethod;
            }

            else if (cbOperandType.SelectedIndex == 15) // Type ref
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                var form = new PickReferenceForm("Type");
                form.Show();
                form.FormClosed += form_FormClosedType;
            }
        }

        private void form_FormClosedField(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            SelectedReference =
                MainForm.CurrentAssembly.Assembly.ManifestModule.Import(SelectedReference as IField);
            cbOperand.Items.Add(SelectedReference as IField);

            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedMethod(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            SelectedReference =
                MainForm.CurrentAssembly.Assembly.ManifestModule.Import(SelectedReference as IMethod);
            cbOperand.Items.Add(SelectedReference as IMethod);

            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedType(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            SelectedReference =
                MainForm.CurrentAssembly.Assembly.ManifestModule.Import(SelectedReference as ITypeDefOrRef);
            cbOperand.Items.Add(SelectedReference as ITypeDefOrRef);

            cbOperand.SelectedIndex = 0;
        }

        private void InstructionReference()
        {
            cbOperand.Enabled = true;
            cbOperand.DropDownStyle = ComboBoxStyle.DropDownList;

            int i = 0;
            foreach (Instruction instruction in MainForm.CurrentAssembly.Method.Method.Body.Instructions)
            {
                _addedOperands.Add(instruction);
                cbOperand.Items.Add(string.Format("{0} -> {1} {2}", i++, instruction.OpCode, instruction.Operand));
            }
        }

        private void ParameterReference()
        {
            cbOperand.Enabled = true;
            cbOperand.DropDownStyle = ComboBoxStyle.DropDownList;

            int i = 0;
            foreach (Parameter parameter in MainForm.CurrentAssembly.Method.Method.Parameters)
            {
                _addedOperands.Add(parameter);

                string name = parameter.Name;

                if (string.IsNullOrEmpty(name))
                    name = "PARAM_" + i++;

                cbOperand.Items.Add(string.Format("{0} : {1}", name, parameter.Type.GetExtendedName()));
            }
        }

        private void VariableReference()
        {
            cbOperand.Enabled = true;
            cbOperand.DropDownStyle = ComboBoxStyle.DropDownList;

            int i = 0;
            foreach (Local variable in MainForm.CurrentAssembly.Method.Method.Body.Variables)
            {
                _addedOperands.Add(variable);

                string name = variable.Name;

                if (string.IsNullOrEmpty(name))
                    name = "VAR_" + i++;

                cbOperand.Items.Add(string.Format("{0} : {1}", name, variable.Type.GetExtendedName()));
            }
        }

        private void ValueReference()
        {
            switch (cbOperandType.Text)
            {
                case "Byte":
                {
                    MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(byte.Parse(cbOperand.Text));
                    break;
                }
                case "SByte":
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(sbyte.Parse(cbOperand.Text));
                    break;
                }
                case "Int32":
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(Int32.Parse(cbOperand.Text));
                    break;
                }
                case "Int64":
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(Int64.Parse(cbOperand.Text));
                    break;
                }
                case "Single":
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(Single.Parse(cbOperand.Text));
                    break;
                }
                case "Double":
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(Double.Parse(cbOperand.Text));
                    break;
                }
                case "String":
                {
                    MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.Text);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}