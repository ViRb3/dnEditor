using System;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace dnEditor.Forms
{
    public partial class WriteAssemblyForm : Form
    {
        private readonly ModuleDefMD _module;

        public WriteAssemblyForm(ModuleDefMD module)
        {
            _module = module;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPath.Text))
                return;

            try
            {
                if (_module.IsILOnly)
                {
                    ModuleWriterOptions writer = new ModuleWriterOptions(_module);

                    if (chkNoThrowInstanceLogger.Checked)
                        writer.Logger = DummyLogger.NoThrowInstance;

                    MetaDataOptions metaDataOptions = writer.MetaDataOptions;

                    #region MetaDataSetters

                    if (chkPreserveTypeRefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeRefRids;
                    if (chkPreserveTypeDefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeDefRids;
                    if (chkPreserveFieldRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveFieldRids;
                    if (chkPreserveMethodRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMethodRids;
                    if (chkPreserveParamRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveParamRids;
                    if (chkPreserveMemberRefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMemberRefRids;
                    if (chkPreserveStandAloneSigRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveStandAloneSigRids;
                    if (chkPreserveEventRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveEventRids;
                    if (chkPreservePropertyRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreservePropertyRids;
                    if (chkPreserveTypeSpecRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeSpecRids;
                    if (chkPreserveMethodSpecRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMethodSpecRids;
                    if (chkPreserveAllMethodRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveAllMethodRids;
                    if (chkPreserveRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveRids;
                    if (chkPreserveStringOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveStringsOffsets;
                    if (chkPreserveUSOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveUSOffsets;
                    if (chkPreserveBlobOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveBlobOffsets;
                    if (chkPreserveExtraSignatureData.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveExtraSignatureData;
                    if (chkPreserveAll.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveAll;
                    if (chkKeepOldMaxStack.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.KeepOldMaxStack;
                    if (chkAlwaysCreateGuidHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateGuidHeap;
                    if (chkAlwaysCreateStringsHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateStringsHeap;
                    if (chkAlwaysCreateUSHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateUSHeap;
                    if (chkAlwaysCreateBlobHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateBlobHeap;

                    #endregion

                    _module.Write(txtPath.Text, writer);

                }
                else
                {
                    NativeModuleWriterOptions writer = new NativeModuleWriterOptions(_module);

                    if (chkNoThrowInstanceLogger.Checked)
                        writer.Logger = DummyLogger.NoThrowInstance;

                    MetaDataOptions metaDataOptions = writer.MetaDataOptions;

                    #region MetaDataSetters

                    if (chkPreserveTypeRefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeRefRids;
                    if (chkPreserveTypeDefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeDefRids;
                    if (chkPreserveFieldRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveFieldRids;
                    if (chkPreserveMethodRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMethodRids;
                    if (chkPreserveParamRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveParamRids;
                    if (chkPreserveMemberRefRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMemberRefRids;
                    if (chkPreserveStandAloneSigRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveStandAloneSigRids;
                    if (chkPreserveEventRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveEventRids;
                    if (chkPreservePropertyRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreservePropertyRids;
                    if (chkPreserveTypeSpecRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveTypeSpecRids;
                    if (chkPreserveMethodSpecRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveMethodSpecRids;
                    if (chkPreserveAllMethodRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveAllMethodRids;
                    if (chkPreserveRids.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveRids;
                    if (chkPreserveStringOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveStringsOffsets;
                    if (chkPreserveUSOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveUSOffsets;
                    if (chkPreserveBlobOffsets.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveBlobOffsets;
                    if (chkPreserveExtraSignatureData.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveExtraSignatureData;
                    if (chkPreserveAll.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.PreserveAll;
                    if (chkKeepOldMaxStack.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.KeepOldMaxStack;
                    if (chkAlwaysCreateGuidHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateGuidHeap;
                    if (chkAlwaysCreateStringsHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateStringsHeap;
                    if (chkAlwaysCreateUSHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateUSHeap;
                    if (chkAlwaysCreateBlobHeap.Checked)
                        metaDataOptions.Flags = metaDataOptions.Flags | MetaDataFlags.AlwaysCreateBlobHeap;

                    #endregion

                    _module.NativeWrite(txtPath.Text, writer);
                }
            }
            catch (Exception o)
                {
                    MessageBox.Show("Could not write assembly!" + Environment.NewLine + Environment.NewLine + o.Message,
                        "Error");
                    return;
                }

            MessageBox.Show("Assembly written to:" + Environment.NewLine + txtPath.Text, "Success");

            Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Choose a location to write the new assembly...",
                Filter = "Executable Files (*.exe)|*.exe|DLL Files (*.dll)|*.dll"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                txtPath.Text = dialog.FileName;
        }
    }
}
