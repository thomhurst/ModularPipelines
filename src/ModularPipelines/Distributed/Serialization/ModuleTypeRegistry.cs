using System.Collections.Concurrent;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Serialization;

internal class ModuleTypeRegistry
{
    private readonly ConcurrentDictionary<string, (Type ModuleType, Type ResultType)> _registry = new();

    public void Register(Type moduleType)
    {
        var resultType = GetResultType(moduleType);
        if (resultType is not null)
        {
            _registry[moduleType.FullName!] = (moduleType, resultType);
        }
    }

    public (Type ModuleType, Type ResultType)? Resolve(string moduleTypeName)
    {
        return _registry.TryGetValue(moduleTypeName, out var entry) ? entry : null;
    }

    public static string? GetResultTypeName(Type moduleType)
    {
        var resultType = GetResultType(moduleType);
        return resultType?.FullName;
    }

    private static Type? GetResultType(Type moduleType)
    {
        // Walk the inheritance chain to find Module<T> and extract T
        var current = moduleType;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(Module<>))
            {
                return current.GetGenericArguments()[0];
            }

            current = current.BaseType;
        }

        return null;
    }
}
