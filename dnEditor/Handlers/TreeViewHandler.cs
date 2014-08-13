using System;
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
        public static TreeNode CurrentNode;
        public static List<TreeNode> ModuleNodes = new List<TreeNode>();
        public static TreeNode RefNode;
        public static TreeView CurrentTreeView;

        public static readonly Dictionary<string, TreeNode> TypeNameSpaceDictionary =
            new Dictionary<string, TreeNode>();

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


        public static void Load(TreeView treeView, AssemblyDef currentAssembly, bool clear)
        {
            CurrentTreeView = treeView;

            if (clear)
                treeView.Nodes.Clear();

            TreeNode file = NewFile(currentAssembly);
            file.AddTo(treeView);

            foreach (ModuleDefMD module in currentAssembly.Modules)
            {
                TreeNode moduleNode = NewModule(module);
                moduleNode.AddTo(file); // Current is module

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

            if (parentNode == RefNode)
            {
                ReferenceHandler.HandleReference2(parentNode, ref children);
            }
            else if (parentNode.Tag is ModuleDefMD)
            {
                foreach (TypeDef type in (parentNode.Tag as ModuleDefMD).Types)
                {
                    children.Add(NewType(type));
                }
            }
            else if (parentNode.Tag is TypeDef)
            {
                foreach (TypeDef type in (parentNode.Tag as TypeDef).NestedTypes)
                    children.Add(NewType(type));
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
                if (child.Tag is TypeDef && (child.Tag as TypeDef).IsNested)
                {
                    /*TODO: TypeHandler.HandleNestedType(child.Tag as TypeDef);
                    continue;*/
                }

                parentNode.Nodes.Add(child);

                if (child.Tag is TypeDef && (child.Tag as TypeDef).HasNestedTypes)
                {
                    AddVirtualNode(child);
                }
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
            CurrentNode = node;
            ModuleNodes.Add(node);
        }

        public static void AddTo(this TreeNode node, TreeNode parentNode)
        {
            parentNode.Nodes.Add(node);
            CurrentNode = node;
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

        public static TreeNode[] NewProperty(PropertyDef property)
        {
            TreeNode node = NewNode(string.Format(property.Name));

            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;

            var nodeList = new List<TreeNode>();

            nodeList.Add(node);

            if (property.GetMethod != null)
            {
                string type = property.GetMethod.ReturnType.GetExtendedName();

                nodeList.Add(NewMethod(property.GetMethod));
                node.Text = string.Format("{0}: {1}", property.Name, type);
            }

            if (property.SetMethod != null)
            {
                nodeList.Add(NewMethod(property.SetMethod));
            }

            foreach (MethodDef method in property.OtherMethods)
            {
                nodeList.Add(NewMethod(method));
            }

            return nodeList.ToArray();
        }

        public static TreeNode[] NewEvent(EventDef eventDef)
        {
            TreeNode node = NewNode(string.Format("{0}: {1}", eventDef.Name, "EventHandler"));
            var nodeList = new List<TreeNode>();

            nodeList.Add(node);

            node.Tag = eventDef;
            node.ImageIndex = node.SelectedImageIndex = 15;
            CurrentNode = node;

            if (eventDef.AddMethod != null)
            {
                nodeList.Add(NewMethod(eventDef.AddMethod));
            }

            if (eventDef.RemoveMethod != null)
            {
                nodeList.Add(NewMethod(eventDef.RemoveMethod));
            }

            if (eventDef.InvokeMethod != null)
            {
                nodeList.Add(NewMethod(eventDef.InvokeMethod));
            }

            foreach (MethodDef method in eventDef.OtherMethods)
            {
                nodeList.Add(NewMethod(method));
            }

            return nodeList.ToArray();
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