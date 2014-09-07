using System.Windows.Forms;

namespace dnEditor.Misc
{
    interface ITreeView
    {
        void treeView_AfterExpand(object sender, TreeViewEventArgs e);
        void treeView_DragDrop(object sender, DragEventArgs e);
        void treeView_DragEnter(object sender, DragEventArgs e);
        void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e);
        void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e);
    }
}
