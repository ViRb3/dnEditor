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
            if (TreeViewHandler.RefNode == null)
            {
                TreeViewHandler.RefNode = TreeViewHandler.CurrentModule.Nodes.Add("References");
                TreeViewHandler.RefNode.ImageIndex = TreeViewHandler.RefNode.SelectedImageIndex = 44;
            }

            var assemblyRefs = new List<AssemblyRef>();

            if (TreeViewHandler.RefNode.Tag as AssemblyRef[] != null)
                assemblyRefs = (TreeViewHandler.RefNode.Tag as AssemblyRef[]).ToList();

            assemblyRefs.AddRange(references.ToArray());

            TreeViewHandler.RefNode.Tag = assemblyRefs.ToArray();
        }

        public static void ProcessAssemblyRefs(out List<TreeNode> children)
        {
            if (TreeViewHandler.RefNode == null)
            {
                children = new List<TreeNode>();
                return;
            }

            children = new List<TreeNode>();
            var refs = TreeViewHandler.RefNode.Tag as AssemblyRef[];

            foreach (AssemblyRef @ref in refs)
                children.Add(TreeViewHandler.NewAssemblyRef(@ref));
        }
    }
}