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

    public class TreeViewHandler : ITreeMenu
    {
        public TreeNode CurrentMethod = null;
        public TreeNode CurrentModule;
        public ContextMenuStrip CurrentTreeMenu;
        public TreeView CurrentTreeView;

        public List<string> NameSpaceList = new List<string>();
        public TreeNode RefNode;

        public TreeNode SelectedNode;

        #region Interface

        public string DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return null;
            }
            var filePath = (string[]) (e.Data.GetData(DataFormats.FileDrop));
            string fileLoc = filePath.FirstOrDefault(File.Exists);
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
            if (SelectedNode == null || SelectedNode.Nodes.Count < 1)
                CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Collapse all").Enabled = false;
            else
                CurrentTreeMenu.Items.Cast<ToolStripItem>().First(i => i.Text == "Collapse all").Enabled = true;
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

            TreeNode assembly = SelectedNode.FirstParentNode();
            if (assembly == null) return;

            CurrentMethod = null;
            CurrentModule = null;
            currentAssembly = null;
            assembly.Remove();
        }

        #endregion TreeMenuStrip

        #region TreeView Events

        public void LoadAssembly(AssemblyDef currentAssembly, string path, bool clear)
        {
            RefNode = null;
            CurrentModule = null;

            if (clear)
                CurrentTreeView.Nodes.Clear();

            TreeNode file = NewFile(currentAssembly, path); // AssemblyDef
            file.AddTo(CurrentTreeView);

            foreach (ModuleDefMD module in currentAssembly.Modules)
            {
                NameSpaceList.Clear();

                TreeNode moduleNode = NewModule(module);

                moduleNode.AddTo(file);

                CurrentModule = moduleNode;

                if (module.Types.Any())
                {
                    foreach (TypeDef type in module.Types)
                    {
                        new TypeHandler(this).HandleType(type, false);
                    }
                }

                CurrentModule = moduleNode;

                if (module.GetAssemblyRefs().Any())
                {
                    new ReferenceHandler(this).HandleReferences(module.GetAssemblyRefs());
                }
            }

            var processor2 = new VirtualNodeHandler(RefNode, this);
            processor2.ProcessNode();
            RefNode = processor2.Node;
        }

        public void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            VirtualNodeUtilities.ExpandHandler(e.Node, this);
        }

        public void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e,
            ref CurrentAssembly currentAssembly)
        {
            SelectedNode = null;
            TreeNode assemblyNode = e.Node.FirstParentNode();

            if (currentAssembly == null || currentAssembly.Assembly != assemblyNode.Tag as AssemblyDef)
            {
                currentAssembly = new CurrentAssembly(assemblyNode.Tag as AssemblyDef);
                currentAssembly.Path = assemblyNode.ToolTipText;
            }

            if (e.Node.Tag is MethodDef)
            {
                DataGridViewHandler.ReadMethod(e.Node.Tag as MethodDef);
                CurrentMethod = e.Node;
            }
            else MainForm.DgBody.Rows.Clear();

            if (e.Button == MouseButtons.Right)
            {
                SelectedNode = e.Node;
            }
        }

        public void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e,
            ref CurrentAssembly currentAssembly)
        {
            if (!(e.Node.Tag is AssemblyRef))
            {
                return;
            }

            var assemblyRef = e.Node.Tag as AssemblyRef;
            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            string directory = Directory.GetParent(currentAssembly.Path).FullName;

            var paths = new List<string>
            {
                Path.Combine(directory, assemblyRef.Name + ".dll"),
                Path.Combine(directory, assemblyRef.Name + ".exe"),
            };

            var paths2 = new List<string>
            {
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".exe"),
                Path.Combine(runtimeDirectory, assemblyRef.Name + ".dll"),
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

        public TreeViewHandler(TreeView currentTreeView, ContextMenuStrip currentTreeMenu)
        {
            CurrentTreeView = currentTreeView;
            CurrentTreeMenu = currentTreeMenu;
        }

        public TreeNode NewNode(string text)
        {
            var node = new TreeNode(text.ShortenTreeNodeText());
            node.ContextMenuStrip = CurrentTreeMenu;

            return node;
        }

        public TreeNode NewFile(AssemblyDef file, string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Path is invalid!");

            TreeNode node = NewNode(file.Name);
            node.Tag = file;
            node.ImageIndex = node.SelectedImageIndex = 0;
            node.ToolTipText = path;

            return node;
        }

        public TreeNode NewModule(ModuleDefMD module)
        {
            TreeNode node = NewNode(module.FullName);
            node.Tag = module;
            node.ImageIndex = node.SelectedImageIndex = 28;

            return node;
        }

        public TreeNode NewReferenceFolder()
        {
            TreeNode node = NewNode("References");
            node.ImageIndex = node.SelectedImageIndex = 44;

            return node;
        }

        public TreeNode NewAssemblyRef(AssemblyRef assemblyRef)
        {
            TreeNode node = NewNode(assemblyRef.FullName);
            node.Tag = assemblyRef;
            node.ImageIndex = node.SelectedImageIndex = 0;

            return node;
        }

        public TreeNode NewNameSpace(string nameSpace)
        {
            TreeNode node = NewNode(nameSpace);
            node.Tag = nameSpace;
            node.ImageIndex = node.SelectedImageIndex = 31;

            return node;
        }

        public TreeNode NewType(TypeDef type) // (Class)
        {
            TreeNode node = NewNode(type.Name); //TODO: Extended name
            node.Tag = type;
            node.ImageIndex = node.SelectedImageIndex = 6;

            return node;
        }

        public TreeNode NewMethod(MethodDef method)
        {
            string parameters = "";

            foreach (Parameter parameter in method.Parameters.Where(param => !param.IsHiddenThisParameter))
            {
                parameters += parameter.Type.GetExtendedName();
                parameters += ", ";
            }

            parameters = parameters.TrimEnd(new[] { ',', ' ' });

            TreeNode node = NewNode(String.Format("{0}({1}): {2}", method.Name, parameters,
                method.ReturnType.GetExtendedName()));
            node.Tag = method;
            node.ImageIndex = node.SelectedImageIndex = 30;

            return node;
        }

        public TreeNode NewProperty(PropertyDef property)
        {
            TreeNode node = NewNode(String.Format(property.Name));

            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;

            if (property.GetMethod != null)
            {
                string type = property.GetMethod.ReturnType.GetExtendedName();

                node.Nodes.Add(NewMethod(property.GetMethod));
                node.Text = String.Format("{0}: {1}", property.Name, type);
            }

            if (property.SetMethod != null)
            {
                node.Nodes.Add(NewMethod(property.SetMethod));
            }

            foreach (MethodDef method in property.OtherMethods)
            {
                node.Nodes.Add(NewMethod(method));
            }

            return node;
        }

        public TreeNode NewEvent(EventDef @event)
        {
            TreeNode node = NewNode(String.Format("{0}: {1}", @event.Name, "EventHandler"));

            node.Tag = @event;
            node.ImageIndex = node.SelectedImageIndex = 15;

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

            foreach (MethodDef method in @event.OtherMethods)
            {
                node.Nodes.Add(NewMethod(method));
            }

            return node;
        }

        public TreeNode NewField(FieldDef field)
        {
            string type = field.FieldType.GetExtendedName();

            TreeNode node =
                NewNode(String.Format("{0}: {1}", field.Name, type));
            node.Tag = field;
            node.ImageIndex = node.SelectedImageIndex = 17;

            return node;
        }
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