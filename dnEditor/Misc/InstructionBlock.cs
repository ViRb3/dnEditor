using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace dnEditor.Misc
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public class InstructionBlock
    {
        public InstructionBlock(int startIndex, int endIndex)
        {
            StartIndex = startIndex;

            EndIndex = endIndex < startIndex ? startIndex : endIndex;
        }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public InstructionBlock NextBlock { get; private set; }

        public int JumpDownRefCount { get; private set; }
        public int JumpUpRefCount { get; private set; }

        public int RefCount
        {
            get { return JumpDownRefCount + JumpUpRefCount; }
        }

        public int Size
        {
            get { return EndIndex - StartIndex + 1; }
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", StartIndex, EndIndex);
        }

        public static List<InstructionBlock> Find(MethodDef method)
        {
            var list = new List<InstructionBlock>();

            if (method == null || !method.HasBody)
                return list;

            IList<Instruction> instructions = method.Body.Instructions;
            if (instructions.Count < 1)
                return list;

            int firstIndex = 0;
            int lastIndex = firstIndex;

            Instruction insLast = instructions[lastIndex];
            while (lastIndex < instructions.Count)
            {
                if (IsBlockDelimiter(insLast) || lastIndex + 1 >= instructions.Count)
                {
                    var instructionBlock = new InstructionBlock(firstIndex, lastIndex);
                    list.Add(instructionBlock);

                    firstIndex = lastIndex + 1;
                    lastIndex = firstIndex;
                }
                else
                {
                    lastIndex++;
                }
                if (lastIndex >= instructions.Count)
                    break;
                insLast = instructions[lastIndex];
            }

            int insCount = 0;
            foreach (InstructionBlock firstBlock in list)
            {
                insCount += firstBlock.EndIndex - firstBlock.StartIndex + 1;

                Instruction insTo = null;
                if (instructions[firstBlock.EndIndex].Operand is Instruction)
                {
                    insTo = instructions[firstBlock.EndIndex].Operand as Instruction;
                }
                if (insTo != null)
                {
                    int to = instructions.IndexOf(insTo);
                    foreach (InstructionBlock nextBlock in list)
                    {
                        if (nextBlock.StartIndex == firstBlock.StartIndex)
                            continue;
                        if (nextBlock.StartIndex <= to && to <= nextBlock.EndIndex)
                        {
                            firstBlock.NextBlock = nextBlock;

                            if (firstBlock.StartIndex < nextBlock.StartIndex)
                                nextBlock.JumpDownRefCount++;
                            else
                                nextBlock.JumpUpRefCount++;
                        }
                    }
                }
            }

            if (insCount != instructions.Count)
            {
                throw new ApplicationException("Internal error in InstructionBlock.Find !");
            }

            return list;
        }

        public static bool IsBlockDelimiter(Instruction instruction)
        {
            if (instruction == null) return false;

            switch (instruction.OpCode.FlowControl)
            {
                case FlowControl.Branch:
                    return true;
                case FlowControl.Return:
                    return true;
                case FlowControl.Throw:
                    return true;
            }

            return false;
        }
    }
}