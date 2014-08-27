using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    public static class Functions
    {
        public static Dictionary<OpCode, string> OpCodeDictionary;
        public static List<OpCode> OpCodes;

        static Functions()
        {
            LoadOpCodes();
            UpdateOpCodeDictionary();
        }

        public static string GetOpCodeDefinition(string opCode)
        {
            OpCode result = GetOpCode(opCode);

            if (result == null) return null;

            string definition;
            OpCodeDictionary.TryGetValue(result, out definition);

            return definition;
        }

        public static string GetOpCodeDefinition(OpCode opCode)
        {
            string definition;
            OpCodeDictionary.TryGetValue(opCode, out definition);

            return definition;
        }

        public static OpCode GetOpCode(string opCode)
        {
            OpCode result = OpCodes.FirstOrDefault(opcode => opcode.Name == opCode);
            return result;
        }

        public static void UpdateOpCodeDictionary()
        {
            if (OpCodeDictionary == null)
                OpCodeDictionary = new Dictionary<OpCode, string>();
            else
                OpCodeDictionary.Clear();

            string[] dictionary = Regex.Split(File.ReadAllText("MSIL Dictionary.txt"), Environment.NewLine);

            foreach (string line in dictionary)
            {
                string[] items = Regex.Split(line, "=");
                OpCode result = OpCodes.FirstOrDefault(opCode => opCode.Name.ToLower() == items[0].ToLower());
                if (result != null)
                    OpCodeDictionary.Add(result, items[1]);
            }
        }

        public static void LoadOpCodes()
        {
            if (OpCodes == null)
                OpCodes = new List<OpCode>();
            else
                OpCodes.Clear();

            Type type = typeof (OpCodes);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.Name != "OpCode") continue;
                var opCode =
                    (OpCode)
                        type.InvokeMember(field.Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField,
                            null, null, null);
                OpCodes.Add(opCode);
            }

            OpCodes = OpCodes.OrderBy(o => o.Name).ToList();
        }

        public static string GetAddress(Instruction instruction)
        {
            return String.Format("L_{0:x04}", instruction.Offset);
        }

        public static string GetAddress(uint offset)
        {
            return String.Format("L_{0:x04}", offset);
        }

        public static string FormatInstruction(List<Instruction> instructions, int index)
        {
            Instruction currentInstruction = instructions[index];

            string output = string.Format("({0}) {1}", instructions.IndexOf(currentInstruction), currentInstruction.OpCode);

            if (currentInstruction.Operand != null)
            {
                if (currentInstruction.Operand is Instruction)
                {
                    output += string.Format(" {0}", ResolveOperandInstructions(instructions, index));
                }
                else
                {
                    output += string.Format(" -> {0}", currentInstruction.Operand);
                }
            }

            return output;
        }

        public static string ResolveOperandInstructions(List<Instruction> instructions, int index)
        {
            string output = "";
            Instruction currentInstruction = instructions[index];

            while (currentInstruction.Operand is Instruction)
            {
                var newInstruction = currentInstruction.Operand as Instruction;
                output += string.Format("-> {0}", FormatInstruction(instructions, instructions.IndexOf(newInstruction)));
                currentInstruction = newInstruction;
            }

            return output;
        }

        /*public static HashSet<MethodDef> GetAccessorMethods(this TypeDef type)
        {
            var accessorMethods = new HashSet<MethodDef>();
            foreach (PropertyDef property in type.Properties)
            {
                accessorMethods.Add(property.GetMethod);
                accessorMethods.Add(property.SetMethod);
                if (property.HasOtherMethods)
                {
                    foreach (MethodDef m in property.OtherMethods)
                        accessorMethods.Add(m);
                }
            }
            foreach (EventDef ev in type.Events)
            {
                accessorMethods.Add(ev.AddMethod);
                accessorMethods.Add(ev.RemoveMethod);
                accessorMethods.Add(ev.InvokeMethod);
                if (ev.HasOtherMethods)
                {
                    foreach (MethodDef m in ev.OtherMethods)
                        accessorMethods.Add(m);
                }
            }
            return accessorMethods;
        }*/

        /*public static string GetExtendedName(this TypeSig typeSig)
        {
            var nothing = new List<Dictionary<Dictionary<Int32, Int32>, String>>();

            string name = typeSig.TypeName;

            if (typeSig.ToGenericInstSig() != null)
            {
                name += "<";

                IList<TypeSig> args = typeSig.ToGenericInstSig().GenericArguments; // Dictionary`2 & String

                for (int i = 0; i < args.Count; i++)
                {
                    string newArgs = args[i].GetExtendedName();

                    if (newArgs != string.Empty)
                        name += newArgs;
                    else
                        name += args[i].TypeName;

                    if (i < args.Count - 1)
                        name += ", ";
                }

                name += ">";
            }

            return name;
        }*/
    }
}

/*string type = "";

            type += typeSig.TypeName;

            if (typeSig.ToGenericInstSig() != null)
            {
                IList<TypeSig> args = typeSig.ToGenericInstSig().GenericArguments;

                if (args.Count > 0)
                {
                    type += "<";
                    for (int i = 0; i < args.Count; i++)
                    {
                        if (i != args.Count - 1)
                            type += string.Format("{0}, ", args[i].TypeName);
                        else
                            type += args[i].TypeName + ">";
                    }
                }
            }

            type += ", ";


            return type.TrimEnd(new[] { ',', ' ' });*/