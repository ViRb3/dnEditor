using System.Collections.Generic;
using dnlib.DotNet;

namespace dnEditor.Misc
{
    public class MemberPath
    {
        public List<object> Path = new List<object>();

        public MemberPath(object member)
        {
            if (member is TypeDef)
            {
                GetTypePath(member);
            }
            else if (member is MethodDef)
            {
                GetMethodPath(member);
            }
            else if (member is FieldDef)
            {
                GetFieldPath(member);
            }
            else if (member is PropertyDef)
            {
                GetPropertyPath(member);
            }
            else if (member is EventDef)
            {
                GetEventPath(member);
            }

            Path.Reverse();
        }

        private void GetEventPath(object member)
        {
            var @event = (EventDef) member;

            Path.Add(@event);

            if (@event.DeclaringType != null)
            {
                Path.Add(@event.DeclaringType);
                TypeDef type = @event.DeclaringType;

                while (type.DeclaringType != null)
                {
                    Path.Add(type.DeclaringType);
                    type = type.DeclaringType;
                }
            }
        }

        private void GetPropertyPath(object member)
        {
            var property = (PropertyDef) member;

            Path.Add(property);

            if (property.DeclaringType != null)
            {
                Path.Add(property.DeclaringType);
                TypeDef type = property.DeclaringType;

                while (type.DeclaringType != null)
                {
                    Path.Add(type.DeclaringType);
                    type = type.DeclaringType;
                }
            }
        }

        private void GetFieldPath(object member)
        {
            var field = (FieldDef) member;

            Path.Add(field);

            if (field.DeclaringType != null)
            {
                Path.Add(field.DeclaringType);
                TypeDef type = field.DeclaringType;

                while (type.DeclaringType != null)
                {
                    Path.Add(type.DeclaringType);
                    type = type.DeclaringType;
                }
            }
        }

        private void GetMethodPath(object member)
        {
            var method = (MethodDef) member;

            Path.Add(method);

            if (method.DeclaringType != null)
            {
                Path.Add(method.DeclaringType);
                TypeDef type = method.DeclaringType;

                while (type.DeclaringType != null)
                {
                    Path.Add(type.DeclaringType);
                    type = type.DeclaringType;
                }
            }
        }

        private void GetTypePath(object member)
        {
            var type = (TypeDef) member;

            Path.Add(type);

            while (type.DeclaringType != null)
            {
                Path.Add(type.DeclaringType);
                type = type.DeclaringType;
            }
        }
    }
}