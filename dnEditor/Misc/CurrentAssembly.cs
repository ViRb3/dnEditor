using System;
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
            Path = path;
            LoadAssembly();
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

        public void LoadAssembly(string path = null)
        {
            Assembly = path == null ? AssemblyDef.Load(Path) : AssemblyDef.Load(path);
        }

        /*public void LoadModule(string path = null)
        {
            Module = path == null ? ModuleDefMD.Load(Path) : ModuleDefMD.Load(path);
        }*/
    }
}