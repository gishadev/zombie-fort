using System;
using System.Reflection;

namespace gishadev.tools.Core
{
    public static class InstanceCreator
    {
        public static T CreateInstanceWithArgs<T>(params object[] constructorArgs)
        {
            Type typeToCreate = typeof(T);
            ConstructorInfo constructor = typeToCreate.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                GetConstructorParameterTypes(constructorArgs),
                null
            );

            if (constructor != null)
            {
                return (T)constructor.Invoke(constructorArgs);
            }
            else
            {
                throw new InvalidOperationException($"No suitable constructor found for type {typeof(T)}");
            }
        }

        private static Type[] GetConstructorParameterTypes(object[] constructorArgs)
        {
            Type[] paramTypes = new Type[constructorArgs.Length];
            for (int i = 0; i < constructorArgs.Length; i++)
            {
                paramTypes[i] = constructorArgs[i].GetType();
            }
            return paramTypes;
        }
    }
}