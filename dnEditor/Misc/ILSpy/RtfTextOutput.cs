using System;
using System.Collections;
using System.IO;
using ICSharpCode.NRefactory;
using Mono.Cecil;

namespace ICSharpCode.Decompiler
{
    /*
     * FULL CREDITS OF THIS CLASS GO TO THE CODER OF SIMPLE ASSEMBLY EXPLORER.
     * I FULLY COPIED IT AND TAKE NO CREDIT FOR HIS WORK!
     */

    public class RtfTextOutput : ITextOutput
    {
        private const int Column = 1;
        private const int Line = 1;
        private readonly ArrayList _colors = new ArrayList();
        private readonly TextWriter _formatter;
        private int _indent;
        private bool _needsIndent;

        public RtfTextOutput(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            _formatter = writer;
            _colors.Add(0x000000);
            CurrentLine = 1;
        }

        public RtfTextOutput()
            : this(new StringWriter())
        {
        }

        public int CurrentLine { get; set; }
        public int CurrentColumn { get; set; }

        public void Indent()
        {
            _indent++;
        }

        public void Unindent()
        {
            _indent--;
        }

        public void Write(char @char)
        {
            WriteIndent();
            WriteQuote(@char.ToString());
        }

        public void Write(string text)
        {
            WriteIndent();
            WriteQuote(text);
        }

        public void WriteLine()
        {
            _formatter.Write(@"\par\li150");
            _needsIndent = true;
            ++CurrentLine;
        }

        public void WriteDefinition(string text, object definition, bool isLocal = false)
        {
            _formatter.Write(@"\b ");
            Write(text);
            _formatter.Write(@"\b0 ");
        }

        public void WriteReference(string text, object reference, bool isLocal = false)
        {
            WriteIndent();

            bool handled = false;
            if (reference is TypeReference)
            {
                var typeReference = (TypeReference) reference;
                if (!IsSystemType(typeReference.FullName))
                {
                    if (!(typeReference is TypeDefinition))
                    {
                        try
                        {
                            typeReference = typeReference.Resolve();
                        }
                        catch
                        {
                        }
                    }
                    if (typeReference is TypeDefinition)
                    {
                        WriteMetadataItem(text, (IMetadataTokenProvider) reference);
                        handled = true;
                    }
                }
            }
            else if (reference is MemberReference)
            {
                var memberReference = (MemberReference) reference;
                TypeReference typeReference = memberReference.DeclaringType;
                if (typeReference == null || (typeReference != null && !IsSystemType(typeReference.FullName)))
                {
                    WriteMetadataItem(text, memberReference);
                    handled = true;
                }
            }
            if (!handled)
            {
                WriteColor(text, 0x006018);
            }
        }

        public void MarkFoldStart(string collapsedText = "...", bool defaultCollapsed = false)
        {
        }

        public void MarkFoldEnd()
        {
        }

        public void AddDebuggerMemberMapping(MemberMapping memberMapping)
        {
        }

        public TextLocation Location
        {
            get { return new TextLocation(Line, Column + (_needsIndent ? _indent : 0)); }
        }

        public override string ToString()
        {
            var writer = new StringWriter();

            writer.Write(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033");
            writer.Write(@"{\colortbl ");

            foreach (int color in _colors)
            {
                writer.Write("\\red");
                writer.Write(((color >> 16) & 0xff).ToString());
                writer.Write("\\green");
                writer.Write(((color >> 8) & 0xff).ToString());
                writer.Write("\\blue");
                writer.Write((color & 0xff).ToString());
                writer.Write(";");
            }

            writer.Write("}");
            writer.Write(@"\par\li150");
            writer.Write(@"\cf0");
            writer.Write(" ");
            writer.Write(_formatter.ToString());
            writer.Write(@"\par");
            writer.Write("}");

            return writer.ToString();
        }

        private void WriteIndent()
        {
            if (_needsIndent)
            {
                _needsIndent = false;
                for (int i = 0; i < _indent; i++)
                {
                    WriteQuote("    ");
                }
            }
        }

        public void WriteColor(string text, int color)
        {
            WriteIndent();

            int index = _colors.IndexOf(color);
            if (index == -1)
            {
                index = _colors.Count;
                _colors.Add(color);
            }

            _formatter.Write("\\cf" + index + " ");
            WriteQuote(text);
            _formatter.Write("\\cf0 ");
        }

        private void WriteMetadataItem(string text, IMetadataTokenProvider mi)
        {
            _formatter.Write(@"{{\rtf1 {0}\v #0x{1:x08}\v0}}", Encode(text), mi.MetadataToken.ToUInt32());
        }

        public void WriteQuote(string text)
        {
            _formatter.Write(Encode(text));
        }

        public string Encode(string text)
        {
            using (var writer = new StringWriter())
            {
                CharEnumerator enumerator = text.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    char @char = enumerator.Current;
                    ushort num = @char;

                    if (num <= 0xff)
                    {
                        switch (@char)
                        {
                            case '\\':
                                writer.Write("\\\\");
                                break;
                            case '}':
                                writer.Write("\\}");
                                break;
                            case '{':
                                writer.Write("\\{");
                                break;
                            default:
                                writer.Write(@char);
                                break;
                        }
                    }
                    else
                    {
                        writer.Write(@"\u{0:d}?", num);
                    }
                }
                text = writer.ToString();
            }

            return text;
        }

        public bool IsSystemType(string typeFullName)
        {
            if (String.IsNullOrEmpty(typeFullName))
                return false;

            string name = typeFullName.EndsWith(".") ? typeFullName : String.Format("{0}.", typeFullName);

            return name.StartsWith("System.") || name.StartsWith("Microsoft.");
        }
    }
}