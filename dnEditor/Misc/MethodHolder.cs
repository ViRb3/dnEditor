using dnlib.DotNet;

namespace dnEditor.Misc
{
    public class MethodHolder
    {
        private readonly MethodDef _originalMethod;
        public MethodDef Method;

        public MethodHolder()
        {
        }

        public MethodHolder(MethodDef method)
        {
            Method = _originalMethod = method;
        }


        public MethodDef OriginalMethod
        {
            get { return _originalMethod; }
        }
    }
}