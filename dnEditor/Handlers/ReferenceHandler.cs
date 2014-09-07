using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public class ReferenceHandler
    {
        private readonly TreeViewHandler _treeViewHandler;

        public ReferenceHandler(TreeViewHandler treeViewHandler)
        {
            _treeViewHandler = treeViewHandler;
        }

        public void HandleReferences(IEnumerable<AssemblyRef> references)
        {
            if (_treeViewHandler.RefNode == null)
            {
                _treeViewHandler.RefNode = _treeViewHandler.NewReferenceFolder();
                _treeViewHandler.RefNode.AddTo(_treeViewHandler.CurrentModule);
            }

            var assemblyRefs = new List<AssemblyRef>();

            if (_treeViewHandler.RefNode.Tag as AssemblyRef[] != null)
                assemblyRefs = (_treeViewHandler.RefNode.Tag as AssemblyRef[]).ToList();

            assemblyRefs.AddRange(references.ToArray());

            _treeViewHandler.RefNode.Tag = assemblyRefs.ToArray();
        }

        public void ProcessAssemblyRefs(out List<TreeNode> children)
        {
            if (_treeViewHandler.RefNode == null)
            {
                children = new List<TreeNode>();
                return;
            }

            children = new List<TreeNode>();
            var refs = _treeViewHandler.RefNode.Tag as AssemblyRef[];

            foreach (AssemblyRef @ref in refs)
                children.Add(_treeViewHandler.NewAssemblyRef(@ref));
        }
    }
}