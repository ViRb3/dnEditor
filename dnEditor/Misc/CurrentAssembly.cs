using System;
using System.IO;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    public class CurrentAssembly
    {
        public ModuleDefMD ManifestModule;

        public Instruction Instruction;
        public MethodHolder Method = new MethodHolder();
        public string Path;
        public bool IsExecutable;

        public CurrentAssembly(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Assembly does not exist!");

            Path = path;

            if (System.IO.Path.GetExtension(path) == ".exe")
                IsExecutable = true;

            OpenAssembly();
        }

        public CurrentAssembly(ModuleDefMD manifestModule)
        {
            ManifestModule = manifestModule;
            Path = null;
        }

        public void OpenAssembly(string path = null)
        {
            try
            {
                if (path == null)
                    ManifestModule = ModuleDefMD.Load(Path);
                else
                {
                    ManifestModule = ModuleDefMD.Load(path);
                    Path = path;
                }
            }
            catch (BadImageFormatException e)
            {
                MessageBox.Show(e.Message, "Error loading assembly!");
                ManifestModule = null;
            }
        }
    }
}