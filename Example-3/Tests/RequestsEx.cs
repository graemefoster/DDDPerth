using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pipeline.Testing;

public static class RequestsEx
{
    public static IEnumerable<Type> FindRequestTypes(this Assembly assembly)
    {
        foreach(var type in assembly.GetTypes())
        {
            if (type.GetInterfaces().Any(i => 
                    i.GetTypeInfo().IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IRequest<>)))
            {
                Console.WriteLine("Found request:" + type.Name);
                yield return type;
            }
        }
    }
}