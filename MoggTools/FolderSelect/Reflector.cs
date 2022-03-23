using System;
using System.Reflection;

namespace FolderSelect
{
    public class Reflector
    {
        private string m_ns;

        private Assembly m_asmb;

        public Reflector(string ns) : this(ns, ns)
        {
        }

        public Reflector(string an, string ns)
        {
            this.m_ns = ns;
            this.m_asmb = null;
            AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                AssemblyName assemblyName = referencedAssemblies[i];
                if (assemblyName.FullName.StartsWith(an))
                {
                    this.m_asmb = Assembly.Load(assemblyName);
                    return;
                }
            }
        }

        public object Call(object obj, string func, params object[] parameters)
        {
            return this.Call2(obj, func, parameters);
        }

        public object Call2(object obj, string func, object[] parameters)
        {
            return this.CallAs2(obj.GetType(), obj, func, parameters);
        }

        public object CallAs(Type type, object obj, string func, params object[] parameters)
        {
            return this.CallAs2(type, obj, func, parameters);
        }

        public object CallAs2(Type type, object obj, string func, object[] parameters)
        {
            return type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj, parameters);
        }

        public object Get(object obj, string prop)
        {
            return this.GetAs(obj.GetType(), obj, prop);
        }

        public object GetAs(Type type, object obj, string prop)
        {
            return type.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj, null);
        }

        public object GetEnum(string typeName, string name)
        {
            return this.GetType(typeName).GetField(name).GetValue(null);
        }

        public Type GetType(string typeName)
        {
            Type type = null;
            string[] strArrays = typeName.Split(new char[] { '.' });
            if (strArrays.Length != 0)
            {
                type = this.m_asmb.GetType(string.Concat(this.m_ns, ".", strArrays[0]));
            }
            for (int i = 1; i < strArrays.Length; i++)
            {
                type = type.GetNestedType(strArrays[i], BindingFlags.NonPublic);
            }
            return type;
        }

        public object New(string name, params object[] parameters)
        {
            object obj;
            ConstructorInfo[] constructors = this.GetType(name).GetConstructors();
            int num = 0;
        Label1:
            while (num < constructors.Length)
            {
                ConstructorInfo constructorInfo = constructors[num];
                try
                {
                    obj = constructorInfo.Invoke(parameters);
                }
                catch
                {
                    goto Label0;
                }
                return obj;
            }
            return null;
        Label0:
            num++;
            goto Label1;
        }
    }
}