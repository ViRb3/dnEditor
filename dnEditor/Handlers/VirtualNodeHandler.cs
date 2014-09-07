using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public class VirtualNodeHandler
    {
        public delegate void EventHandler(TreeNode processedNode);

        public TreeNode Node;
        private TreeViewHandler _treeViewHandler;

        public VirtualNodeHandler(TreeNode node, TreeViewHandler treeViewHandler)
        {
            Node = node;
            _treeViewHandler = treeViewHandler;
        }

        public event EventHandler WorkerFinished;

        public void ProcessNode()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw.RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            TreeNode node = Node;

            var children = new List<TreeNode>();

            if (node == _treeViewHandler.RefNode || node.Tag is AssemblyRef) // Assembly Reference
            {
                var referenceHandler = new ReferenceHandler(_treeViewHandler);
                referenceHandler.ProcessAssemblyRefs(out children);
            }
            else if (node.Tag is ModuleDefMD) // Module
            {
                foreach (TypeDef type in (node.Tag as ModuleDefMD).Types)
                {
                    new TypeHandler(_treeViewHandler).HandleType(type, true);
                }
            }
            else if (node.Tag is TypeDef) // Type
            {
                new TypeHandler(_treeViewHandler).ProcessTypeMembers(node, ref children);
            }

            Node = node;
            e.Result = new object[] {children.ToArray()};
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as object[];
            var children = result[0] as TreeNode[];

            if (children == null || children.Length == 0) return;

            foreach (TreeNode child in children)
            {
                Node.Nodes.Add(child);
            }

            if (WorkerFinished != null)
                WorkerFinished.Invoke(Node);
        }
    }

    public static class VirtualNodeUtilities
    {
        public static TreeNode NewVirtualNode()
        {
            var node = new TreeNode(VirtualNode.Name);
            node.Tag = new VirtualNode();

            return node;
        }

        public static TreeNode FindVirtualNode(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == VirtualNode.Name && n.Tag is VirtualNode);
        }

        public static bool HasVirtualNode(this TreeNode node)
        {
            if (node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == VirtualNode.Name && n.Tag is VirtualNode) !=
                null)
                return true;

            return false;
        }

        public static void ExpandHandler(TreeNode expandedNode, TreeViewHandler treeViewHandler)
        {
            if (expandedNode.HasVirtualNode())
            {
                var processor = new VirtualNodeHandler(expandedNode, treeViewHandler);
                processor.ProcessNode();
                expandedNode.Nodes.Remove(expandedNode.FindVirtualNode());
            }
        }

        public static void VirtualizeNode(TreeNode node)
        {
            node.Nodes.Clear();
            NewVirtualNode().AddTo(node);
        }
    }
}