using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace ModularPipelines.Generated;

/// <summary>
/// Stores secret-property accessors emitted by ModularPipelines.SourceGenerator.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class GeneratedSecretMetadata
{
    internal const int CurrentSchemaVersion = 2;

    private static readonly ConditionalWeakTable<Type, SecretMetadata> Accessors = [];
    private static readonly ConditionalWeakTable<Assembly, AssemblyCoverage> AssemblyCoverageByAssembly = [];
    private static readonly ConditionalWeakTable<Assembly, ExternalSecretMetadata> ExternalAccessors = [];

    /// <summary>
    /// Registers that an assembly ran the C# metadata generator and whether reflection fallback is unsafe.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly, bool requiresGeneratedMetadata)
    {
        var coverage = AssemblyCoverageByAssembly.GetValue(
            assembly,
            static _ => new AssemblyCoverage());
        if (requiresGeneratedMetadata)
        {
            coverage.RequiresGeneratedMetadata = true;
        }
    }

    /// <summary>
    /// Registers generated secret accessors for a declaring type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors,
        int schemaVersion)
    {
        ValidateSchemaVersion(declaringType.Assembly, schemaVersion);

        try
        {
            Accessors.Add(declaringType, new SecretMetadata(accessors));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException(
                $"Secret metadata is already registered for {declaringType}.",
                exception);
        }
    }

    private static void ValidateSchemaVersion(Assembly assembly, int schemaVersion)
    {
        if (schemaVersion == CurrentSchemaVersion)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Assembly '{assembly.GetName().Name}' registered secret metadata schema "
            + $"{schemaVersion}, but this ModularPipelines runtime requires schema "
            + $"{CurrentSchemaVersion}. Rebuild the assembly against ModularPipelines v4.");
    }

    /// <summary>
    /// Registers source coverage for a type that generated code cannot reference directly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredTypeName(Assembly assembly, string metadataName)
    {
        RegisterCoveredTypeNames(assembly, [metadataName]);
    }

    /// <summary>
    /// Registers source coverage for types that generated code does not reference directly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredTypeNames(
        Assembly assembly,
        IReadOnlyList<string> metadataNames)
    {
        var coverage = AssemblyCoverageByAssembly.GetValue(
            assembly,
            static _ => new AssemblyCoverage());
        foreach (var metadataName in metadataNames)
        {
            coverage.CoveredTypeNames.TryAdd(metadataName, 0);
        }
    }

    /// <summary>
    /// Registers secret metadata emitted by a consuming assembly for an external type.
    /// Registrations are scoped weakly to the consumer so collectible assemblies are not retained.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(
        Assembly consumerAssembly,
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors,
        int schemaVersion)
    {
        ValidateSchemaVersion(consumerAssembly, schemaVersion);
        var registrations = ExternalAccessors.GetValue(
            consumerAssembly,
            static _ => new ExternalSecretMetadata());
        registrations.Accessors.TryAdd(
            declaringType,
            new SecretMetadata(accessors));
    }

    /// <summary>
    /// Registers referenced assemblies that cannot declare or inherit secret properties because
    /// they do not reference the ModularPipelines runtime assembly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredExternalAssemblyIdentities(
        Assembly consumerAssembly,
        IReadOnlyList<string> assemblyIdentities)
    {
        var registrations = ExternalAccessors.GetValue(
            consumerAssembly,
            static _ => new ExternalSecretMetadata());
        foreach (var assemblyIdentity in assemblyIdentities)
        {
            registrations.CoveredAssemblyIdentities.TryAdd(assemblyIdentity, 0);
        }
    }

    /// <summary>
    /// Registers empty secret metadata for external types that generated code cannot reference directly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredExternalTypeNames(
        Assembly consumerAssembly,
        string assemblyIdentity,
        IReadOnlyList<string> metadataNames) =>
        RegisterExternalTypeNames(
            consumerAssembly,
            assemblyIdentity,
            metadataNames,
            static registrations => registrations.CoveredTypeNamesByAssemblyIdentity);

    private static void RegisterExternalTypeNames(
        Assembly consumerAssembly,
        string assemblyIdentity,
        IReadOnlyList<string> metadataNames,
        Func<ExternalSecretMetadata,
            ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> getRegistrations)
    {
        var registrations = ExternalAccessors.GetValue(
            consumerAssembly,
            static _ => new ExternalSecretMetadata());
        var registeredTypeNames = getRegistrations(registrations).GetOrAdd(
            assemblyIdentity,
            static _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        foreach (var metadataName in metadataNames)
        {
            registeredTypeNames.TryAdd(metadataName, 0);
        }
    }

    /// <summary>
    /// Registers partial source types whose final runtime shape requires reflection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterIncompleteTypeNames(
        Assembly assembly,
        IReadOnlyList<string> metadataNames)
    {
        var coverage = AssemblyCoverageByAssembly.GetValue(
            assembly,
            static _ => new AssemblyCoverage());
        foreach (var metadataName in metadataNames)
        {
            coverage.IncompleteTypeNames.TryAdd(metadataName, 0);
        }
    }

    internal static bool TryGetAccessors(Type type, out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        if (type.IsArray || type.IsEnum || typeof(Delegate).IsAssignableFrom(type)
            || IsKnownCompilerGeneratedInfrastructure(type))
        {
            return ReturnEmptyAccessors(out accessors, result: true);
        }

        if (Accessors.TryGetValue(type, out var directMetadata))
        {
            accessors = directMetadata.Accessors;
            return true;
        }

        if (TryGetExternalAccessors(type, out accessors))
        {
            return true;
        }

        if (IsCoveredExternalType(type) || IsCoveredExternalAssembly(type))
        {
            return ReturnEmptyAccessors(out accessors, result: true);
        }

        return ReturnEmptyAccessors(out accessors, IsCoveredGeneratedType(type));
    }

    private static bool TryGetExternalAccessors(
        Type type,
        out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        foreach (var registrations in ExternalAccessors)
        {
            if (registrations.Value.Accessors.TryGetValue(type, out var metadata))
            {
                accessors = metadata.Accessors;
                return true;
            }
        }

        accessors = Array.Empty<SecretPropertyAccessor>();
        return false;
    }

    private static bool IsCoveredGeneratedType(Type type)
    {
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return metadataType.FullName is { } metadataName
               && AssemblyCoverageByAssembly.TryGetValue(type.Assembly, out var coverage)
               && coverage.CoveredTypeNames.ContainsKey(metadataName);
    }

    private static bool ReturnEmptyAccessors(
        out IReadOnlyList<SecretPropertyAccessor> accessors,
        bool result)
    {
        accessors = Array.Empty<SecretPropertyAccessor>();
        return result;
    }

    private static bool IsCoveredExternalAssembly(Type type)
    {
        if (type.Assembly.FullName is not { } assemblyIdentity)
        {
            return false;
        }

        var loadContext = AssemblyLoadContext.GetLoadContext(type.Assembly);
        return ExternalAccessors.Any(registrations =>
            ReferenceEquals(AssemblyLoadContext.GetLoadContext(registrations.Key), loadContext)
            && registrations.Value.CoveredAssemblyIdentities.ContainsKey(assemblyIdentity));
    }

    private static bool IsCoveredExternalType(Type type) =>
        IsExternalTypeNameRegistered(
            type,
            static registrations => registrations.CoveredTypeNamesByAssemblyIdentity);

    private static bool IsExternalTypeNameRegistered(
        Type type,
        Func<ExternalSecretMetadata,
            ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> getRegistrations)
    {
        if (type.Assembly.FullName is not { } assemblyIdentity)
        {
            return false;
        }

        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        if (metadataType.FullName is not { } metadataName)
        {
            return false;
        }

        var loadContext = AssemblyLoadContext.GetLoadContext(type.Assembly);
        return ExternalAccessors.Any(registrations =>
            ReferenceEquals(AssemblyLoadContext.GetLoadContext(registrations.Key), loadContext)
            && getRegistrations(registrations.Value).TryGetValue(
                assemblyIdentity,
                out var coveredTypeNames)
            && coveredTypeNames.ContainsKey(metadataName));
    }

    internal static bool IsIncomplete(Type type)
    {
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return metadataType.FullName is { } metadataName
               && AssemblyCoverageByAssembly.TryGetValue(type.Assembly, out var coverage)
               && coverage.IncompleteTypeNames.ContainsKey(metadataName);
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) =>
        AssemblyCoverageByAssembly.TryGetValue(assembly, out _);

    internal static bool IsGeneratedMetadataRequired(Assembly assembly) =>
        AssemblyCoverageByAssembly.TryGetValue(assembly, out var coverage)
        && coverage.RequiresGeneratedMetadata;

    private static bool IsKnownCompilerGeneratedInfrastructure(Type type)
    {
        if (!type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
        {
            return false;
        }

        return type.Name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal)
               || type.Name.StartsWith("VB$AnonymousType_", StringComparison.Ordinal)
               || type.Name == "<>c"
               || type.Name.StartsWith("<>c__DisplayClass", StringComparison.Ordinal)
               || (type.Name.Contains(">d__", StringComparison.Ordinal)
                   && IsIteratorStateMachine(type));
    }

    private static bool IsIteratorStateMachine(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type)
               || typeof(IAsyncStateMachine).IsAssignableFrom(type);
    }

    private sealed record SecretMetadata(IReadOnlyList<SecretPropertyAccessor> Accessors);

    private sealed class ExternalSecretMetadata
    {
        public ConcurrentDictionary<Type, SecretMetadata> Accessors { get; } = [];

        public ConcurrentDictionary<string, byte> CoveredAssemblyIdentities { get; } = new(StringComparer.Ordinal);

        public ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>
            CoveredTypeNamesByAssemblyIdentity
        { get; } = new(StringComparer.Ordinal);
    }

    private sealed class AssemblyCoverage
    {
        public bool RequiresGeneratedMetadata { get; set; }

        public ConcurrentDictionary<string, byte> CoveredTypeNames { get; } = new(StringComparer.Ordinal);

        public ConcurrentDictionary<string, byte> IncompleteTypeNames { get; } = new(StringComparer.Ordinal);
    }
}

/// <summary>
/// Provides direct access to a property marked with SecretValueAttribute.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed record SecretPropertyAccessor(
    string PropertyName,
    Func<object, object?> Getter,
    IReadOnlyList<string>? SecretValueKeys = null)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SecretPropertyAccessor"/> class.
    /// Preserves the constructor emitted by source generators built before key-filtered secrets were supported.
    /// </summary>
    public SecretPropertyAccessor(string propertyName, Func<object, object?> getter)
        : this(propertyName, getter, null)
    {
    }

    /// <summary>
    /// Preserves the two-value deconstructor emitted before key-filtered secrets were supported.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="getter">The property value accessor.</param>
    public void Deconstruct(out string propertyName, out Func<object, object?> getter)
    {
        propertyName = PropertyName;
        getter = Getter;
    }
}
