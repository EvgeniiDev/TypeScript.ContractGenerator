using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class TypeInfo : ITypeInfo
    {
        private TypeInfo(Type type)
        {
            Type = type;
        }

        private TypeInfo(Type type, NullabilityInfo? nullabilityInfo)
        {
            Type = type;
            NullabilityInfo = nullabilityInfo;
        }

        public static ITypeInfo From<T>()
        {
            return new TypeInfo(typeof(T));
        }

        public static ITypeInfo From(Type type)
        {
            return type == null ? null : new TypeInfo(type);
        }

        public Type Type { get; }
        private NullabilityInfo? NullabilityInfo { get; }

        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string Namespace => Type.Namespace;
        public bool IsEnum => Type.IsEnum;
        public bool IsValueType => Type.IsValueType;
        public bool IsArray => Type.IsArray;
        public bool IsClass => Type.IsClass;
        public bool IsInterface => Type.IsInterface;
        public bool IsAbstract => Type.IsAbstract;
        public bool IsGenericType => Type.IsGenericType;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
        public ITypeInfo? BaseType => From(Type.BaseType);

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return Type.GetMethods(bindingAttr).Select(x => (IMethodInfo)new MethodWrapper(x)).ToArray();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return Type.GetProperties(bindingAttr).Select(x => (IPropertyInfo)new PropertyWrapper(x)).ToArray();
        }

        public IFieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return Type.GetFields(bindingAttr).Select(x => (IFieldInfo)new FieldWrapper(x)).ToArray();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            return Type.GetGenericArguments().Select(From).ToArray();
        }

        public ITypeInfo[] GetInterfaces()
        {
            return Type.GetInterfaces().Select(From).ToArray();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            return From(Type.GetGenericTypeDefinition());
        }

        public ITypeInfo GetElementType()
        {
            return From(Type.GetElementType()).WithNullabilityInfo(NullabilityInfo?.ForItem());
        }

        public ITypeInfo WithNullabilityInfo(NullabilityInfo? info)
        {
            return new TypeInfo(Type, info);
        }

        public string[] GetEnumNames()
        {
            return Type.GetEnumNames();
        }

        public bool CanBeNull(NullabilityMode nullabilityMode)
        {
            return NullabilityInfo.CanBeNull(nullabilityMode);
        }

        public bool IsAssignableFrom(ITypeInfo type)
        {
            return TypeInfoHelpers.IsAssignableFrom(this, type);
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return Type.GetAttributes(inherit);
        }

        public bool Equals(ITypeInfo other)
        {
            return TypeInfoHelpers.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ITypeInfo typeInfo && Equals(typeInfo);
        }

        public override int GetHashCode()
        {
            return TypeInfoHelpers.GetHashCode(this);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}