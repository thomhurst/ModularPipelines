using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Serialization;

internal static class StableTypeName
{
    private static readonly ConditionalWeakTable<Type, string> BuildFingerprints = [];
    // The trusted-platform list includes application assemblies. Only shared-framework
    // dependency manifests identify directories whose implementation builds may vary.
    // The host separates these manifests with semicolons on every platform.
    private static readonly HashSet<string> SharedFrameworkDirectories =
        ((string?) AppContext.GetData("APP_CONTEXT_DEPS_FILES") ?? string.Empty)
        .Split(';', StringSplitOptions.RemoveEmptyEntries)
        .Where(path => Path.GetFileName(path) is "Microsoft.NETCore.App.deps.json"
            or "Microsoft.AspNetCore.App.deps.json" or "Microsoft.WindowsDesktop.App.deps.json")
        .Select(path => Path.GetDirectoryName(path)!)
        .ToHashSet(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

    public static string Get(Type type) =>
        $"{GetTypeSpecification(type)}, {type.Assembly.GetName().Name}";

    public static string GetBuildFingerprint(Type type) =>
        BuildFingerprints.GetValue(type, static value =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(GetBuildIdentity(value, [], [])))));

    private static string GetBuildIdentity(Type type, HashSet<Type> visitedTypes, HashSet<Type> expandedDefinitions)
    {
        var frameworkType = IsFrameworkAssembly(type.Assembly);
        var identity = frameworkType ? Get(type) : $"{Get(type)}\0{type.Module.ModuleVersionId}";
        // Types can recur through base classes, generic arguments, and serialized members.
        if (!visitedTypes.Add(type))
        {
            return identity;
        }

        identity += GetConstructionBuildIdentity(type, visitedTypes, expandedDefinitions);

        // Framework servicing builds do not define the application's wire contract.
        // Generic arguments still need validation, e.g. List<ApplicationResult>.
        if (frameworkType)
        {
            return identity;
        }

        // Node<T>.Next can be Node<Node<T>>. Expand each definition once, while still
        // recording every encountered construction and its argument builds above.
        if (!expandedDefinitions.Add(type.IsGenericType ? type.GetGenericTypeDefinition() : type))
        {
            return identity;
        }

        if (type.BaseType is { } baseType)
        {
            identity += $"\0Base={GetBuildIdentity(baseType, visitedTypes, expandedDefinitions)}";
        }

        foreach (var memberType in GetSerializationContractTypes(type).Distinct().OrderBy(Get, StringComparer.Ordinal))
        {
            identity += $"\0Member={GetBuildIdentity(memberType, visitedTypes, expandedDefinitions)}";
        }

        return identity;
    }

    private static string GetConstructionBuildIdentity(Type type, HashSet<Type> visitedTypes, HashSet<Type> expandedDefinitions)
    {
        if (type.HasElementType)
        {
            return $"\0Element={GetBuildIdentity(type.GetElementType()!, visitedTypes, expandedDefinitions)}";
        }

        if (type.IsGenericType)
        {
            var arguments = string.Join("\u001F", type.GetGenericArguments()
                .Select(argument => GetBuildIdentity(argument, visitedTypes, expandedDefinitions)));
            return $"\0Arguments={arguments}";
        }

        return string.Empty;
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000", Justification = "Locationless assemblies retain build validation; CoreLib is recognized by assembly identity.")]
    public static bool IsFrameworkAssembly(Assembly assembly) =>
        assembly == typeof(object).Assembly
        || (!assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location)
            && SharedFrameworkDirectories.Contains(Path.GetDirectoryName(assembly.Location)!));

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Runtime result serialization and its build fingerprints are explicitly unsupported in trimmed applications.")]
    private static IEnumerable<Type> GetSerializationContractTypes(Type type)
    {
        foreach (var derived in type.GetCustomAttributes<JsonDerivedTypeAttribute>(inherit: false))
        {
            yield return derived.DerivedType;
        }

        foreach (var converterType in GetConverterTypes(type, type))
        {
            yield return converterType;
        }

        foreach (var (member, memberType) in GetSerializedMembers(type))
        {
            yield return memberType;
            foreach (var converterType in GetConverterTypes(member, memberType))
            {
                yield return converterType;
            }
        }

        foreach (var contract in type.GetInterfaces())
        {
            // Concrete collections can expose their serialized element type only through
            // IEnumerable<T>. Dictionary enumeration also carries both key and value types.
            if (type.IsInterface || (contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                yield return contract;
            }
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Runtime result serialization and its build fingerprints are explicitly unsupported in trimmed applications.")]
    private static IEnumerable<(MemberInfo Member, Type Type)> GetSerializedMembers(Type type)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        foreach (var property in type.GetProperties(flags))
        {
            if (property.GetIndexParameters().Length == 0
                && (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true
                    || property.IsDefined(typeof(JsonIncludeAttribute)))
                && property.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition != JsonIgnoreCondition.Always)
            {
                yield return (property, property.PropertyType);
            }
        }

        // Serializer options leave IncludeFields disabled; fields require an explicit JsonInclude.
        foreach (var field in type.GetFields(flags))
        {
            if (field.IsDefined(typeof(JsonIncludeAttribute))
                && field.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition != JsonIgnoreCondition.Always)
            {
                yield return (field, field.FieldType);
            }
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "Runtime converter construction and its build fingerprints are explicitly unsupported in trimmed applications.")]
    private static IEnumerable<Type> GetConverterTypes(MemberInfo member, Type typeToConvert)
    {
        if (member.GetCustomAttribute<JsonConverterAttribute>(inherit: false) is not { } attribute)
        {
            yield break;
        }

        yield return attribute.GetType();
        JsonConverter? converter;
        if (attribute.ConverterType is { } converterType)
        {
            yield return converterType;
            if (!typeof(JsonConverterFactory).IsAssignableFrom(converterType))
            {
                yield break;
            }

            converter = (JsonConverter?) Activator.CreateInstance(converterType);
        }
        else
        {
            converter = attribute.CreateConverter(typeToConvert);
        }

        if (converter is null)
        {
            throw new InvalidOperationException($"The JSON converter for '{member.Name}' returned no converter.");
        }

        yield return converter.GetType();
        if (converter is JsonConverterFactory factory)
        {
            yield return GetProducedConverterType(factory, typeToConvert, member.Name);
        }
    }

    private static Type GetProducedConverterType(JsonConverterFactory factory, Type typeToConvert, string memberName)
    {
        // System.Text.Json forwards nullable values to an attribute's underlying-value converter.
        var targetType = !factory.CanConvert(typeToConvert)
            && Nullable.GetUnderlyingType(typeToConvert) is { } underlyingType
            && factory.CanConvert(underlyingType)
                ? underlyingType
                : typeToConvert;
        var produced = factory.CreateConverter(targetType, ModuleResultSerializer.CreateOptions());
        if (produced is null or JsonConverterFactory)
        {
            throw new InvalidOperationException($"The JSON converter factory for '{memberName}' returned no concrete converter.");
        }

        return produced.GetType();
    }

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
