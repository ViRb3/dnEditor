using System;
using System.IO;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    public class CurrentAssembly
    {
        public AssemblyDef Assembly;

        public Instruction Instruction;
        public MethodHolder Method = new MethodHolder();
        public string Path;

        public CurrentAssembly(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Assembly does not exist!");

            Path = path;
            OpenAssembly();
        }

        public CurrentAssembly(AssemblyDef assembly)
        {
            Assembly = assembly;
            Path = null;
        }

        public ModuleDefMD Module
        {
            get { return (ModuleDefMD) Assembly.ManifestModule; }
        }

        public void OpenAssembly(string path = null)
        {
            try
            {
                if (path == null)
                    Assembly = AssemblyDef.Load(Path);
                else
                {
                    Assembly = AssemblyDef.Load(path);
                    Path = path;
                }
            }
            catch (BadImageFormatException e)
            {
                MessageBox.Show(e.Message, "Error loading assembly!");
                Assembly = null;
            }
        }
    }
}