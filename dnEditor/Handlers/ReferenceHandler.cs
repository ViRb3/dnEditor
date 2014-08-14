using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public static class ReferenceHandler
    {
        public static void HandleReferences(IEnumerable<AssemblyRef> references)
        {
            TreeViewHandler.RefNode = TreeViewHandler.CurrentModule.Nodes.Add("References");
            TreeViewHandler.RefNode.ImageIndex = TreeViewHandler.RefNode.SelectedImageIndex = 44;

            TreeNode virtualNode = TreeViewHandler.RefNode.FindVirtualNode();

            if (virtualNode == null)
                TreeViewHandler.AddVirtualNodeWithContent(TreeViewHandler.RefNode, references.ToArray());
            else
            {
                List<AssemblyRef> assemblyRefs = (virtualNode.Tag as AssemblyRef[]).ToList();
                assemblyRefs.AddRange(references.ToArray());
                virtualNode.Tag = assemblyRefs.ToArray();
            }
        }

        public static void ProcessAssemblyRefs(TreeNode parentNode, ref List<TreeNode> children)
        {
            children = new List<TreeNode>();
            var refs = parentNode.FindVirtualNode().Tag as AssemblyRef[];

            foreach (AssemblyRef @ref in refs)
                children.Add(TreeViewHandler.NewAssemblyRef(@ref));
        }
    }
}
