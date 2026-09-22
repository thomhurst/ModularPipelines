using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ModularPipelines.Serialization;

internal static class StableTypeName
{
    public static string Get(Type type) =>
        $"{GetTypeSpecification(type)}, {type.Assembly.GetName().Name}";

    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Runtime module result value types are explicitly unsupported in trimmed applications.")]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Runtime module result value types are explicitly unsupported in trimmed applications.")]
    public static Type? Resolve(string typeName) =>
        Type.GetType(
            typeName,
            ResolveAssembly,
            static (assembly, name, ignoreCase) => assembly?.GetType(name, throwOnError: false, ignoreCase),
            throwOnError: false);

    private static string GetTypeSpecification(Type type)
    {
        if (type.HasElementType)
        {
            var suffix = type switch
            {
                { IsSZArray: true } => "[]",
                { IsArray: true } when type.GetArrayRank() == 1 => "[*]",
                { IsArray: true } => $"[{new string(',', type.GetArrayRank() - 1)}]",
                { IsByRef: true } => "&",
                _ => "*",
            };
            return GetTypeSpecification(type.GetElementType()!) + suffix;
        }

        if (!type.IsGenericType)
        {
            return type.FullName ?? type.Name;
        }

        var definition = type.GetGenericTypeDefinition();
        var arguments = string.Join(
            ",",
            type.GetGenericArguments().Select(static argument => $"[{Get(argument)}]"));
        return $"{definition.FullName}[{arguments}]";
    }

    private static Assembly? ResolveAssembly(AssemblyName requestedAssembly)
    {
        if (requestedAssembly.Name is not { } simpleName)
        {
            return null;
        }

        return AppDomain.CurrentDomain.GetAssemblies()
                   .FirstOrDefault(assembly => string.Equals(
                       assembly.GetName().Name,
                       simpleName,
                       StringComparison.Ordinal))
               ?? TryLoad(simpleName);
    }

    private static Assembly? TryLoad(string simpleName)
    {
        try
        {
            return Assembly.Load(new AssemblyName(simpleName));
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }
}
