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
    public enum EditInstructionMode
    {
        Add,
        Edit,
        InsertBefore,
        InsertAfter
    }

    public partial class EditInstructionForm : Form
    {
        public static object SelectedReference;
        private readonly List<object> _addedOperands = new List<object>();
        private bool _enableOperandTypeChangedEvent = true;

        public EditInstructionForm()
        {
            InitializeComponent();
            cbOperandType.SelectedIndex = 0;

            ListOpCodes(cbOpCode, null);
        }

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
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Instruction reference");
                    InstructionReference();
                    cbOperand.SelectedIndex =
                        _addedOperands.Cast<Instruction>().ToList().IndexOf(instruction.Operand as Instruction);
                    break;

                case OperandType.ShortInlineBrTarget:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Instruction reference");
                    InstructionReference();
                    cbOperand.SelectedIndex =
                        _addedOperands.Cast<Instruction>().ToList().IndexOf(instruction.Operand as Instruction);
                    break;

                case OperandType.InlineString:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("String");
                    cbOperand.Enabled = true;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    cbOperand.Text = instruction.Operand.ToString();
                    break;

                case OperandType.InlineNone:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("[None]");
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
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

                    #region Selected Item

                    if (instruction.Operand is byte)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("Byte");
                    }
                    else if (instruction.Operand is sbyte)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("SByte");
                    }
                    else if (instruction.Operand is int)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("Int32");
                    }
                    else if (instruction.Operand is Int64)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("Int64");
                    }
                    else if (instruction.Operand is Single)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("Single");
                    }
                    else if (instruction.Operand is double)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("Double");
                    }
                    else if (instruction.Operand is string)
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("String");
                    }

                    #endregion Selected Item

                    cbOperand.Enabled = true;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    List<Instruction> instructions = MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToList();
                    cbOperand.Text = Functions.GetOperandText(instructions, instructions.IndexOf(instruction));
                    break;

                case OperandType.InlineSwitch:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Multiple instructions reference");
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    List<Instruction> instructions2 = MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToList();
                    cbOperand.Text = Functions.GetOperandText(instructions2, instructions2.IndexOf(instruction));

                    if (instruction.Operand != null)
                        _addedOperands.AddRange((Instruction[]) instruction.Operand);

                    break;

                case OperandType.InlineVar:
                    if (opCode.Name.ToLower().StartsWith("ldarg"))
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Parameter reference");
                        ParameterReference();
                        cbOperand.SelectedIndex = _addedOperands.Cast<Parameter>()
                            .ToList()
                            .IndexOf(instruction.Operand as Parameter);
                    }
                    else
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Variable reference");
                        VariableReference();
                        cbOperand.SelectedIndex = _addedOperands.Cast<Local>()
                            .ToList()
                            .IndexOf(instruction.Operand as Local);
                    }
                    break;

                case OperandType.ShortInlineVar:
                    if (opCode.Name.ToLower().StartsWith("ldarg"))
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Parameter reference");
                        ParameterReference();
                        cbOperand.SelectedIndex = _addedOperands.Cast<Parameter>()
                            .ToList()
                            .IndexOf(instruction.Operand as Parameter);
                    }
                    else
                    {
                        cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Variable reference");
                        VariableReference();
                        cbOperand.SelectedIndex = _addedOperands.Cast<Local>()
                            .ToList()
                            .IndexOf(instruction.Operand as Local);
                    }
                    break;

                case OperandType.InlineField:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Field reference");
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference = instruction.Operand;
                    cbOperand.Items.Add(SelectedReference);
                    cbOperand.SelectedIndex = 0;
                    break;

                case OperandType.InlineMethod:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Method reference");
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference = instruction.Operand;
                    cbOperand.Items.Add(SelectedReference);
                    cbOperand.SelectedIndex = 0;
                    break;

                case OperandType.InlineType:
                    cbOperandType.SelectedItem = cbOperandType.GetItemByText("-> Type reference");
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    SelectedReference = instruction.Operand;
                    cbOperand.Items.Add(SelectedReference);
                    cbOperand.SelectedIndex = 0;
                    break;
            }

            _enableOperandTypeChangedEvent = true;
            CheckEnableButton();
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
                if (cbOperandType.SelectedItem.ToString() == "[None]")
                {
                    #region None

                    MainForm.NewInstruction = ((OpCode) cbOpCode.SelectedItem).ToInstruction();

                    #endregion None
                }
                else if (cbOperandType.SelectedItem.ToString() == "Byte" ||
                         cbOperandType.SelectedItem.ToString() == "SByte" ||
                         cbOperandType.SelectedItem.ToString() == "Int32" ||
                         cbOperandType.SelectedItem.ToString() == "Int64" ||
                         cbOperandType.SelectedItem.ToString() == "Single" ||
                         cbOperandType.SelectedItem.ToString() == "Double" ||
                         cbOperandType.SelectedItem.ToString() == "String")
                {
                    #region Value

                    CreateValueInstruction();

                    #endregion Value
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Instruction reference")
                {
                    #region Instruction

                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions[cbOperand.SelectedIndex]);

                    #endregion Instruction
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Multiple instructions reference")
                {
                    #region Multi instructions

                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(SelectedReference as Instruction[]);

                    #endregion Instruction
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Variable reference")
                {
                    #region Variable

                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.Method.NewMethod.Body.Variables[cbOperand.SelectedIndex]);

                    #endregion Variable
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Parameter reference")
                {
                    #region Parameter

                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.Method.NewMethod.Parameters[cbOperand.SelectedIndex]);

                    #endregion Parameter
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Field reference")
                {
                    #region Field

                    IField field = cbOperand.SelectedItem as IField;

                    if (field.DeclaringType.DefinitionAssembly != MainForm.CurrentAssembly.ManifestModule.Assembly ||
                                field.Module != MainForm.CurrentAssembly.ManifestModule)
                    {
                        MemberRefUser fieldRef = new MemberRefUser(field.Module, field.Name, field.FieldSig, 
                            field.DeclaringType);

                        MainForm.NewInstruction = ((OpCode)cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.ManifestModule.Import(fieldRef));
                    }
                    else
                    {
                        MainForm.NewInstruction = ((OpCode)cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.ManifestModule.ResolveField(field.Rid));
                    }
                    
                    #endregion Field
                }
                else if (cbOperandType.SelectedItem.ToString() == "-> Method reference")
                {
                    #region Method

                    IMethod method = cbOperand.SelectedItem as IMethod;

                    if (method.DeclaringType.DefinitionAssembly != MainForm.CurrentAssembly.ManifestModule.Assembly ||
                                method.Module != MainForm.CurrentAssembly.ManifestModule)
                    {
                        MemberRefUser methodRef = new MemberRefUser(method.Module, method.Name, method.MethodSig, 
                            method.DeclaringType);

                        MainForm.NewInstruction = ((OpCode)cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.ManifestModule.Import(methodRef));
                    }
                    else
                    {
                        MainForm.NewInstruction = ((OpCode)cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.ManifestModule.ResolveMethod(method.Rid));
                    }

                    #endregion Method
                }

                else if (cbOperandType.SelectedItem.ToString() == "-> Type reference")
                {
                    #region Type

                    ITypeDefOrRef type = cbOperand.SelectedItem as ITypeDefOrRef;

                    if (type.DefinitionAssembly != MainForm.CurrentAssembly.ManifestModule.Assembly ||
                                type.Module != MainForm.CurrentAssembly.ManifestModule)
                    {
                        TypeRefUser typeRef = new TypeRefUser(type.Module, type.Namespace, type.Name, 
                            MainForm.CurrentAssembly.ManifestModule.CorLibTypes.AssemblyRef);

                        MainForm.NewInstruction = ((OpCode) cbOpCode.SelectedItem).ToInstruction(
                        MainForm.CurrentAssembly.ManifestModule.Import(typeRef));
                    }
                    else
                    {
                        MainForm.NewInstruction = ((OpCode)cbOpCode.SelectedItem).ToInstruction(
                            MainForm.CurrentAssembly.ManifestModule.ResolveTypeDef(type.Rid));
                    }

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
            cbOperand.Text = "";
            _addedOperands.Clear();
            btnSelectOperand.Enabled = false;

            if (cbOperandType.SelectedItem.ToString() == "[None]")
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
            }
            else if (cbOperandType.SelectedItem.ToString() == "Byte" || cbOperandType.SelectedItem.ToString() == "SByte" ||
                     cbOperandType.SelectedItem.ToString() == "Int32" || cbOperandType.SelectedItem.ToString() == "Int64" ||
                     cbOperandType.SelectedItem.ToString() == "Single" ||
                     cbOperandType.SelectedItem.ToString() == "Double" ||
                     cbOperandType.SelectedItem.ToString() == "String")
            {
                cbOperand.Enabled = true;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Instruction reference")
            {
                InstructionReference();
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Multiple instructions reference")
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Variable reference")
            {
                VariableReference();
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Parameter reference")
            {
                ParameterReference();
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Field reference" ||
                     cbOperandType.SelectedItem.ToString() == "-> Method reference" ||
                     cbOperandType.SelectedItem.ToString() == "-> Type reference")
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
            }

            CheckEnableButton();
        }

        private void CheckEnableButton()
        {
            if (cbOperandType.SelectedItem.ToString() == "-> Multiple instructions reference")
            {
                btnSelectOperand.Enabled = true;
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Field reference")
            {
                btnSelectOperand.Enabled = true;
            }
            else if (cbOperandType.SelectedItem.ToString() == "-> Method reference")
            {
                btnSelectOperand.Enabled = true;
            }

            else if (cbOperandType.SelectedItem.ToString() == "-> Type reference")
            {
                btnSelectOperand.Enabled = true;
            }
        }

        private void form_FormClosedField(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Clear();
            cbOperand.Items.Add(SelectedReference);

            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedMethod(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Clear();
            cbOperand.Items.Add(SelectedReference);

            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedType(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Clear();
            cbOperand.Items.Add(SelectedReference);

            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedMultipleInstructions(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Clear();

            cbOperand.Items.Add(
                Functions.GetSwitchText(MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToList(),
                    ((Instruction[]) SelectedReference).ToList()));
            cbOperand.SelectedIndex = 0;
        }

        private void InstructionReference()
        {
            cbOperand.Enabled = true;
            cbOperand.DropDownStyle = ComboBoxStyle.DropDownList;

            List<Instruction> instructions = MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToList();

            for (int i = 0; i < instructions.Count; i++)
            {
                _addedOperands.Add(instructions[i]);
                cbOperand.Items.Add(Functions.FormatFullInstruction(instructions, i));
            }
        }

        private void ParameterReference()
        {
            cbOperand.Enabled = true;
            cbOperand.DropDownStyle = ComboBoxStyle.DropDownList;

            int i = 0;
            foreach (Parameter parameter in MainForm.CurrentAssembly.Method.NewMethod.Parameters)
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
            foreach (Local variable in MainForm.CurrentAssembly.Method.NewMethod.Body.Variables)
            {
                _addedOperands.Add(variable);

                string name = variable.Name;

                if (string.IsNullOrEmpty(name))
                    name = "VAR_" + i++;

                cbOperand.Items.Add(string.Format("{0} : {1}", name, variable.Type.GetExtendedName()));
            }
        }

        private void CreateValueInstruction()
        {
            switch (cbOperandType.Text)
            {
                case "Byte":
                {
                    MainForm.NewInstruction = ((OpCode) cbOpCode.SelectedItem).ToInstruction(byte.Parse(cbOperand.Text));
                    break;
                }
                case "SByte":
                {
                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(sbyte.Parse(cbOperand.Text));
                    break;
                }
                case "Int32":
                {
                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(Int32.Parse(cbOperand.Text));
                    break;
                }
                case "Int64":
                {
                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(Int64.Parse(cbOperand.Text));
                    break;
                }
                case "Single":
                {
                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(Single.Parse(cbOperand.Text));
                    break;
                }
                case "Double":
                {
                    MainForm.NewInstruction =
                        ((OpCode) cbOpCode.SelectedItem).ToInstruction(Double.Parse(cbOperand.Text));
                    break;
                }
                case "String":
                {
                    MainForm.NewInstruction = ((OpCode) cbOpCode.SelectedItem).ToInstruction(cbOperand.Text);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void btnSelectOperand_Click(object sender, EventArgs e)
        {
            switch (cbOperandType.Text)
            {
                case ("-> Multiple instructions reference"):
                    Instruction[] selectedInstructions = null;

                    if (_addedOperands.Count > 0)
                        selectedInstructions = _addedOperands.Cast<Instruction>().ToArray();

                    var form =
                        new MultipleInstructionsSelectForm(
                            MainForm.CurrentAssembly.Method.NewMethod.Body.Instructions.ToArray(), selectedInstructions);
                    form.FormClosed += form_FormClosedMultipleInstructions;
                    form.ShowDialog();
                    break;

                case ("-> Field reference"):
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    var form2 = new PickReferenceForm("Field");
                    form2.FormClosed += form_FormClosedField;
                    form2.ShowDialog();
                    break;

                case ("-> Method reference"):
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    var form3 = new PickReferenceForm("Method");
                    form3.FormClosed += form_FormClosedMethod;
                    form3.ShowDialog();
                    break;

                case ("-> Type reference"):
                    cbOperand.Enabled = false;
                    cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                    var form4 = new PickReferenceForm("Type");
                    form4.FormClosed += form_FormClosedType;
                    form4.ShowDialog();
                    break;
            }
        }
    }
}