namespace dnEditor.Forms
{
    partial class PickReferenceForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickReferenceForm));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btnSelect = new System.Windows.Forms.Button();
            this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToEntryPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToModuleCCtorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList2;
            this.treeView1.Location = new System.Drawing.Point(12, 35);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(421, 462);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Assembly.png");
            this.imageList2.Images.SetKeyName(1, "AssemblyList.png");
            this.imageList2.Images.SetKeyName(2, "AssemblyWarning.png");
            this.imageList2.Images.SetKeyName(3, "Back.png");
            this.imageList2.Images.SetKeyName(4, "Break.png");
            this.imageList2.Images.SetKeyName(5, "Breakpoint.png");
            this.imageList2.Images.SetKeyName(6, "Class.png");
            this.imageList2.Images.SetKeyName(7, "ClearSearch.png");
            this.imageList2.Images.SetKeyName(8, "Constructor.png");
            this.imageList2.Images.SetKeyName(9, "CurrentLine.png");
            this.imageList2.Images.SetKeyName(10, "Delegate.png");
            this.imageList2.Images.SetKeyName(11, "Delete.png");
            this.imageList2.Images.SetKeyName(12, "DisabledBreakpoint.png");
            this.imageList2.Images.SetKeyName(13, "Enum.png");
            this.imageList2.Images.SetKeyName(14, "EnumValue.png");
            this.imageList2.Images.SetKeyName(15, "Event.png");
            this.imageList2.Images.SetKeyName(16, "ExtensionMethod.png");
            this.imageList2.Images.SetKeyName(17, "Field.png");
            this.imageList2.Images.SetKeyName(18, "FieldReadOnly.png");
            this.imageList2.Images.SetKeyName(19, "Find.png");
            this.imageList2.Images.SetKeyName(20, "FindAssembly.png");
            this.imageList2.Images.SetKeyName(21, "Folder.Closed.png");
            this.imageList2.Images.SetKeyName(22, "Folder.Open.png");
            this.imageList2.Images.SetKeyName(23, "Forward.png");
            this.imageList2.Images.SetKeyName(24, "ILSpy.ico");
            this.imageList2.Images.SetKeyName(25, "ILSpy-Large.ico");
            this.imageList2.Images.SetKeyName(26, "Indexer.png");
            this.imageList2.Images.SetKeyName(27, "Interface.png");
            this.imageList2.Images.SetKeyName(28, "Library.png");
            this.imageList2.Images.SetKeyName(29, "Literal.png");
            this.imageList2.Images.SetKeyName(30, "Method.png");
            this.imageList2.Images.SetKeyName(31, "NameSpace.png");
            this.imageList2.Images.SetKeyName(32, "OK.png");
            this.imageList2.Images.SetKeyName(33, "Open.png");
            this.imageList2.Images.SetKeyName(34, "Operator.png");
            this.imageList2.Images.SetKeyName(35, "OverlayCompilerControlled.png");
            this.imageList2.Images.SetKeyName(36, "OverlayInternal.png");
            this.imageList2.Images.SetKeyName(37, "OverlayPrivate.png");
            this.imageList2.Images.SetKeyName(38, "OverlayProtected.png");
            this.imageList2.Images.SetKeyName(39, "OverlayProtectedInternal.png");
            this.imageList2.Images.SetKeyName(40, "OverlayStatic.png");
            this.imageList2.Images.SetKeyName(41, "PInvokeMethod.png");
            this.imageList2.Images.SetKeyName(42, "PrivateInternal.png");
            this.imageList2.Images.SetKeyName(43, "Property.png");
            this.imageList2.Images.SetKeyName(44, "ReferenceFolder.Closed.png");
            this.imageList2.Images.SetKeyName(45, "ReferenceFolder.Open.png");
            this.imageList2.Images.SetKeyName(46, "Refresh.png");
            this.imageList2.Images.SetKeyName(47, "Resource.png");
            this.imageList2.Images.SetKeyName(48, "ResourceImage.png");
            this.imageList2.Images.SetKeyName(49, "ResourceResourcesFile.png");
            this.imageList2.Images.SetKeyName(50, "ResourceXml.png");
            this.imageList2.Images.SetKeyName(51, "ResourceXsd.png");
            this.imageList2.Images.SetKeyName(52, "ResourceXsl.png");
            this.imageList2.Images.SetKeyName(53, "ResourceXslt.png");
            this.imageList2.Images.SetKeyName(54, "SaveFile.png");
            this.imageList2.Images.SetKeyName(55, "Search.png");
            this.imageList2.Images.SetKeyName(56, "StaticClass.png");
            this.imageList2.Images.SetKeyName(57, "Struct.png");
            this.imageList2.Images.SetKeyName(58, "SubTypes.png");
            this.imageList2.Images.SetKeyName(59, "SuperTypes.png");
            this.imageList2.Images.SetKeyName(60, "ViewCode.png");
            this.imageList2.Images.SetKeyName(61, "VirtualMethod.png");
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(12, 6);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(421, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "SELECT";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // treeMenu
            // 
            this.treeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToEntryPointToolStripMenuItem,
            this.goToModuleCCtorToolStripMenuItem,
            this.toolStripSeparator2,
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeAssemblyToolStripMenuItem});
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(179, 170);
            this.treeMenu.Opened += new System.EventHandler(this.treeMenu_Opened);
            // 
            // goToEntryPointToolStripMenuItem
            // 
            this.goToEntryPointToolStripMenuItem.Name = "goToEntryPointToolStripMenuItem";
            this.goToEntryPointToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.goToEntryPointToolStripMenuItem.Text = "Go to EntryPoint";
            this.goToEntryPointToolStripMenuItem.Click += new System.EventHandler(this.goToEntryPointToolStripMenuItem_Click);
            // 
            // goToModuleCCtorToolStripMenuItem
            // 
            this.goToModuleCCtorToolStripMenuItem.Name = "goToModuleCCtorToolStripMenuItem";
            this.goToModuleCCtorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.goToModuleCCtorToolStripMenuItem.Text = "Go to ModuleCCtor";
            this.goToModuleCCtorToolStripMenuItem.Click += new System.EventHandler(this.goToModuleCCtorToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.collapseToolStripMenuItem.Text = "Collapse";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.expandToolStripMenuItem.Text = "Expand";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // closeAssemblyToolStripMenuItem
            // 
            this.closeAssemblyToolStripMenuItem.Name = "closeAssemblyToolStripMenuItem";
            this.closeAssemblyToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.closeAssemblyToolStripMenuItem.Text = "Close assembly";
            this.closeAssemblyToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // PickReferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 509);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PickReferenceForm";
            this.Text = "Choose a reference...";
            this.Shown += new System.EventHandler(this.PickReferenceForm_Shown);
            this.treeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ContextMenuStrip treeMenu;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeAssemblyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToEntryPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToModuleCCtorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}