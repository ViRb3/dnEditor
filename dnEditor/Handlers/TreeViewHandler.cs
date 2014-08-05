using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    public static class TreeViewHandler
    {
        private const string VirtualNode = "VIRT";
        private static TreeNode _currentNode;
        private static TreeView _currentTreeView;

        private static readonly Dictionary<string, TreeNode> typeNameSpaceDictionary =
            new Dictionary<string, TreeNode>();

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

        public static TreeNode FindMethod(TreeView treeView, MethodDef method)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                TreeNode nodeResult = CycleNodes(node, method);

                if (nodeResult != null)
                    return nodeResult;
            }
            return null;
        }

        public static TreeNode CycleNodes(TreeNode node, MethodDef method)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                if ((subNode.Tag is MethodDef) && (subNode.Tag as MethodDef) == method)
                {
                    return subNode;
                }

                TreeNode nodeResult = CycleNodes(subNode, method);

                if (nodeResult != null)
                    return nodeResult;
            }
            return null;
        }

        public static void Load(TreeView treeView, AssemblyDef currentAssembly, bool clear)
        {
            _currentTreeView = treeView;

            if (clear)
                treeView.Nodes.Clear();

            AddFile(currentAssembly);

            foreach (ModuleDefMD module in currentAssembly.Modules)
            {
                AddModule(module); // node is ModuleDefMD

                if (module.GetAssemblyRefs().Any())
                {
                    TreeNode node = _currentNode.Nodes.Add("References");
                    node.ImageIndex = node.SelectedImageIndex = 44;
                    _currentNode = node;

                    foreach (AssemblyRef assemblyRef in module.GetAssemblyRefs().OrderBy(m => m.Name))
                    {
                        AddAssemblyRef(assemblyRef); //node is AssemblyRef
                        _currentNode = _currentNode.Parent; // node is References
                    }
                }

                typeNameSpaceDictionary.Clear();

                foreach (TypeDef type in module.Types.OrderBy(m => m.Name))
                {
                    LoadMembers(type);
                }
            }
        }

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

        private static void LoadMembers(TypeDef type, TypeDef parentType = null)
        {
            if (!type.IsNested)
            {
                if (!typeNameSpaceDictionary.ContainsKey(type.Namespace))
                {
                    ReturnToModule(); // node is ModuleDefMD

                    AddNameSpace(type.Namespace);
                    AddType(type); // node is TypeDef
                    typeNameSpaceDictionary.Add(type.Namespace, _currentNode.Parent);
                }
                else
                {
                    _currentNode = typeNameSpaceDictionary[type.Namespace]; // node is String
                    AddType(type); // node is TypeDef
                }
            }
            else if (parentType != null)
                AddType(type); // node is nested TypeDef

            List<MethodDef> accessorMethods = type.GetAccessorMethods();
            foreach (MethodDef method in type.Methods.OrderBy(m => m.Name))
            {
                if (!accessorMethods.Contains(method))
                {
                    AddMethod(method); // node is MethodDef
                    ReturnToType(); // node is TypeDef
                }
            }

            foreach (PropertyDef property in type.Properties.OrderBy(m => m.Name))
            {
                AddProperty(property); // node is PropertyDef
                ReturnToType(); // node is TypeDef
            }

            foreach (EventDef eventDef in type.Events.OrderBy(m => m.Name))
            {
                AddEvent(eventDef); // node is EventDef
                ReturnToType(); // node is TypeDef
            }

            foreach (FieldDef field in type.Fields.OrderBy(m => m.Name))
            {
                AddField(field); // node is FieldDef
                ReturnToType(); // node is TypeDef
            }

            //foreach (TypeDef nestedType in type.NestedTypes.OrderBy(m => m.Name))
            //{
            //    ReturnToType(type); // node is TypeDef
            //    LoadMembers(nestedType, type);
            //}
        }

        private static void ReturnToModule()
        {
            if (!(_currentNode.Tag is ModuleDefMD))
            {
                _currentNode = _currentNode.Parent;
                ReturnToModule();
            }
        }

        private static void ReturnToType()
        {
            if (!(_currentNode.Tag is TypeDef))
            {
                _currentNode = _currentNode.Parent;
                ReturnToType();
            }
        }

        private static void ReturnToType(TypeDef type)
        {
            if (!(_currentNode.Tag is TypeDef) || (_currentNode.Tag as TypeDef) != type)
            {
                _currentNode = _currentNode.Parent;
                ReturnToType();
            }
        }

        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var oArgs = e.Argument as object[];
            var tNodeParent = oArgs[0] as TreeNode;
            //string sInfo = oArgs[1].ToString();


            var r = new Random();
            Thread.Sleep(r.Next(500, 2500));

            var arrChildren = new object[] {"Grapes", "Apples"};

            // Return the Parent Tree Node and the list of children to the
            // UI thread.
            e.Result = new object[] {tNodeParent, arrChildren};
        }

        private static void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var oResult = e.Result as object[];
            var tNodeParent = oResult[0] as TreeNode;
            var arrChildren = oResult[1] as string[];

            tNodeParent.Nodes.Clear();

            foreach (string sChild in arrChildren)
            {
                TreeNode tNode = tNodeParent.Nodes.Add(sChild);
                AddVirtualNode(tNode);
            }
        }

        public static void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.ContainsKey(VirtualNode))
            {
                var bw = new BackgroundWorker();
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted +=
                    bw_RunWorkerCompleted;

                object[] oArgs = {e.Node, "Some information..."};
                bw.RunWorkerAsync(oArgs);
            }
        }

        public static void AddVirtualNode(TreeNode tNode)
        {
            var tVirt = new TreeNode
            {
                Text = "Loading...",
                Name = VirtualNode,
                ForeColor = Color.Blue,
                NodeFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline)
            };

            tNode.Nodes.Add(tVirt);
        }

        #region AddNode

        private static TreeNode NewNode(string text)
        {
            return new TreeNode("text");
        }

        private static TreeNode AddFile(AssemblyDef file)
        {
            TreeNode node = NewNode(file.Name);
            node.Tag = file;
            node.ImageIndex = node.SelectedImageIndex = 0;
            return node;
        }

        private static TreeNode AddModule(ModuleDefMD module)
        {
            TreeNode node = NewNode(module.FullName);
            node.Tag = module;
            node.ImageIndex = node.SelectedImageIndex = 28;
            return node;
        }

        private static TreeNode AddAssemblyRef(AssemblyRef assemblyRef)
        {
            TreeNode node = NewNode(assemblyRef.FullName);
            node.Tag = assemblyRef;
            node.ImageIndex = node.SelectedImageIndex = 0;
            return node;
        }

        private static TreeNode AddNameSpace(string nameSpace)
        {
            TreeNode node = NewNode(nameSpace);
            node.Tag = nameSpace;
            node.ImageIndex = node.SelectedImageIndex = 31;
            return node;
        }

        private static TreeNode AddType(TypeDef type) // (Class)
        {
            TreeNode node = NewNode(type.Name);
            node.Tag = type;
            node.ImageIndex = node.SelectedImageIndex = 6;
            return node;
        }

        private static TreeNode AddMethod(MethodDef method)
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

        private static TreeNode[] AddProperty(PropertyDef property)
        {
            TreeNode node = NewNode(string.Format(property.Name));

            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;

            var nodeList = new List<TreeNode>();

            nodeList.Add(node);

            if (property.GetMethod != null)
            {
                string type = property.GetMethod.ReturnType.GetExtendedName();

                nodeList.Add(AddMethod(property.GetMethod));
                node.Text = string.Format("{0}: {1}", property.Name, type);
            }

            if (property.SetMethod != null)
            {
                nodeList.Add(AddMethod(property.SetMethod));
            }

            foreach (MethodDef method in property.OtherMethods)
            {
                nodeList.Add(AddMethod(method));
            }

            return nodeList.ToArray();
        }

        private static TreeNode[] AddEvent(EventDef eventDef)
        {
            TreeNode node = NewNode(string.Format("{0}: {1}", eventDef.Name, "EventHandler"));
            var nodeList = new List<TreeNode>();

            nodeList.Add(node);

            node.Tag = eventDef;
            node.ImageIndex = node.SelectedImageIndex = 15;
            _currentNode = node;

            if (eventDef.AddMethod != null)
            {
                nodeList.Add(AddMethod(eventDef.AddMethod));
            }

            if (eventDef.RemoveMethod != null)
            {
                nodeList.Add(AddMethod(eventDef.RemoveMethod));
            }

            if (eventDef.InvokeMethod != null)
            {
                nodeList.Add(AddMethod(eventDef.InvokeMethod));
            }

            foreach (MethodDef method in eventDef.OtherMethods)
            {
                nodeList.Add(AddMethod(method));
            }

            return nodeList.ToArray();
        }

        private static TreeNode AddField(FieldDef field)
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