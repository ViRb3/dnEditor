namespace dnEditor.Forms
{
    partial class EditVariableForm
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
            this.cbVariableType = new System.Windows.Forms.ComboBox();
            this.lblVariableName = new System.Windows.Forms.Label();
            this.lblVariableType = new System.Windows.Forms.Label();
            this.txtVariableName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTypeSpecification = new System.Windows.Forms.ComboBox();
            this.btnSelectOperand = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbVariableType
            // 
            this.cbVariableType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbVariableType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbVariableType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVariableType.FormattingEnabled = true;
            this.cbVariableType.Location = new System.Drawing.Point(89, 40);
            this.cbVariableType.Name = "cbVariableType";
            this.cbVariableType.Size = new System.Drawing.Size(301, 21);
            this.cbVariableType.TabIndex = 1;
            // 
            // lblVariableName
            // 
            this.lblVariableName.AutoSize = true;
            this.lblVariableName.Location = new System.Drawing.Point(11, 17);
            this.lblVariableName.Name = "lblVariableName";
            this.lblVariableName.Size = new System.Drawing.Size(35, 13);
            this.lblVariableName.TabIndex = 2;
            this.lblVariableName.Text = "Name";
            // 
            // lblVariableType
            // 
            this.lblVariableType.AutoSize = true;
            this.lblVariableType.Location = new System.Drawing.Point(11, 43);
            this.lblVariableType.Name = "lblVariableType";
            this.lblVariableType.Size = new System.Drawing.Size(72, 13);
            this.lblVariableType.TabIndex = 2;
            this.lblVariableType.Text = "Variable Type";
            // 
            // txtVariableName
            // 
            this.txtVariableName.Location = new System.Drawing.Point(52, 14);
            this.txtVariableName.Name = "txtVariableName";
            this.txtVariableName.Size = new System.Drawing.Size(380, 20);
            this.txtVariableName.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(202, 108);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(121, 108);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type specification";
            // 
            // cbTypeSpecification
            // 
            this.cbTypeSpecification.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbTypeSpecification.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbTypeSpecification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeSpecification.FormattingEnabled = true;
            this.cbTypeSpecification.Items.AddRange(new object[] {
            "None",
            "Array",
            "Multi-dimensional array",
            "Reference",
            "Pointer"});
            this.cbTypeSpecification.Location = new System.Drawing.Point(110, 67);
            this.cbTypeSpecification.Name = "cbTypeSpecification";
            this.cbTypeSpecification.Size = new System.Drawing.Size(191, 21);
            this.cbTypeSpecification.TabIndex = 1;
            // 
            // btnSelectOperand
            // 
            this.btnSelectOperand.Location = new System.Drawing.Point(399, 39);
            this.btnSelectOperand.Name = "btnSelectOperand";
            this.btnSelectOperand.Size = new System.Drawing.Size(33, 21);
            this.btnSelectOperand.TabIndex = 6;
            this.btnSelectOperand.Text = "...";
            this.btnSelectOperand.UseVisualStyleBackColor = true;
            this.btnSelectOperand.Click += new System.EventHandler(this.btnSelectOperand_Click);
            // 
            // EditVariableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 143);
            this.Controls.Add(this.btnSelectOperand);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtVariableName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblVariableType);
            this.Controls.Add(this.lblVariableName);
            this.Controls.Add(this.cbTypeSpecification);
            this.Controls.Add(this.cbVariableType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditVariableForm";
            this.Text = "Edit Variable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbVariableType;
        private System.Windows.Forms.Label lblVariableName;
        private System.Windows.Forms.Label lblVariableType;
        private System.Windows.Forms.TextBox txtVariableName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTypeSpecification;
        private System.Windows.Forms.Button btnSelectOperand;
    }
}