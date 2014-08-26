using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public class NodeHandler
    {
        public TreeNode Node;

        public NodeHandler(TreeNode node)
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
                    TypeHandler.HandleType(type);
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
                TreeNode fullAccessChild = child; // Get rid of readonly exception

                if (fullAccessChild.Tag is TypeDef)
                {
                    var type = fullAccessChild.Tag as TypeDef;

                    if (type.IsNested)
                    {
                        TypeHandler.HandleType(fullAccessChild.Tag as TypeDef);
                        continue;
                    }
                }

                Node.Nodes.Add(fullAccessChild);
            }
        }
    }
}
