using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    public static class TreeViewHandler
    {
        private static TreeNode _currentNode;
        private static TreeView _currentTreeView;
        private static Dictionary<string, TreeNode> typeNameSpaceDictionary = new Dictionary<string, TreeNode>();

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
                    LoadMembers(type); //TODO: Type loading takes very long time on larger assemblies
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
            else if(parentType != null)
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

            foreach (TypeDef nestedType in type.NestedTypes.OrderBy(m => m.Name))
            {
                ReturnToType(type); // node is TypeDef
                LoadMembers(nestedType, type);
            }
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

        #region AddNode

        private static void AddFile(AssemblyDef file)
        {
            TreeNode node = _currentTreeView.Nodes.Add(file.Name);
            node.Tag = file;
            node.ImageIndex = node.SelectedImageIndex = 0;
            _currentNode = node;
        }

        private static void AddModule(ModuleDefMD module)
        {
            TreeNode node = _currentNode.Nodes.Add(module.FullName);
            node.Tag = module;
            node.ImageIndex = node.SelectedImageIndex = 28;
            _currentNode = node;
        }

        private static void AddAssemblyRef(AssemblyRef assemblyRef)
        {
            TreeNode node = _currentNode.Nodes.Add(assemblyRef.FullName);
            node.Tag = assemblyRef;
            node.ImageIndex = node.SelectedImageIndex = 0;
            _currentNode = node;
        }

        private static void AddNameSpace(string nameSpace)
        {
            TreeNode node = _currentNode.Nodes.Add(nameSpace);
            node.Tag = nameSpace;
            node.ImageIndex = node.SelectedImageIndex = 31;
            _currentNode = node;
        }

        private static void AddType(TypeDef type) // (Class)
        {
            TreeNode node = _currentNode.Nodes.Add(type.Name);
            node.Tag = type;
            node.ImageIndex = node.SelectedImageIndex = 6;
            _currentNode = node;
        }

        private static void AddMethod(MethodDef method)
        {
            string parameters = "";

            foreach (Parameter parameter in method.Parameters.Where(param => !param.IsHiddenThisParameter))
            {
                parameters += parameter.Type.GetExtendedName();
                parameters += ", ";
            }

            parameters = parameters.TrimEnd(new[] { ',', ' ' });

            TreeNode node =
                _currentNode.Nodes.Add(string.Format("{0}({1}): {2}", method.Name, parameters,
                    method.ReturnType.GetExtendedName()));
            node.Tag = method;
            node.ImageIndex = node.SelectedImageIndex = 30;
            _currentNode = node;
        }

        private static void AddProperty(PropertyDef property)
        {
            TreeNode node = _currentNode.Nodes.Add(string.Format(property.Name));
            node.Tag = property;
            node.ImageIndex = node.SelectedImageIndex = 43;
            _currentNode = node;

            if (property.GetMethod != null)
            {
                string type = property.GetMethod.ReturnType.GetExtendedName();

                AddMethod(property.GetMethod);
                node.Text = string.Format("{0}: {1}", property.Name, type);
                _currentNode = node;
            }

            if (property.SetMethod != null)
            {
                AddMethod(property.SetMethod);
                _currentNode = node;
            }
            foreach (MethodDef method in property.OtherMethods)
            {
                AddMethod(method);
                _currentNode = node;
            }

            _currentNode = node;
        }

        private static void AddEvent(EventDef eventDef)
        {
            TreeNode node = _currentNode.Nodes.Add(string.Format("{0}: {1}", eventDef.Name, "EventHandler"));

            node.Tag = eventDef;
            node.ImageIndex = node.SelectedImageIndex = 15;
            _currentNode = node;

            if (eventDef.AddMethod != null)
            {
                AddMethod(eventDef.AddMethod);
                _currentNode = node;
            }

            if (eventDef.RemoveMethod != null)
            {
                AddMethod(eventDef.RemoveMethod);
                _currentNode = node;
            }
            if (eventDef.InvokeMethod != null)
            {
                AddMethod(eventDef.InvokeMethod);
                _currentNode = node;
            }
            foreach (MethodDef method in eventDef.OtherMethods)
            {
                AddMethod(method);
                _currentNode = node;
            }
        }

        private static void AddField(FieldDef field)
        {
            string type = field.FieldType.GetExtendedName();

            TreeNode node =
                _currentNode.Nodes.Add(string.Format("{0}: {1}", field.Name, type));
            node.Tag = field;
            node.ImageIndex = node.SelectedImageIndex = 17;
            _currentNode = node;
        }

        #endregion AddNode
    }
}