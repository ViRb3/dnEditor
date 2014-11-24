using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    public class VirtualNode
    {
        public static string Name = "VIRTNODE";
    }

    public class DeVirtualNode
    {
        public static string Name = "DEVIRTNODE";
    }

    public class TreeViewHandler : ITreeMenu
    {
        public TreeNode CurrentMethod;
        public TreeNode CurrentModule;
        public ContextMenuStrip CurrentTreeMenu;
        public TreeView CurrentTreeView;
        public List<string> NameSpaceList = new List<string>();
        public TreeNode RefNode;
        public TreeNode SelectedNode;

        public TreeViewHandler(TreeView currentTreeView, ContextMenuStrip currentTreeMenu)
        {
            CurrentTreeView = currentTreeView;
            CurrentTreeMenu = currentTreeMenu;
        }

        private void OrderNamespaces()
        {
            var nodes = new List<TreeNode>();

            nodes.AddRange(CurrentModule.Nodes.Cast<TreeNode>().Where(n => !(n.Tag is string)));
            nodes.AddRange(CurrentModule.Nodes.Cast<TreeNode>().Where(n => n.Tag is string).OrderBy(n => n.Text));
            CurrentModule.Nodes.Clear();
            CurrentModule.Nodes.AddRange(nodes.ToArray());
        }

        private void OrderReferences()
        {
            var nodes = new List<TreeNode>();

            var referenceFolder = CurrentModule.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Tag is AssemblyRef[]);
            if (referenceFolder == null) return;

            nodes.AddRange(referenceFolder.Nodes.Cast<TreeNode>().OrderBy(n => n.Text));

            referenceFolder.Nodes.Clear();
            referenceFolder.Nodes.AddRange(nodes.ToArray());
        }

        public TreeNode NewNode(string text)
        {
            var node = new TreeNode(text.ShortenTreeNodeText());
            node.ContextMenuStrip = CurrentTreeMenu;

            return node;
        }

        public TreeNode NewFile(ModuleDefMD file, string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Path is invalid!");

            var node = NewNode(file.Name);
            node.Tag = file;
            node.ImageIndex = node.SelectedImageIndex = 0;
            node.ToolTipText = path;

            return node;
        }

        public TreeNode NewModule(ModuleDefMD module)
        {
            var node = NewNode(module.FullName);
            node.Tag = module;
            node.ImageIndex = node.SelectedImageIndex = 28;
            node.ToolTipText = module.MDToken.FullMetadataTokenString();

            return node;
        }

        public TreeNode NewReferenceFolder()
        {
            var node = NewNode("References");
            node.ImageIndex = node.SelectedImageIndex = 44;

            return node;
        }

        public TreeNode NewAssemblyRef(AssemblyRef assemblyRef)
        {
            var node = NewNode(assemblyRef.FullName);
            node.Tag = assemblyRef;
            node.ImageIndex = node.SelectedImageIndex = 0;
            node.ToolTipText = assemblyRef.MDToken.FullMetadataTokenString();

            return node;
        }

        public TreeNode NewNameSpace(string nameSpace)
        {
            var node = NewNode(nameSpace);
            node.Tag = nameSpace;
            node.ImageIndex = node.SelectedImageIndex = 31;

            return node;
        }

        public TreeNode NewType(TypeDef type) // (Class)
        {
            var node = NewNode(type.GetExtendedName());
            node.Tag = type;
            node.ImageIndex = node.SelectedImageIndex = 6;
            node.ToolTipText = type.MDToken.FullMetadataTokenString();

            return node;
        }

        public TreeNode NewMethod(MethodDef method)
        {
            var parameters = "";

            foreach (var parameter in method.Parameters.Where(param => !param.IsHiddenThisParameter))
            {
                parameters += parameter.Type.GetExtendedName();
                parameters += ", ";
            }

            parameters = parameters.TrimEnd(',', ' ');

            var node = NewNode(String.Format("{0}({1}): {2}", method.Name, parameters,
                method.ReturnType.GetExtendedName()));
            node.Tag = method;
            node.ImageIndex = node.SelectedImageIndex = 30;
            node.ToolTipText = method.MDToken.FullMetadataTokenString();

            return node;
        }

        public TreeNode NewProperty(PropertyDef property)
        {
            var node = NewNode(String.Format(property.Name));

            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;
            node.ToolTipText = property.MDToken.FullMetadataTokenString();

            if (property.GetMethod != null)
            {
                var type = property.GetMethod.ReturnType.GetExtendedName();

                node.Nodes.Add(NewMethod(property.GetMethod));
                node.Text = String.Format("{0}: {1}", property.Name, type);
            }

            if (property.SetMethod != null)
            {
                node.Nodes.Add(NewMethod(property.SetMethod));
            }

            foreach (var method in property.OtherMethods)
            {
                node.Nodes.Add(NewMethod(method));
            }

            return node;
        }

        public TreeNode NewEvent(EventDef @event)
        {
            var node = NewNode(String.Format("{0}: {1}", @event.Name, "EventHandler"));

            node.Tag = @event;
            node.ImageIndex = node.SelectedImageIndex = 15;
            node.ToolTipText = @event.MDToken.FullMetadataTokenString();

            if (@event.AddMethod != null)
            {
                node.Nodes.Add(NewMethod(@event.AddMethod));
            }

            if (@event.RemoveMethod != null)
            {
                node.Nodes.Add(NewMethod(@event.RemoveMethod));
            }

            if (@event.InvokeMethod != null)
            {
                node.Nodes.Add(NewMethod(@event.InvokeMethod));
            }

            foreach (var method in @event.OtherMethods)
            {
                node.Nodes.Add(NewMethod(method));
            }

            return node;
        }

        public TreeNode NewField(FieldDef field)
        {
            var type = field.FieldType.GetExtendedName();

            var node =
                NewNode(String.Format("{0}: {1}", field.Name, type));
            node.Tag = field;
            node.ImageIndex = node.SelectedImageIndex = 17;
            node.ToolTipText = field.MDToken.FullMetadataTokenString();

            return node;
        }

        #region Interface

        public string DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return null;
            }
            var filePath = (string[]) (e.Data.GetData(DataFormats.FileDrop));
            var fileLoc = filePath.FirstOrDefault(File.Exists);
            if (fileLoc == null) return null;

            return Path.GetFullPath(fileLoc);
        }

        public void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        #endregion Interface

        #region TreeMenuStrip

        public void treeMenu_Opened(object sender, EventArgs e)
        {
            CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Collapse all").Enabled = false;
            CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Close assembly").Enabled = false;

            if (SelectedNode != null)
            {
                if (SelectedNode.Nodes.Count > 0)
                    CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Collapse all").Enabled = true;

                if (SelectedNode.Tag is ModuleDefMD)
                    CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Close assembly").Enabled = true;
            }
        }

        public void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null)
                SelectedNode.Expand();
        }

        public void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null)
                SelectedNode.Collapse();
        }

        public void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null)
                CurrentTreeView.CollapseAll();
            else
                SelectedNode.Collapse(false);
        }

        public void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void closeToolStripMenuItem_Click(object sender, EventArgs e, ref CurrentAssembly currentAssembly)
        {
            if (SelectedNode == null) return;

            var assembly = SelectedNode.FirstParentNode();
            if (assembly == null) return;

            CurrentMethod = null;
            CurrentModule = null;
            currentAssembly = null;
            DataGridViewHandler.ClearInstructions();
            assembly.Remove();
        }

        #endregion TreeMenuStrip

        #region TreeView Events

        public void LoadAssembly(ModuleDefMD manifestModule, string path, bool clear)
        {
            RefNode = null;
            CurrentModule = null;

            if (clear)
                CurrentTreeView.Nodes.Clear();

            var file = NewFile(manifestModule, path);
            file.AddTo(CurrentTreeView);

            var module = manifestModule;
            NameSpaceList.Clear();

            var moduleNode = NewModule(module);

            moduleNode.AddTo(file);

            CurrentModule = moduleNode;

            if (module.Types.Any())
            {
                foreach (var type in module.Types.OrderBy(t => t.Name))
                {
                    new TypeHandler(this).HandleType(type, false);
                }
            }

            CurrentModule = moduleNode;

            if (module.GetAssemblyRefs().Any())
            {
                new ReferenceHandler(this).HandleReferences(module.GetAssemblyRefs().OrderBy(a => a.Name));
            }

            var processor = new NodeDevirtualizer(RefNode, this);
            processor.ProcessNode();
            processor.WorkerFinished += processor_WorkerFinished;
            RefNode = processor.Node;
        }

        private void processor_WorkerFinished(TreeNode processedNode)
        {
            OrderNamespaces();
            OrderReferences();
        }

        public void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            VirtualNodeUtilities.ExpandHandler(e.Node, this);
        }

        public void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e,
            ref CurrentAssembly currentAssembly)
        {
            var assemblyNode = e.Node.FirstParentNode();

            if (currentAssembly == null || currentAssembly.ManifestModule != assemblyNode.Tag as ModuleDefMD)
            {
                currentAssembly = new CurrentAssembly(assemblyNode.Tag as ModuleDefMD);
                currentAssembly.Path = assemblyNode.ToolTipText;
            }

            if (e.Node.Tag is MethodDef)
            {
                var method = e.Node.Tag as MethodDef;

                if (CurrentMethod == null || method != CurrentMethod.Tag as MethodDef)
                {
                    MainForm.RtbILSpy.Clear();
                    DataGridViewHandler.ReadMethod(method);
                    CurrentMethod = e.Node;
                }
            }
            else
            {
                MainForm.DgBody.Rows.Clear();
                MainForm.RtbILSpy.Clear();
            }

            SelectedNode = e.Node;
        }

        public void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e,
            ref CurrentAssembly currentAssembly)
        {
            if (!(e.Node.Tag is AssemblyRef))
            {
                return;
            }

            var assemblyRef = e.Node.Tag as AssemblyRef;
            var runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            var directory = Directory.GetParent(currentAssembly.Path).FullName;

            var paths = new List<string>
            {
                Path.Combine(directory, assemblyRef.Name + ".dll"),
                Path.Combine(directory, assemblyRef.Name + ".exe")
            };

            var paths2 = new List<string>
            {
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".exe"),
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".dll")
            };


            if (paths.Where(File.Exists).Count() == 1)
            {
                Functions.OpenFile(this, paths.First(File.Exists), ref currentAssembly);
                return;
            }
            if (paths2.Where(File.Exists).Count() == 1)
            {
                Functions.OpenFile(this, paths2.First(File.Exists), ref currentAssembly);
                return;
            }

            if (MessageBox.Show("Could not automatically find reference file. Browse for it?", "Error",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                currentAssembly = null;
                return;
            }

            var dialog = new OpenFileDialog
            {
                Title = String.Format("Browse for the reference \"{0}\"", assemblyRef.Name),
                Filter = "Executable Files (*.exe)|*.exe|Library Files (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() != DialogResult.OK && File.Exists(dialog.FileName))
            {
                currentAssembly = null;
                return;
            }

            Functions.OpenFile(this, dialog.FileName, ref currentAssembly);
        }

        #endregion TreeView Events
    }

    public static class NodeEmitter
    {
        public static void AddTo(this TreeNode node, TreeView view)
        {
            view.Nodes.Add(node);
        }

        public static void AddTo(this TreeNode node, TreeNode parentNode)
        {
            parentNode.Nodes.Add(node);
        }
    }
}