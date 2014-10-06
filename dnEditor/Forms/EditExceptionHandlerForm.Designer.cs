namespace dnEditor.Forms
{
    partial class EditExceptionHandlerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbHandlerType = new System.Windows.Forms.ComboBox();
            this.cbCatchType = new System.Windows.Forms.ComboBox();
            this.cbTryStart = new System.Windows.Forms.ComboBox();
            this.cbTryEnd = new System.Windows.Forms.ComboBox();
            this.cbHandlerStart = new System.Windows.Forms.ComboBox();
            this.cbHandlerEnd = new System.Windows.Forms.ComboBox();
            this.cbFilterStart = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCatchType = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Handler type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Catch type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Try start";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Try end";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Handler start";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Handler end";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Filter start";
            // 
            // cbHandlerType
            // 
            this.cbHandlerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHandlerType.FormattingEnabled = true;
            this.cbHandlerType.Items.AddRange(new object[] {
            "Catch",
            "Duplicated",
            "Fault",
            "Filter",
            "Finally"});
            this.cbHandlerType.Location = new System.Drawing.Point(84, 12);
            this.cbHandlerType.Name = "cbHandlerType";
            this.cbHandlerType.Size = new System.Drawing.Size(289, 21);
            this.cbHandlerType.TabIndex = 1;
            this.cbHandlerType.SelectedIndexChanged += new System.EventHandler(this.cbHandlerType_SelectedIndexChanged);
            // 
            // cbCatchType
            // 
            this.cbCatchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cbCatchType.Enabled = false;
            this.cbCatchType.FormattingEnabled = true;
            this.cbCatchType.Location = new System.Drawing.Point(84, 39);
            this.cbCatchType.Name = "cbCatchType";
            this.cbCatchType.Size = new System.Drawing.Size(289, 20);
            this.cbCatchType.TabIndex = 1;
            // 
            // cbTryStart
            // 
            this.cbTryStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTryStart.FormattingEnabled = true;
            this.cbTryStart.Location = new System.Drawing.Point(84, 66);
            this.cbTryStart.Name = "cbTryStart";
            this.cbTryStart.Size = new System.Drawing.Size(289, 21);
            this.cbTryStart.TabIndex = 1;
            // 
            // cbTryEnd
            // 
            this.cbTryEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTryEnd.FormattingEnabled = true;
            this.cbTryEnd.Location = new System.Drawing.Point(84, 93);
            this.cbTryEnd.Name = "cbTryEnd";
            this.cbTryEnd.Size = new System.Drawing.Size(289, 21);
            this.cbTryEnd.TabIndex = 1;
            // 
            // cbHandlerStart
            // 
            this.cbHandlerStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHandlerStart.FormattingEnabled = true;
            this.cbHandlerStart.Location = new System.Drawing.Point(84, 120);
            this.cbHandlerStart.Name = "cbHandlerStart";
            this.cbHandlerStart.Size = new System.Drawing.Size(289, 21);
            this.cbHandlerStart.TabIndex = 1;
            // 
            // cbHandlerEnd
            // 
            this.cbHandlerEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHandlerEnd.FormattingEnabled = true;
            this.cbHandlerEnd.Location = new System.Drawing.Point(84, 147);
            this.cbHandlerEnd.Name = "cbHandlerEnd";
            this.cbHandlerEnd.Size = new System.Drawing.Size(289, 21);
            this.cbHandlerEnd.TabIndex = 1;
            // 
            // cbFilterStart
            // 
            this.cbFilterStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterStart.Enabled = false;
            this.cbFilterStart.FormattingEnabled = true;
            this.cbFilterStart.Location = new System.Drawing.Point(84, 174);
            this.cbFilterStart.Name = "cbFilterStart";
            this.cbFilterStart.Size = new System.Drawing.Size(289, 21);
            this.cbFilterStart.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(130, 214);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(211, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCatchType
            // 
            this.btnCatchType.Enabled = false;
            this.btnCatchType.Location = new System.Drawing.Point(379, 32);
            this.btnCatchType.Name = "btnCatchType";
            this.btnCatchType.Size = new System.Drawing.Size(33, 21);
            this.btnCatchType.TabIndex = 5;
            this.btnCatchType.Text = "...";
            this.btnCatchType.UseVisualStyleBackColor = true;
            this.btnCatchType.Click += new System.EventHandler(this.btnCatchType_Click);
            // 
            // EditExceptionHandlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 247);
            this.Controls.Add(this.btnCatchType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbFilterStart);
            this.Controls.Add(this.cbHandlerEnd);
            this.Controls.Add(this.cbHandlerStart);
            this.Controls.Add(this.cbTryEnd);
            this.Controls.Add(this.cbTryStart);
            this.Controls.Add(this.cbCatchType);
            this.Controls.Add(this.cbHandlerType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EditExceptionHandlerForm";
            this.Text = "Edit Exception Handler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.form_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbHandlerType;
        private System.Windows.Forms.ComboBox cbCatchType;
        private System.Windows.Forms.ComboBox cbTryStart;
        private System.Windows.Forms.ComboBox cbTryEnd;
        private System.Windows.Forms.ComboBox cbHandlerStart;
        private System.Windows.Forms.ComboBox cbHandlerEnd;
        private System.Windows.Forms.ComboBox cbFilterStart;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCatchType;
    }
}