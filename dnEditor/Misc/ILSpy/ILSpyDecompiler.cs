using System;
using ICSharpCode.Decompiler;
using ICSharpCode.ILSpy;
using Mono.Cecil;

namespace dnEditor.Misc.ILSpy
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public class ILSpyDecompiler
    {
        public string Decompile(object @object)
        {
            if (@object == null) return String.Empty;
            Language l = new CSharpLanguage();

            ITextOutput output = new RtfTextOutput();
            var options = new DecompilationOptions();
            
            if (@object is AssemblyDefinition)
                l.DecompileAssembly((AssemblyDefinition)@object, output, options);
            else if (@object is TypeDefinition)
                l.DecompileType((TypeDefinition)@object, output, options);
            else if (@object is MethodDefinition)
                l.DecompileMethod((MethodDefinition)@object, output, options);
            else if (@object is FieldDefinition)
                l.DecompileField((FieldDefinition)@object, output, options);
            else if (@object is PropertyDefinition)
                l.DecompileProperty((PropertyDefinition)@object, output, options);
            else if (@object is EventDefinition)
                l.DecompileEvent((EventDefinition)@object, output, options);
            else if (@object is AssemblyNameReference)
            {
                output.Write("// Assembly Reference ");
                output.WriteDefinition(@object.ToString(), null);
                output.WriteLine();
            }
            else if(@object is ModuleReference)
            {
                output.Write("// Module Reference ");
                output.WriteDefinition(@object.ToString(), null);
                output.WriteLine();
            }
            else
            {
                output.Write(String.Format("// {0} ", @object.GetType().Name));
                output.WriteDefinition(@object.ToString(), null);
                output.WriteLine();
            }                

            return output.ToString();
        }

    }
}
