using dnlib.DotNet;

namespace dnEditor.Misc
{
    public class MethodHolder
    {
        private readonly MethodDef _originalMethod;
        public MethodDef NewMethod;

        public MethodHolder()
        {
        }

        public MethodHolder(MethodDef method)
        {
            NewMethod = _originalMethod = method;
        }


        public MethodDef OriginalMethod
        {
            get { return _originalMethod; }
        }
    }
}