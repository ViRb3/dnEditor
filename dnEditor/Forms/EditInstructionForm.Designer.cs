namespace dnEditor.Forms
{
    partial class EditInstructionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbOpCode = new System.Windows.Forms.ComboBox();
            this.lblFlowControl = new System.Windows.Forms.Label();
            this.lblOpCodeType = new System.Windows.Forms.Label();
            this.lblOperandType = new System.Windows.Forms.Label();
            this.cbOperand = new System.Windows.Forms.ComboBox();
            this.lblOperand = new System.Windows.Forms.Label();
            this.lblOpCode = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblOpCodeDescription = new System.Windows.Forms.Label();
            this.cbOperandType = new System.Windows.Forms.ComboBox();
            this.lblOperandType2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbOpCode
            // 
            this.cbOpCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOpCode.FormattingEnabled = true;
            this.cbOpCode.Location = new System.Drawing.Point(66, 12);
            this.cbOpCode.Name = "cbOpCode";
            this.cbOpCode.Size = new System.Drawing.Size(169, 21);
            this.cbOpCode.TabIndex = 0;
            this.cbOpCode.SelectedIndexChanged += new System.EventHandler(this.cbOpCode_SelectedIndexChanged);
            // 
            // lblFlowControl
            // 
            this.lblFlowControl.AutoSize = true;
            this.lblFlowControl.Location = new System.Drawing.Point(105, 49);
            this.lblFlowControl.Name = "lblFlowControl";
            this.lblFlowControl.Size = new System.Drawing.Size(0, 13);
            this.lblFlowControl.TabIndex = 1;
            // 
            // lblOpCodeType
            // 
            this.lblOpCodeType.AutoSize = true;
            this.lblOpCodeType.Location = new System.Drawing.Point(105, 62);
            this.lblOpCodeType.Name = "lblOpCodeType";
            this.lblOpCodeType.Size = new System.Drawing.Size(0, 13);
            this.lblOpCodeType.TabIndex = 1;
            // 
            // lblOperandType
            // 
            this.lblOperandType.AutoSize = true;
            this.lblOperandType.Location = new System.Drawing.Point(105, 75);
            this.lblOperandType.Name = "lblOperandType";
            this.lblOperandType.Size = new System.Drawing.Size(0, 13);
            this.lblOperandType.TabIndex = 1;
            // 
            // cbOperand
            // 
            this.cbOperand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperand.DropDownWidth = 500;
            this.cbOperand.FormattingEnabled = true;
            this.cbOperand.Location = new System.Drawing.Point(68, 179);
            this.cbOperand.Name = "cbOperand";
            this.cbOperand.Size = new System.Drawing.Size(365, 21);
            this.cbOperand.TabIndex = 0;
            // 
            // lblOperand
            // 
            this.lblOperand.AutoSize = true;
            this.lblOperand.Location = new System.Drawing.Point(14, 182);
            this.lblOperand.Name = "lblOperand";
            this.lblOperand.Size = new System.Drawing.Size(48, 13);
            this.lblOperand.TabIndex = 1;
            this.lblOperand.Text = "Operand";
            // 
            // lblOpCode
            // 
            this.lblOpCode.AutoSize = true;
            this.lblOpCode.Location = new System.Drawing.Point(14, 15);
            this.lblOpCode.Name = "lblOpCode";
            this.lblOpCode.Size = new System.Drawing.Size(46, 13);
            this.lblOpCode.TabIndex = 1;
            this.lblOpCode.Text = "OpCode";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(138, 215);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(219, 215);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblOpCodeDescription
            // 
            this.lblOpCodeDescription.AutoSize = true;
            this.lblOpCodeDescription.Location = new System.Drawing.Point(95, 102);
            this.lblOpCodeDescription.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblOpCodeDescription.Name = "lblOpCodeDescription";
            this.lblOpCodeDescription.Size = new System.Drawing.Size(0, 13);
            this.lblOpCodeDescription.TabIndex = 3;
            // 
            // cbOperandType
            // 
            this.cbOperandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperandType.FormattingEnabled = true;
            this.cbOperandType.Items.AddRange(new object[] {
            "[None]",
            "Byte",
            "SByte",
            "Int32",
            "Int64",
            "Single",
            "Double",
            "String",
            "Verbatim String",
            "-> Instruction reference",
            "-> Multiple instructions reference",
            "-> Variable reference",
            "-> Parameter reference",
            "-> Field reference",
            "-> Method reference",
            "-> Type reference"});
            this.cbOperandType.Location = new System.Drawing.Point(91, 152);
            this.cbOperandType.Name = "cbOperandType";
            this.cbOperandType.Size = new System.Drawing.Size(342, 21);
            this.cbOperandType.TabIndex = 0;
            this.cbOperandType.SelectedIndexChanged += new System.EventHandler(this.cbOperandType_SelectedIndexChanged);
            // 
            // lblOperandType2
            // 
            this.lblOperandType2.AutoSize = true;
            this.lblOperandType2.Location = new System.Drawing.Point(14, 155);
            this.lblOperandType2.Name = "lblOperandType2";
            this.lblOperandType2.Size = new System.Drawing.Size(71, 13);
            this.lblOperandType2.TabIndex = 1;
            this.lblOperandType2.Text = "Operand type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "FlowControl:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "OpCodeType:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "OperandType:";
            // 
            // EditInstructionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 250);
            this.Controls.Add(this.lblOpCodeDescription);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblOperandType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblOpCodeType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFlowControl);
            this.Controls.Add(this.cbOpCode);
            this.Controls.Add(this.lblOperandType2);
            this.Controls.Add(this.lblOperand);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblOpCode);
            this.Controls.Add(this.cbOperandType);
            this.Controls.Add(this.cbOperand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EditInstructionForm";
            this.Text = "Edit Instruction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbOpCode;
        private System.Windows.Forms.Label lblFlowControl;
        private System.Windows.Forms.Label lblOpCodeType;
        private System.Windows.Forms.Label lblOperandType;
        private System.Windows.Forms.ComboBox cbOperand;
        private System.Windows.Forms.Label lblOperand;
        private System.Windows.Forms.Label lblOpCode;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblOpCodeDescription;
        private System.Windows.Forms.ComboBox cbOperandType;
        private System.Windows.Forms.Label lblOperandType2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}