using System.Collections.Generic;
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
            RefNode = null;
            CurrentModule = null;

            if (clear)
                treeView.Nodes.Clear();

            TreeNode file = NewFile(currentAssembly); // AssemblyDef
            file.AddTo(treeView);

            foreach (ModuleDefMD module in currentAssembly.Modules)
            {
                NameSpaceList.Clear();

                TreeNode moduleNode = NewModule(module);

                moduleNode.AddTo(file);

                CurrentModule = moduleNode;

                if (module.Types.Any())
                {
                    var processor = new NodeHandler(moduleNode);
                    processor.ProcessNode();
                    moduleNode = processor.Node;
                }

                CurrentModule = moduleNode;

                if (module.GetAssemblyRefs().Any())
                {
                    ReferenceHandler.HandleReferences(module.GetAssemblyRefs());
                }
            }

            var processor2 = new NodeHandler(RefNode);
            processor2.ProcessNode();
            RefNode = processor2.Node;
        }

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