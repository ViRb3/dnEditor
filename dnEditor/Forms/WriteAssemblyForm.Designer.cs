namespace dnEditor.Forms
{
    partial class WriteAssemblyForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.chkPreserveTypeRefRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveTypeDefRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveFieldRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveMethodRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveParamRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveMemberRefRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveStandAloneSigRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveEventRids = new System.Windows.Forms.CheckBox();
            this.chkPreservePropertyRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveTypeSpecRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveMethodSpecRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveAllMethodRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveRids = new System.Windows.Forms.CheckBox();
            this.chkPreserveStringOffsets = new System.Windows.Forms.CheckBox();
            this.chkPreserveUSOffsets = new System.Windows.Forms.CheckBox();
            this.chkPreserveBlobOffsets = new System.Windows.Forms.CheckBox();
            this.chkPreserveExtraSignatureData = new System.Windows.Forms.CheckBox();
            this.chkPreserveAll = new System.Windows.Forms.CheckBox();
            this.chkKeepOldMaxStack = new System.Windows.Forms.CheckBox();
            this.chkAlwaysCreateGuidHeap = new System.Windows.Forms.CheckBox();
            this.chkAlwaysCreateStringsHeap = new System.Windows.Forms.CheckBox();
            this.chkAlwaysCreateUSHeap = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkNoThrowInstanceLogger = new System.Windows.Forms.CheckBox();
            this.chkAlwaysCreateBlobHeap = new System.Windows.Forms.CheckBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(407, 80);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkPreserveTypeRefRids
            // 
            this.chkPreserveTypeRefRids.AutoSize = true;
            this.chkPreserveTypeRefRids.Location = new System.Drawing.Point(6, 19);
            this.chkPreserveTypeRefRids.Name = "chkPreserveTypeRefRids";
            this.chkPreserveTypeRefRids.Size = new System.Drawing.Size(136, 17);
            this.chkPreserveTypeRefRids.TabIndex = 1;
            this.chkPreserveTypeRefRids.Text = "Preserve TypeRef Rids";
            this.chkPreserveTypeRefRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveTypeDefRids
            // 
            this.chkPreserveTypeDefRids.AutoSize = true;
            this.chkPreserveTypeDefRids.Location = new System.Drawing.Point(6, 42);
            this.chkPreserveTypeDefRids.Name = "chkPreserveTypeDefRids";
            this.chkPreserveTypeDefRids.Size = new System.Drawing.Size(136, 17);
            this.chkPreserveTypeDefRids.TabIndex = 1;
            this.chkPreserveTypeDefRids.Text = "Preserve TypeDef Rids";
            this.chkPreserveTypeDefRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveFieldRids
            // 
            this.chkPreserveFieldRids.AutoSize = true;
            this.chkPreserveFieldRids.Location = new System.Drawing.Point(6, 65);
            this.chkPreserveFieldRids.Name = "chkPreserveFieldRids";
            this.chkPreserveFieldRids.Size = new System.Drawing.Size(117, 17);
            this.chkPreserveFieldRids.TabIndex = 1;
            this.chkPreserveFieldRids.Text = "Preserve Field Rids";
            this.chkPreserveFieldRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveMethodRids
            // 
            this.chkPreserveMethodRids.AutoSize = true;
            this.chkPreserveMethodRids.Location = new System.Drawing.Point(6, 88);
            this.chkPreserveMethodRids.Name = "chkPreserveMethodRids";
            this.chkPreserveMethodRids.Size = new System.Drawing.Size(131, 17);
            this.chkPreserveMethodRids.TabIndex = 1;
            this.chkPreserveMethodRids.Text = "Preserve Method Rids";
            this.chkPreserveMethodRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveParamRids
            // 
            this.chkPreserveParamRids.AutoSize = true;
            this.chkPreserveParamRids.Location = new System.Drawing.Point(6, 111);
            this.chkPreserveParamRids.Name = "chkPreserveParamRids";
            this.chkPreserveParamRids.Size = new System.Drawing.Size(125, 17);
            this.chkPreserveParamRids.TabIndex = 1;
            this.chkPreserveParamRids.Text = "Preserve Param Rids";
            this.chkPreserveParamRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveMemberRefRids
            // 
            this.chkPreserveMemberRefRids.AutoSize = true;
            this.chkPreserveMemberRefRids.Location = new System.Drawing.Point(6, 134);
            this.chkPreserveMemberRefRids.Name = "chkPreserveMemberRefRids";
            this.chkPreserveMemberRefRids.Size = new System.Drawing.Size(150, 17);
            this.chkPreserveMemberRefRids.TabIndex = 1;
            this.chkPreserveMemberRefRids.Text = "Preserve MemberRef Rids";
            this.chkPreserveMemberRefRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveStandAloneSigRids
            // 
            this.chkPreserveStandAloneSigRids.AutoSize = true;
            this.chkPreserveStandAloneSigRids.Location = new System.Drawing.Point(6, 157);
            this.chkPreserveStandAloneSigRids.Name = "chkPreserveStandAloneSigRids";
            this.chkPreserveStandAloneSigRids.Size = new System.Drawing.Size(165, 17);
            this.chkPreserveStandAloneSigRids.TabIndex = 1;
            this.chkPreserveStandAloneSigRids.Text = "Preserve StandAloneSig Rids";
            this.chkPreserveStandAloneSigRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveEventRids
            // 
            this.chkPreserveEventRids.AutoSize = true;
            this.chkPreserveEventRids.Location = new System.Drawing.Point(6, 180);
            this.chkPreserveEventRids.Name = "chkPreserveEventRids";
            this.chkPreserveEventRids.Size = new System.Drawing.Size(123, 17);
            this.chkPreserveEventRids.TabIndex = 1;
            this.chkPreserveEventRids.Text = "Preserve Event Rids";
            this.chkPreserveEventRids.UseVisualStyleBackColor = true;
            // 
            // chkPreservePropertyRids
            // 
            this.chkPreservePropertyRids.AutoSize = true;
            this.chkPreservePropertyRids.Location = new System.Drawing.Point(6, 203);
            this.chkPreservePropertyRids.Name = "chkPreservePropertyRids";
            this.chkPreservePropertyRids.Size = new System.Drawing.Size(134, 17);
            this.chkPreservePropertyRids.TabIndex = 1;
            this.chkPreservePropertyRids.Text = "Preserve Property Rids";
            this.chkPreservePropertyRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveTypeSpecRids
            // 
            this.chkPreserveTypeSpecRids.AutoSize = true;
            this.chkPreserveTypeSpecRids.Location = new System.Drawing.Point(6, 226);
            this.chkPreserveTypeSpecRids.Name = "chkPreserveTypeSpecRids";
            this.chkPreserveTypeSpecRids.Size = new System.Drawing.Size(144, 17);
            this.chkPreserveTypeSpecRids.TabIndex = 1;
            this.chkPreserveTypeSpecRids.Text = "Preserve TypeSpec Rids";
            this.chkPreserveTypeSpecRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveMethodSpecRids
            // 
            this.chkPreserveMethodSpecRids.AutoSize = true;
            this.chkPreserveMethodSpecRids.Location = new System.Drawing.Point(6, 249);
            this.chkPreserveMethodSpecRids.Name = "chkPreserveMethodSpecRids";
            this.chkPreserveMethodSpecRids.Size = new System.Drawing.Size(156, 17);
            this.chkPreserveMethodSpecRids.TabIndex = 1;
            this.chkPreserveMethodSpecRids.Text = "Preserve MethodSpec Rids";
            this.chkPreserveMethodSpecRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveAllMethodRids
            // 
            this.chkPreserveAllMethodRids.AutoSize = true;
            this.chkPreserveAllMethodRids.Location = new System.Drawing.Point(6, 295);
            this.chkPreserveAllMethodRids.Name = "chkPreserveAllMethodRids";
            this.chkPreserveAllMethodRids.Size = new System.Drawing.Size(145, 17);
            this.chkPreserveAllMethodRids.TabIndex = 1;
            this.chkPreserveAllMethodRids.Text = "Preserve All Method Rids";
            this.chkPreserveAllMethodRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveRids
            // 
            this.chkPreserveRids.AutoSize = true;
            this.chkPreserveRids.Location = new System.Drawing.Point(6, 318);
            this.chkPreserveRids.Name = "chkPreserveRids";
            this.chkPreserveRids.Size = new System.Drawing.Size(92, 17);
            this.chkPreserveRids.TabIndex = 1;
            this.chkPreserveRids.Text = "Preserve Rids";
            this.chkPreserveRids.UseVisualStyleBackColor = true;
            // 
            // chkPreserveStringOffsets
            // 
            this.chkPreserveStringOffsets.AutoSize = true;
            this.chkPreserveStringOffsets.Location = new System.Drawing.Point(6, 19);
            this.chkPreserveStringOffsets.Name = "chkPreserveStringOffsets";
            this.chkPreserveStringOffsets.Size = new System.Drawing.Size(134, 17);
            this.chkPreserveStringOffsets.TabIndex = 1;
            this.chkPreserveStringOffsets.Text = "Preserve String Offsets";
            this.chkPreserveStringOffsets.UseVisualStyleBackColor = true;
            // 
            // chkPreserveUSOffsets
            // 
            this.chkPreserveUSOffsets.AutoSize = true;
            this.chkPreserveUSOffsets.Location = new System.Drawing.Point(6, 42);
            this.chkPreserveUSOffsets.Name = "chkPreserveUSOffsets";
            this.chkPreserveUSOffsets.Size = new System.Drawing.Size(122, 17);
            this.chkPreserveUSOffsets.TabIndex = 1;
            this.chkPreserveUSOffsets.Text = "Preserve US Offsets";
            this.chkPreserveUSOffsets.UseVisualStyleBackColor = true;
            // 
            // chkPreserveBlobOffsets
            // 
            this.chkPreserveBlobOffsets.AutoSize = true;
            this.chkPreserveBlobOffsets.Location = new System.Drawing.Point(6, 65);
            this.chkPreserveBlobOffsets.Name = "chkPreserveBlobOffsets";
            this.chkPreserveBlobOffsets.Size = new System.Drawing.Size(128, 17);
            this.chkPreserveBlobOffsets.TabIndex = 1;
            this.chkPreserveBlobOffsets.Text = "Preserve Blob Offsets";
            this.chkPreserveBlobOffsets.UseVisualStyleBackColor = true;
            // 
            // chkPreserveExtraSignatureData
            // 
            this.chkPreserveExtraSignatureData.AutoSize = true;
            this.chkPreserveExtraSignatureData.Location = new System.Drawing.Point(6, 88);
            this.chkPreserveExtraSignatureData.Name = "chkPreserveExtraSignatureData";
            this.chkPreserveExtraSignatureData.Size = new System.Drawing.Size(169, 17);
            this.chkPreserveExtraSignatureData.TabIndex = 1;
            this.chkPreserveExtraSignatureData.Text = "Preserve Extra Signature Data";
            this.chkPreserveExtraSignatureData.UseVisualStyleBackColor = true;
            // 
            // chkPreserveAll
            // 
            this.chkPreserveAll.AutoSize = true;
            this.chkPreserveAll.Location = new System.Drawing.Point(6, 134);
            this.chkPreserveAll.Name = "chkPreserveAll";
            this.chkPreserveAll.Size = new System.Drawing.Size(82, 17);
            this.chkPreserveAll.TabIndex = 1;
            this.chkPreserveAll.Text = "Preserve All";
            this.chkPreserveAll.UseVisualStyleBackColor = true;
            // 
            // chkKeepOldMaxStack
            // 
            this.chkKeepOldMaxStack.AutoSize = true;
            this.chkKeepOldMaxStack.Location = new System.Drawing.Point(6, 180);
            this.chkKeepOldMaxStack.Name = "chkKeepOldMaxStack";
            this.chkKeepOldMaxStack.Size = new System.Drawing.Size(124, 17);
            this.chkKeepOldMaxStack.TabIndex = 1;
            this.chkKeepOldMaxStack.Text = "Keep Old Max Stack";
            this.chkKeepOldMaxStack.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysCreateGuidHeap
            // 
            this.chkAlwaysCreateGuidHeap.AutoSize = true;
            this.chkAlwaysCreateGuidHeap.Location = new System.Drawing.Point(6, 203);
            this.chkAlwaysCreateGuidHeap.Name = "chkAlwaysCreateGuidHeap";
            this.chkAlwaysCreateGuidHeap.Size = new System.Drawing.Size(147, 17);
            this.chkAlwaysCreateGuidHeap.TabIndex = 1;
            this.chkAlwaysCreateGuidHeap.Text = "Always Create Guid Heap";
            this.chkAlwaysCreateGuidHeap.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysCreateStringsHeap
            // 
            this.chkAlwaysCreateStringsHeap.AutoSize = true;
            this.chkAlwaysCreateStringsHeap.Location = new System.Drawing.Point(6, 226);
            this.chkAlwaysCreateStringsHeap.Name = "chkAlwaysCreateStringsHeap";
            this.chkAlwaysCreateStringsHeap.Size = new System.Drawing.Size(157, 17);
            this.chkAlwaysCreateStringsHeap.TabIndex = 1;
            this.chkAlwaysCreateStringsHeap.Text = "Always Create Strings Heap";
            this.chkAlwaysCreateStringsHeap.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysCreateUSHeap
            // 
            this.chkAlwaysCreateUSHeap.AutoSize = true;
            this.chkAlwaysCreateUSHeap.Location = new System.Drawing.Point(6, 249);
            this.chkAlwaysCreateUSHeap.Name = "chkAlwaysCreateUSHeap";
            this.chkAlwaysCreateUSHeap.Size = new System.Drawing.Size(140, 17);
            this.chkAlwaysCreateUSHeap.TabIndex = 1;
            this.chkAlwaysCreateUSHeap.Text = "Always Create US Heap";
            this.chkAlwaysCreateUSHeap.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPreserveTypeRefRids);
            this.groupBox1.Controls.Add(this.chkPreserveTypeDefRids);
            this.groupBox1.Controls.Add(this.chkPreserveFieldRids);
            this.groupBox1.Controls.Add(this.chkPreserveMethodRids);
            this.groupBox1.Controls.Add(this.chkPreserveParamRids);
            this.groupBox1.Controls.Add(this.chkPreserveMemberRefRids);
            this.groupBox1.Controls.Add(this.chkPreserveStandAloneSigRids);
            this.groupBox1.Controls.Add(this.chkPreserveEventRids);
            this.groupBox1.Controls.Add(this.chkPreservePropertyRids);
            this.groupBox1.Controls.Add(this.chkPreserveTypeSpecRids);
            this.groupBox1.Controls.Add(this.chkPreserveMethodSpecRids);
            this.groupBox1.Controls.Add(this.chkPreserveAllMethodRids);
            this.groupBox1.Controls.Add(this.chkPreserveRids);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 356);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.chkPreserveStringOffsets);
            this.groupBox2.Controls.Add(this.chkPreserveUSOffsets);
            this.groupBox2.Controls.Add(this.chkPreserveBlobOffsets);
            this.groupBox2.Controls.Add(this.chkPreserveExtraSignatureData);
            this.groupBox2.Controls.Add(this.chkAlwaysCreateBlobHeap);
            this.groupBox2.Controls.Add(this.chkAlwaysCreateUSHeap);
            this.groupBox2.Controls.Add(this.chkPreserveAll);
            this.groupBox2.Controls.Add(this.chkAlwaysCreateStringsHeap);
            this.groupBox2.Controls.Add(this.chkKeepOldMaxStack);
            this.groupBox2.Controls.Add(this.chkAlwaysCreateGuidHeap);
            this.groupBox2.Location = new System.Drawing.Point(204, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 356);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkNoThrowInstanceLogger);
            this.groupBox3.Location = new System.Drawing.Point(0, 308);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(190, 48);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // chkNoThrowInstanceLogger
            // 
            this.chkNoThrowInstanceLogger.AutoSize = true;
            this.chkNoThrowInstanceLogger.Location = new System.Drawing.Point(6, 19);
            this.chkNoThrowInstanceLogger.Name = "chkNoThrowInstanceLogger";
            this.chkNoThrowInstanceLogger.Size = new System.Drawing.Size(147, 17);
            this.chkNoThrowInstanceLogger.TabIndex = 1;
            this.chkNoThrowInstanceLogger.Text = "NoThrowInstance Logger";
            this.chkNoThrowInstanceLogger.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysCreateBlobHeap
            // 
            this.chkAlwaysCreateBlobHeap.AutoSize = true;
            this.chkAlwaysCreateBlobHeap.Location = new System.Drawing.Point(6, 272);
            this.chkAlwaysCreateBlobHeap.Name = "chkAlwaysCreateBlobHeap";
            this.chkAlwaysCreateBlobHeap.Size = new System.Drawing.Size(146, 17);
            this.chkAlwaysCreateBlobHeap.TabIndex = 1;
            this.chkAlwaysCreateBlobHeap.Text = "Always Create Blob Heap";
            this.chkAlwaysCreateBlobHeap.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(470, 20);
            this.txtPath.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(407, 38);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // WriteAssemblyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 402);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "WriteAssemblyForm";
            this.Text = "Write Assembly";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkPreserveTypeRefRids;
        private System.Windows.Forms.CheckBox chkPreserveTypeDefRids;
        private System.Windows.Forms.CheckBox chkPreserveFieldRids;
        private System.Windows.Forms.CheckBox chkPreserveMethodRids;
        private System.Windows.Forms.CheckBox chkPreserveParamRids;
        private System.Windows.Forms.CheckBox chkPreserveMemberRefRids;
        private System.Windows.Forms.CheckBox chkPreserveStandAloneSigRids;
        private System.Windows.Forms.CheckBox chkPreserveEventRids;
        private System.Windows.Forms.CheckBox chkPreservePropertyRids;
        private System.Windows.Forms.CheckBox chkPreserveTypeSpecRids;
        private System.Windows.Forms.CheckBox chkPreserveMethodSpecRids;
        private System.Windows.Forms.CheckBox chkPreserveAllMethodRids;
        private System.Windows.Forms.CheckBox chkPreserveRids;
        private System.Windows.Forms.CheckBox chkPreserveStringOffsets;
        private System.Windows.Forms.CheckBox chkPreserveUSOffsets;
        private System.Windows.Forms.CheckBox chkPreserveBlobOffsets;
        private System.Windows.Forms.CheckBox chkPreserveExtraSignatureData;
        private System.Windows.Forms.CheckBox chkPreserveAll;
        private System.Windows.Forms.CheckBox chkKeepOldMaxStack;
        private System.Windows.Forms.CheckBox chkAlwaysCreateGuidHeap;
        private System.Windows.Forms.CheckBox chkAlwaysCreateStringsHeap;
        private System.Windows.Forms.CheckBox chkAlwaysCreateUSHeap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkNoThrowInstanceLogger;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkAlwaysCreateBlobHeap;
    }
}