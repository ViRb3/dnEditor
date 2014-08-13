using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    /*
     * NOTE: Main module is ALWAYS the first module and is Manifest Module!
     * 
     * TODO:
     * 01. File nodes X
     * 02. Module nodes X
     * 03. Reference nodes X
     * 04. NS nodes X
     * 05. Type nodes X
     * 06. Method nodes X
     * 07. Property nodes X
     * 08. Event nodes X
     * 09. Field nodes X
     * 10. Nested type nodes X
     * 11. Resources
     */

    public static class TreeViewHandler
    {
        private const string VirtualNode = "VIRT";

        public static TreeNode RefNode;
        public static TreeNode CurrentModule;
        public static TreeView CurrentTreeView;

        public static List<string> NameSpaceList = new List<string>();

        #region Interface

        public static CurrentAssembly DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return null;
            }
            var filePath = (string[]) (e.Data.GetData(DataFormats.FileDrop));
            foreach (string fileLoc in filePath.Where(File.Exists))
            {
                return new CurrentAssembly(Path.GetFullPath(fileLoc));
            }
            return null;
        }

        public static void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        #endregion Interface

        public static void LoadAssembly(TreeView treeView, AssemblyDef currentAssembly, bool clear)
        {
            CurrentTreeView = treeView;

            if (clear)
                treeView.Nodes.Clear();

            TreeNode file = NewFile(currentAssembly);
            file.AddTo(treeView);

            foreach (ModuleDefMD module in currentAssembly.Modules)
            {
                NameSpaceList.Clear();

                TreeNode moduleNode = NewModule(module);
                moduleNode.AddTo(file);

                CurrentModule = moduleNode;

                if (module.GetAssemblyRefs().Any())
                {
                    ReferenceHandler.HandleReferences(module.GetAssemblyRefs());
                }

                if (module.Types.Any())
                {
                    AddVirtualNode(moduleNode);
                }
            }
        }


        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var parentNode = e.Argument as TreeNode;

            var children = new List<TreeNode>();

            if (parentNode == RefNode) // Assembly Reference
            {
                ReferenceHandler.ProcessAssemblyRefs(parentNode, ref children);
            }
            else if (parentNode.Tag is ModuleDefMD) // Module
            {
                foreach (TypeDef type in (parentNode.Tag as ModuleDefMD).Types)
                {
                    TypeHandler.HandleType(type);
                }
            }
            else if (parentNode.Tag is TypeDef) // Type
            {
                TypeHandler.ProcessTypeMembers(parentNode, ref children);
            }


            e.Result = new object[] {parentNode, children.ToArray()};
        }

        private static void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as object[];
            var parentNode = result[0] as TreeNode;
            var children = result[1] as TreeNode[];

            if (children == null) return;

            parentNode.FindVirtualNode().Remove();

            foreach (TreeNode child in children)
            {
                if (child.Tag is TypeDef)
                {
                    var type = child.Tag as TypeDef;

                    if (type.IsNested)
                    {
                        TypeHandler.HandleType(child.Tag as TypeDef);
                        continue;
                    }

                    if (ExpandableType(type))
                    {
                        AddVirtualNode(child);
                    }
                }

                parentNode.Nodes.Add(child);
            }
        }

        public static void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FindVirtualNode() == null) return;

            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw.RunWorkerAsync(e.Node);
        }

        #region Functions

        public static bool ExpandableType(TypeDef type)
        {
            return (type.HasNestedTypes || type.HasMethods || type.HasEvents || type.HasFields || type.HasProperties);
        }

        public static TreeNode FindMethod(TreeNode node, MethodDef method)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                if ((subNode.Tag is MethodDef) && (subNode.Tag as MethodDef) == method)
                {
                    return subNode;
                }

                TreeNode nodeResult = FindMethod(subNode, method);

                if (nodeResult != null)
                    return nodeResult;
            }
            return null;
        }

        public static void AddVirtualNode(TreeNode parentNode)
        {
            var node = new TreeNode
            {
                Text = "Loading...",
                Name = VirtualNode,
                ForeColor = Color.Blue,
                NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline),
            };

            parentNode.Nodes.Add(node);
        }

        public static void AddVirtualNodeWithContent(TreeNode parentNode, object[] content)
        {
            var node = new TreeNode
            {
                Text = "Loading...",
                Name = VirtualNode,
                ForeColor = Color.Blue,
                NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline),
                Tag = content
            };

            parentNode.Nodes.Add(node);
        }

        public static TreeNode FindVirtualNode(this TreeNode parentNode)
        {
            return
                parentNode.Nodes.Cast<TreeNode>()
                    .FirstOrDefault(n => n.Name == VirtualNode && n.ForeColor == Color.Blue && n.Text == "Loading...");
        }

        #endregion Functions

        #region AddNode

        public static void AddTo(this TreeNode node, TreeView view)
        {
            view.Nodes.Add(node);
        }

        public static void AddTo(this TreeNode node, TreeNode parentNode)
        {
            parentNode.Nodes.Add(node);
        }

        public static TreeNode NewNode(string text)
        {
            return new TreeNode(text);
        }

        public static TreeNode NewFile(AssemblyDef file)
        {
            TreeNode node = NewNode(file.Name);
            node.Tag = file;
            node.ImageIndex = node.SelectedImageIndex = 0;

            return node;
        }

        public static TreeNode NewModule(ModuleDefMD module)
        {
            TreeNode node = NewNode(module.FullName);
            node.Tag = module;
            node.ImageIndex = node.SelectedImageIndex = 28;

            return node;
        }

        public static TreeNode NewAssemblyRef(AssemblyRef assemblyRef)
        {
            TreeNode node = NewNode(assemblyRef.FullName);
            node.Tag = assemblyRef;
            node.ImageIndex = node.SelectedImageIndex = 0;

            return node;
        }

        public static TreeNode NewNameSpace(string nameSpace)
        {
            TreeNode node = NewNode(nameSpace);
            node.Tag = nameSpace;
            node.ImageIndex = node.SelectedImageIndex = 31;

            return node;
        }

        public static TreeNode NewType(TypeDef type) // (Class)
        {
            TreeNode node = NewNode(type.Name); //TODO: Extended name
            node.Tag = type;
            node.ImageIndex = node.SelectedImageIndex = 6;

            return node;
        }

        public static TreeNode NewMethod(MethodDef method)
        {
            string parameters = "";

            foreach (Parameter parameter in method.Parameters.Where(param => !param.IsHiddenThisParameter))
            {
                parameters += parameter.Type.GetExtendedName();
                parameters += ", ";
            }

            parameters = parameters.TrimEnd(new[] {',', ' '});

            TreeNode node = NewNode(string.Format("{0}({1}): {2}", method.Name, parameters,
                method.ReturnType.GetExtendedName()));
            node.Tag = method;
            node.ImageIndex = node.SelectedImageIndex = 30;

            return node;
        }

        public static TreeNode NewProperty(PropertyDef property)
        {
            TreeNode node = NewNode(string.Format(property.Name));

            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;

            if (property.GetMethod != null)
            {
                string type = property.GetMethod.ReturnType.GetExtendedName();

                node.Nodes.Add(NewMethod(property.GetMethod));
                node.Text = string.Format("{0}: {1}", property.Name, type);
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

        public static TreeNode NewEvent(EventDef @event)
        {
            TreeNode node = NewNode(string.Format("{0}: {1}", @event.Name, "EventHandler"));

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

        public static TreeNode NewField(FieldDef field)
        {
            string type = field.FieldType.GetExtendedName();

            TreeNode node =
                NewNode(string.Format("{0}: {1}", field.Name, type));
            node.Tag = field;
            node.ImageIndex = node.SelectedImageIndex = 17;

            return node;
        }

        #endregion AddNode
    }
}