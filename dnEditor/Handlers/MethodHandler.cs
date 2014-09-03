using System.Windows.Forms;
using dnEditor.Misc;
using dnlib.DotNet;

namespace dnEditor.Handlers
{
    public static class MethodHandler
    {
        public static TreeNode FindMethod(TreeView treeView, MethodDef method)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                TreeNode nodeResult = Functions.FindMethod(node, method);

                if (nodeResult != null)
                    return nodeResult;
            }
            return null;
        }
    }
}