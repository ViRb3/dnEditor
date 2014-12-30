using System;

namespace dnEditor.Misc
{
    internal interface ITreeMenu
    {
        void treeMenu_Opened(object sender, EventArgs e);

        void goToEntryPointToolStripMenuItem_Click(object sender, EventArgs e);

        void goToModuleCtorToolStripMenuItem_Click(object sender, EventArgs e);

        void expandToolStripMenuItem_Click(object sender, EventArgs e);

        void collapseToolStripMenuItem_Click(object sender, EventArgs e);

        void collapseAllToolStripMenuItem_Click(object sender, EventArgs e);

        void closeToolStripMenuItem_Click(object sender, EventArgs e);
    }
}