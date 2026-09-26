using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Serialization;

internal static class StableTypeName
{
    private static readonly ConditionalWeakTable<Type, string> BuildFingerprints = [];
    private static readonly ConditionalWeakTable<Type, string> InterfaceBuildFingerprints = [];
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

    // Interface signatures affect module behavior even when none of their values are
    // serialized. Inspect CLR types without constructing attributed JSON converters.
    public static string GetInterfaceBuildFingerprint(Type type) =>
        InterfaceBuildFingerprints.GetValue(type, static value =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(GetInterfaceBuildIdentity(value)))));

    private static string GetInterfaceBuildIdentity(Type type)
    {
        var identity = GetBuildIdentity(type, [], [], expandMembers: false);
        foreach (var memberType in GetInterfaceMemberTypes(type).Distinct().OrderBy(Get, StringComparer.Ordinal))
        {
            identity += $"\0InterfaceMember={GetBuildIdentity(memberType, [], [], expandMembers: false)}";
        }

        return identity;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Module interface build fingerprints require runtime type metadata.")]
    private static IEnumerable<Type> GetInterfaceMemberTypes(Type type)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        // Accessors include property/indexer/event signatures as well as ordinary methods.
        foreach (var method in type.GetMethods(flags))
        {
            yield return method.ReturnType;
            foreach (var parameter in method.GetParameters())
            {
                yield return parameter.ParameterType;
            }

            foreach (var constraint in method.GetGenericArguments().SelectMany(argument => argument.GetGenericParameterConstraints()))
            {
                yield return constraint;
            }
        }

        foreach (var field in type.GetFields(flags))
        {
            yield return field.FieldType;
        }
    }

    private static string GetBuildIdentity(Type type, HashSet<Type> visitedTypes, HashSet<Type> expandedDefinitions, bool expandMembers = true)
    {
        var frameworkType = IsFrameworkAssembly(type.Assembly);
        var identity = GetTypeBuildIdentity(type);
        // Types can recur through base classes, generic arguments, and serialized members.
        if (!visitedTypes.Add(type))
        {
            return identity;
        }

        identity += GetConstructionBuildIdentity(type, visitedTypes, expandedDefinitions, expandMembers);

        // Framework servicing builds do not define the application's wire contract.
        // Generic arguments still need validation, e.g. List<ApplicationResult>.
        if (frameworkType)
        {
            return identity;
        }

        // Converters replace reflected members, but can execute the declared type's code.
        // Keep its binary, base, and generic argument builds without expanding members.
        if (!expandMembers)
        {
            return identity + GetBaseBuildIdentity(type, visitedTypes, expandedDefinitions, expandMembers: false);
        }

        // Node<T>.Next can be Node<Node<T>>. Expand each definition once, while still
        // recording every encountered construction and its argument builds above.
        if (!expandedDefinitions.Add(type.IsGenericType ? type.GetGenericTypeDefinition() : type))
        {
            return identity;
        }

        identity += GetBaseBuildIdentity(type, visitedTypes, expandedDefinitions);

        foreach (var contract in GetSerializationContractTypes(type).Distinct()
                     .OrderBy(contract => Get(contract.Type), StringComparer.Ordinal).ThenBy(contract => contract.ExpandMembers))
        {
            // A restricted visit must not prevent a later full visit through a visible member.
            var memberIdentity = contract.ExpandMembers
                ? GetBuildIdentity(contract.Type, visitedTypes, expandedDefinitions)
                : GetBuildIdentity(contract.Type, [], [], expandMembers: false);
            identity += $"\0Member={memberIdentity}";
        }

        return identity;
    }

    private static string GetTypeBuildIdentity(Type type) =>
        IsFrameworkAssembly(type.Assembly) ? Get(type) : $"{Get(type)}\0{type.Module.ModuleVersionId}";

    private static string GetBaseBuildIdentity(Type type, HashSet<Type> visitedTypes, HashSet<Type> expandedDefinitions, bool expandMembers = true)
    {
        var identity = string.Empty;
        for (var current = type.BaseType; current is not null; current = current.BaseType)
        {
            // Preserve base binary/argument checks without treating its hidden declarations
            // as a second serialization contract. Members are selected from the derived type.
            identity += $"\0Base={GetTypeBuildIdentity(current)}{GetConstructionBuildIdentity(current, visitedTypes, expandedDefinitions, expandMembers)}";
            if (IsFrameworkAssembly(current.Assembly))
            {
                break;
            }
        }

        return identity;
    }

    private static IEnumerable<Type> GetApplicationTypeHierarchy(Type type)
    {
        for (var current = type; current is not null && !IsFrameworkAssembly(current.Assembly); current = current.BaseType)
        {
            yield return current;
        }
    }

    private static string GetConstructionBuildIdentity(Type type, HashSet<Type> visitedTypes, HashSet<Type> expandedDefinitions, bool expandMembers = true)
    {
        if (type.HasElementType)
        {
            return $"\0Element={GetBuildIdentity(type.GetElementType()!, visitedTypes, expandedDefinitions, expandMembers)}";
        }

        if (type.IsGenericType)
        {
            var arguments = string.Join("\u001F", type.GetGenericArguments()
                .Select(argument => GetBuildIdentity(argument, visitedTypes, expandedDefinitions, expandMembers)));
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
    private static IEnumerable<(Type Type, bool ExpandMembers)> GetSerializationContractTypes(Type type)
    {
        // A type-level converter replaces the reflected object/collection contract.
        if (type.IsDefined(typeof(JsonConverterAttribute), inherit: false))
        {
            foreach (var converterType in GetConverterTypes(type, type))
            {
                yield return (converterType, true);
            }

            yield break;
        }

        foreach (var current in GetApplicationTypeHierarchy(type))
        {
            foreach (var derived in current.GetCustomAttributes<JsonDerivedTypeAttribute>(inherit: false))
            {
                yield return (derived.DerivedType, true);
            }

            foreach (var converterType in GetConverterTypes(current, current))
            {
                yield return (converterType, true);
            }
        }

        foreach (var (member, memberType) in GetSerializedMembers(type))
        {
            // Converters own the member contract, but not the declared type's build identity.
            yield return (memberType, !member.IsDefined(typeof(JsonConverterAttribute), inherit: false));

            foreach (var converterType in GetConverterTypes(member, memberType))
            {
                yield return (converterType, true);
            }
        }

        foreach (var contract in type.GetInterfaces())
        {
            // Concrete collections can expose their serialized element type only through
            // IEnumerable<T>. Dictionary enumeration also carries both key and value types.
            if (type.IsInterface || (contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                yield return (contract, true);
            }
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Runtime result serialization and its build fingerprints are explicitly unsupported in trimmed applications.")]
    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Runtime result serialization and its inherited build fingerprints are explicitly unsupported in trimmed applications.")]
    private static IEnumerable<(MemberInfo Member, Type Type)> GetSerializedMembers(Type type)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        var getters = new HashSet<MethodInfo>();
        var membersByJsonName = new Dictionary<string, MemberInfo>(StringComparer.Ordinal);
        foreach (var current in GetApplicationTypeHierarchy(type))
        {
            foreach (var property in current.GetProperties(flags))
            {
                if (!IsJsonProperty(property))
                {
                    continue;
                }

                // Record overrides and JSON name collisions before checking readability:
                // setter-only and WhenWriting members still shadow base declarations.
                if (property.GetMethod is { } getter && !getters.Add(getter.GetBaseDefinition()))
                {
                    continue;
                }

                if (IsEffectiveJsonMember(property, membersByJsonName)
                    && property.GetMethod is { } readableGetter
                    && (readableGetter.IsPublic || property.IsDefined(typeof(JsonIncludeAttribute)))
                    && property.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition != JsonIgnoreCondition.WhenWriting)
                {
                    yield return (property, property.PropertyType);
                }
            }

            // Serializer options leave IncludeFields disabled; fields require an explicit JsonInclude.
            foreach (var field in current.GetFields(flags))
            {
                if (field.IsDefined(typeof(JsonIncludeAttribute))
                    && IsEffectiveJsonMember(field, membersByJsonName)
                    && field.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition != JsonIgnoreCondition.WhenWriting)
                {
                    yield return (field, field.FieldType);
                }
            }
        }
    }

    private static bool IsJsonProperty(PropertyInfo property) =>
        property.GetIndexParameters().Length == 0
        && (property.GetMethod?.IsPublic == true || property.SetMethod?.IsPublic == true
            || property.IsDefined(typeof(JsonIncludeAttribute)));

    private static bool IsEffectiveJsonMember(MemberInfo member, Dictionary<string, MemberInfo> membersByJsonName)
    {
        // An always-ignored new member allows the base declaration to supply its JSON name.
        // Ignored virtual overrides have already suppressed their base getter above.
        if (member.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition == JsonIgnoreCondition.Always)
        {
            return false;
        }

        var jsonName = member.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? member.Name;
        if (membersByJsonName.TryGetValue(jsonName, out var derivedMember)
            && member.Name == derivedMember.Name
            && member.DeclaringType!.IsAssignableFrom(derivedMember.DeclaringType))
        {
            return false;
        }

        membersByJsonName.TryAdd(jsonName, member);
        return true;
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

    public static Type? Resolve(string typeName, AssemblyLoadContext? loadContext = null, string? buildFingerprint = null)
    {
        var localType = ResolveInContext(typeName, loadContext);
        if (localType is not null || loadContext is null || buildFingerprint is null)
        {
            return localType;
        }

        // A default-context module can return a plugin value. Only fall back when its
        // own context cannot resolve the type; never replace an incompatible local build.
        var candidates = AssemblyLoadContext.All
            .Where(context => context != loadContext)
            .Select(context => ResolveInContext(typeName, context))
            .OfType<Type>()
            .Distinct()
            .Where(type => string.Equals(GetBuildFingerprint(type), buildFingerprint, StringComparison.Ordinal))
            .Take(2)
            .ToArray();
        return candidates.Length switch
        {
            0 => null,
            1 => candidates[0],
            _ => throw new JsonException($"Module result value type '{typeName}' is ambiguous across loaded contexts."),
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Runtime module result value types are explicitly unsupported in trimmed applications.")]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Runtime module result value types are explicitly unsupported in trimmed applications.")]
    private static Type? ResolveInContext(string typeName, AssemblyLoadContext? loadContext) =>
        Type.GetType(
            typeName,
            assemblyName => ResolveAssembly(assemblyName, loadContext),
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

    private static Assembly? ResolveAssembly(AssemblyName requestedAssembly, AssemblyLoadContext? loadContext)
    {
        if (requestedAssembly.Name is not { } simpleName)
        {
            return null;
        }

        if (loadContext is not null)
        {
            try
            {
                return loadContext.Assemblies.FirstOrDefault(assembly => string.Equals(
                           assembly.GetName().Name, simpleName, StringComparison.Ordinal))
                       ?? loadContext.LoadFromAssemblyName(requestedAssembly);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
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
