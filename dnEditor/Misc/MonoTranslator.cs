using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using dnEditor.Forms;
using dnEditor.Misc.ILSpy;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Mono.Cecil;

namespace dnEditor.Misc
{
    public static class MonoTranslator
    {
        public static AssemblyDefinition Translate(ModuleDefMD manifestModule)
        {
            using (var assemblyStream = new MemoryStream())
            {
                try
                {
                    if (manifestModule.IsILOnly)
                    {
                        var writerOptions = new ModuleWriterOptions(manifestModule);
                        writerOptions.Logger = DummyLogger.NoThrowInstance;

                        MetaDataOptions metaDataOptions = new MetaDataOptions();
                        metaDataOptions.Flags = MetaDataFlags.PreserveAll;

                        manifestModule.Write(assemblyStream, writerOptions);
                    }
                    else
                    {
                        var writerOptions = new NativeModuleWriterOptions(manifestModule);
                        writerOptions.Logger = DummyLogger.NoThrowInstance;

                        MetaDataOptions metaDataOptions = new MetaDataOptions();
                        metaDataOptions.Flags = MetaDataFlags.PreserveAll;

                        manifestModule.NativeWrite(assemblyStream, writerOptions);
                    }
                }
                catch (Exception)
                {
                    if (assemblyStream.Length == 0)
                        return null;
                }

                assemblyStream.Position = 0;
                AssemblyDefinition newAssembly = AssemblyDefinition.ReadAssembly(assemblyStream);

                return newAssembly;
            }
        }

        public class Decompiler
        {
            private readonly BackgroundWorker _worker = new BackgroundWorker();

            public void Start()
            {
                _worker.DoWork += worker_DoWork;
                _worker.RunWorkerAsync();
            }

            private void worker_DoWork(object sender, DoWorkEventArgs e)
            {
                MainForm.RtbILSpy.BeginInvoke(new MethodInvoker(() =>
                {
                    MainForm.RtbILSpy.Clear();
                    MainForm.RtbILSpy.Text = Environment.NewLine + "Decompiling..." + Environment.NewLine +
                                             "First time might take a while.";
                }));

                try
                {
                    AssemblyDefinition assembly = Translate(MainForm.CurrentAssembly.ManifestModule);

                    if (assembly == null)
                        throw new Exception("Could not write assembly to stream!");

                    var dnMethod = new MonoMethod(MainForm.CurrentAssembly.Method.NewMethod);
                    object method = dnMethod.Method(assembly);

                    if (method == null)
                        return;

                    var mtp = (IMetadataTokenProvider) method;
                    method = assembly.MainModule.LookupToken(mtp.MetadataToken);

                    if (method == null || string.IsNullOrEmpty(method.ToString()))
                    {
                        MainForm.RtbILSpy.BeginInvoke(new MethodInvoker(
                            () =>
                            {
                                MainForm.RtbILSpy.Clear();
                                MainForm.RtbILSpy.Text = Environment.NewLine +
                                                         "Could not find member by Metadata Token!";
                            }));

                        return;
                    }

                    DefaultAssemblyResolver bar = GlobalAssemblyResolver.Instance;
                    bool savedRaiseResolveException = true;
                    try
                    {
                        if (bar != null)
                        {
                            savedRaiseResolveException = bar.RaiseResolveException;
                            bar.RaiseResolveException = false;
                        }

                        var il = new ILSpyDecompiler();
                        string source = il.Decompile(method);

                        MainForm.RtbILSpy.BeginInvoke(new MethodInvoker(() =>
                        {
                            MainForm.RtbILSpy.Clear();
                            MainForm.RtbILSpy.Rtf = source;
                        }));
                    }
                    finally
                    {
                        if (bar != null)
                            bar.RaiseResolveException = savedRaiseResolveException;
                    }
                }
                catch (Exception o)
                {
                    MainForm.RtbILSpy.BeginInvoke(new MethodInvoker(
                        () =>
                        {
                            MainForm.RtbILSpy.Clear();
                            MainForm.RtbILSpy.Text = Environment.NewLine + "Decompilation unsuccessful!" +
                                                     Environment.NewLine +
                                                     Environment.NewLine + o.Message;
                        }));
                }
            }
        }

        public class MonoMethod
        {
            private readonly string _methodPath;
            private readonly string _typePath;

            public MonoMethod(MethodDef method)
            {
                _methodPath = method.FullName;
                _typePath = method.DeclaringType.FullName;
            }

            public MethodDefinition Method(AssemblyDefinition assembly)
            {
                TypeDefinition type = assembly.MainModule.GetType(_typePath);
                MethodDefinition method = type.Methods.First(m => m.FullName == _methodPath);

                return method;
            }
        }
    }
}