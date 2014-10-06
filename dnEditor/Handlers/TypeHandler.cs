using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;
using dnlib.Utils;

namespace dnEditor.Handlers
{
    public class TypeHandler
    {
        private readonly TreeViewHandler _treeViewHandler;

        public TypeHandler(TreeViewHandler treeViewHandler)
        {
            _treeViewHandler = treeViewHandler;
        }
        public void ProcessTypeMembers(TreeNode parentNode, ref List<TreeNode> children)
        {
            var type = parentNode.Tag as TypeDef;

            foreach (TypeDef nestedType in type.NestedTypes.OrderBy(t => t.Name))
            {
                TreeNode newTypeNode = _treeViewHandler.NewType(nestedType);
                VirtualNodeUtilities.NewVirtualNode().AddTo(newTypeNode);

                children.Add(newTypeNode);
            }

            foreach (MethodDef method in type.Methods.OrderBy(m => m.Name))
            {
                List<MethodDef> accessorMethods = type.GetAccessorMethods();

                if (!accessorMethods.Contains(method))
                    children.Add(_treeViewHandler.NewMethod(method));
            }

            foreach (PropertyDef property in type.Properties.OrderBy(p => p.Name))
                children.Add(_treeViewHandler.NewProperty(property));

            foreach (FieldDef field in type.Fields.OrderBy(f => f.Name))
                children.Add(_treeViewHandler.NewField(field));

            foreach (EventDef @event in type.Events.OrderBy(e => e.Name))
                children.Add(_treeViewHandler.NewEvent(@event));
        }

        public void HandleType(TypeDef type, bool processChildren)
        {
            TreeNode targetType = _treeViewHandler.NewType(type);

            if (processChildren)
            {
                var children = new List<TreeNode>();
                ProcessTypeMembers(targetType, ref children);

                foreach (var child in children)
                    targetType.Nodes.Add(child);
            }

            if (type.IsExpandable())
                VirtualNodeUtilities.NewVirtualNode().AddTo(targetType);

            if (!_treeViewHandler.NameSpaceList.Contains(type.Namespace))
            {
                TreeNode nameSpace = _treeViewHandler.NewNameSpace(type.Namespace);

                _treeViewHandler.NameSpaceList.Add(type.Namespace);

                _treeViewHandler.CurrentTreeView.BeginInvoke(new MethodInvoker(() =>
                {
                    nameSpace.AddTo(_treeViewHandler.CurrentModule);
                    targetType.AddTo(nameSpace);
                }));

            }
            else
            {
                _treeViewHandler.CurrentTreeView.BeginInvoke(new MethodInvoker(() => targetType.AddTo(_treeViewHandler.CurrentModule.Nodes.Cast<TreeNode>().First(n => n.Text == type.Namespace))));
            }
        }
    }
}