using System;
using System.Collections.Generic;
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

        public EditInstructionForm(OpCode opCode = null)
        {
            InitializeComponent();
            ListOpCodes(cbOpCode, opCode);
            cbOperandType.SelectedIndex = 0;
        }

        public EditInstructionForm(string opCode = null)
        {
            InitializeComponent();
            ListOpCodes(cbOpCode, opCode);
            cbOperandType.SelectedIndex = 0;
        }

        private void ListOpCodes(ComboBox comboBox, OpCode inputOpCode = null)
        {
            foreach (OpCode opCode in Functions.OpCodes)
                comboBox.Items.Add(opCode);

            if (inputOpCode != null)
                comboBox.SelectedItem = inputOpCode;
        }

        private void ListOpCodes(ComboBox comboBox, string inputOpCode = null)
        {
            foreach (OpCode opCode in Functions.OpCodes)
                comboBox.Items.Add(opCode);

            if (inputOpCode != null && Functions.GetOpCode(inputOpCode) != null)
                comboBox.SelectedItem = Functions.GetOpCode(inputOpCode);
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
            if (cbOperandType.SelectedIndex == 0) // None
            {
                #region None
                try
                {
                    MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion None
            }
            else if (cbOperandType.SelectedIndex > 0 && cbOperandType.SelectedIndex < 9) 
                // Byte, SByte, Int32, Int64, Single, Double, String, Verbatim String
            {
                #region Value

                try
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
                                MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(sbyte.Parse(cbOperand.Text));
                                break;
                            }
                        case "Int32":
                            {
                                MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(Int32.Parse(cbOperand.Text));
                                break;
                            }
                        case "Int64":
                            {
                                MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(Int64.Parse(cbOperand.Text));
                                break;
                            }
                        case "Single":
                            {
                                MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(Single.Parse(cbOperand.Text));
                                break;
                            }
                        case "Double":
                            {
                                MainForm.NewInstruction = (cbOpCode.SelectedItem as OpCode).ToInstruction(Double.Parse(cbOperand.Text));
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
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Value
            }
            else if (cbOperandType.SelectedIndex == 9) // Instruction ref
            {
                #region Instruction
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Body.Instructions[cbOperand.SelectedIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Instruction
            }
            else if (cbOperandType.SelectedIndex == 10) // Variable ref
            {
                #region Variable
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Body.Variables[cbOperand.SelectedIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Variable
            }
            else if (cbOperandType.SelectedIndex == 11) // Parameter ref
            {
                #region Parameter
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(
                            MainForm.CurrentAssembly.Method.Method.Parameters[cbOperand.SelectedIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Parameter
            }
            else if (cbOperandType.SelectedIndex == 12) // Field ref
            {
                #region Field
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as FieldDef);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Field
            }
            else if (cbOperandType.SelectedIndex == 13) // Method ref
            {
                #region Method
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as MethodDef);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Method
            }

            else if (cbOperandType.SelectedIndex == 14) // Type ref
            {
                #region Type
                try
                {
                    MainForm.NewInstruction =
                        (cbOpCode.SelectedItem as OpCode).ToInstruction(cbOperand.SelectedItem as TypeDef);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create instruction!" + Environment.NewLine +
                        Environment.NewLine + ex.Message, "Error");
                    return;
                }
                #endregion Type
            }

            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbOperandType_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            else if (cbOperandType.SelectedIndex == 10) // Variable ref
            {
                VariableReference();
            }
            else if (cbOperandType.SelectedIndex == 11) // Parameter ref
            {
                ParameterReference();
            }
            else if (cbOperandType.SelectedIndex == 12) // Field ref
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                var form = new PickReferenceForm("Field");
                form.Show();
                form.FormClosed += form_FormClosedField;
            }
            else if (cbOperandType.SelectedIndex == 13) // Method ref
            {
                cbOperand.Enabled = false;
                cbOperand.DropDownStyle = ComboBoxStyle.Simple;
                var form = new PickReferenceForm("Method");
                form.Show();
                form.FormClosed += form_FormClosedMethod;
            }

            else if (cbOperandType.SelectedIndex == 14) // Type ref
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
            
            cbOperand.Items.Add(SelectedReference as FieldDef);
            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedMethod(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Add(SelectedReference as MethodDef);
            cbOperand.SelectedIndex = 0;
        }

        private void form_FormClosedType(object sender, FormClosedEventArgs e)
        {
            if (SelectedReference == null) return;

            cbOperand.Items.Add(SelectedReference as TypeDef);
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
    }
}