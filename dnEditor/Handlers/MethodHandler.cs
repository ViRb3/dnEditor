using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public static class MethodHandler
    {
        public static TreeNode FindMethod(TreeView treeView, MethodDef method)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                TreeNode nodeResult = TreeViewHandler.FindMethod(node, method);

                if (nodeResult != null)
                    return nodeResult;
            }
            return null;
        }
    }
}
