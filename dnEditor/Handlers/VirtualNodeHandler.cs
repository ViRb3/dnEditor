using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public class VirtualNodeHandler
    {
        public TreeNode Node;

        public VirtualNodeHandler(TreeNode node)
        {
            Node = node;
        }

        public void ProcessNode()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw.RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var node = Node;

            var children = new List<TreeNode>();

            if (node == TreeViewHandler.RefNode || node.Tag is AssemblyRef) // Assembly Reference
            {
                ReferenceHandler.ProcessAssemblyRefs(out children);
            }
            else if (node.Tag is ModuleDefMD) // Module
            {
                foreach (TypeDef type in (node.Tag as ModuleDefMD).Types)
                {
                    TypeHandler.HandleType(type, true);
                }
            }
            else if (node.Tag is TypeDef) // Type
            {
                TypeHandler.ProcessTypeMembers(node, ref children);
            }

            Node = node;
            e.Result = new object[] { children.ToArray() };
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

        public static void ExpandHandler(TreeNode expandedNode)
        {
            if (expandedNode.HasVirtualNode())
            {
                var processor = new VirtualNodeHandler(expandedNode);
                processor.ProcessNode();
                expandedNode.Nodes.Remove(expandedNode.FindVirtualNode());
            }
        }
    }
}
