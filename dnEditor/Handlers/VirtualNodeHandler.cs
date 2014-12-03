using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public class VirtualNode
    {
        public static string Name = "VIRTNODE";
    }

    public class DeVirtualNode
    {
        public static string Name = "DEVIRTNODE";
    }

    public static class VirtualNodeHandler
    {
        public static int VirtualNodes;
    }

    public class NodeDevirtualizer
    {
        public delegate void EventHandler(TreeNode processedNode);

        private readonly TreeViewHandler _treeViewHandler;
        public TreeNode Node;

        public NodeDevirtualizer(TreeNode node, TreeViewHandler treeViewHandler)
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
                foreach (TypeDef type in (node.Tag as ModuleDefMD).Types.OrderBy(t => t.Name.ToLower()))
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
            if (result != null)
            {
                var children = result[0] as TreeNode[];

                if (children != null && children.Length > 0)
                    foreach (TreeNode child in children)
                    {
                        Node.Nodes.Add(child);
                    }
            }

            if (WorkerFinished != null)
                WorkerFinished.Invoke(Node);
        }
    }

    public static class VirtualNodeUtilities
    {
        public static TreeNode NewVirtualNode()
        {
            var node = new TreeNode(VirtualNode.Name)
            {
                Tag = new VirtualNode()
            };

            VirtualNodeHandler.VirtualNodes++;

            return node;
        }

        public static TreeNode NewDeVirtualNode()
        {
            var node = new TreeNode(DeVirtualNode.Name)
            {
                Tag = new DeVirtualNode()
            };

            return node;
        }

        public static TreeNode FindVirtualNode(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == VirtualNode.Name && n.Tag is VirtualNode);
        }

        public static TreeNode FindDeVirtualNode(this TreeNode node)
        {
            return node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == DeVirtualNode.Name && n.Tag is DeVirtualNode);
        }

        public static bool HasVirtualNode(this TreeNode node)
        {
            if (node.FindVirtualNode() !=  null)
                return true;

            return false;
        }

        public static bool HasDeVirtualNode(this TreeNode node)
        {
            if (node.FindDeVirtualNode() != null)
                return true;

            return false;
        }

        public static void ExpandHandler(TreeNode expandedNode, TreeViewHandler treeViewHandler)
        {
            if (expandedNode.HasVirtualNode())
            {
                var processor = new NodeDevirtualizer(expandedNode, treeViewHandler);
                processor.ProcessNode();
                VirtualNodeHandler.VirtualNodes--;
                expandedNode.Nodes.Remove(expandedNode.FindVirtualNode());
            }
        }

        public static void Virtualize(this TreeNode node)
        {
            node.Nodes.Clear();
            NewVirtualNode().AddTo(node);
        }
    }
}