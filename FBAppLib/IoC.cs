using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FBAppLib
{
    internal static class IoC
    {
        static readonly IDictionary<Type, Type> sr_Types = new Dictionary<Type, Type>();

        public static void Register<TContract, TImplementation>()
        {
            sr_Types[typeof(TContract)] = typeof(TImplementation);
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static object Resolve(Type i_Contract)
        {
            Type implementation = sr_Types[i_Contract];
            ConstructorInfo constructor = implementation.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            List<object> parameters = new List<object>(constructorParameters.Length);
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(Resolve(parameterInfo.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }
    }
}
