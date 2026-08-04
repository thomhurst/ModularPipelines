using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores secret-property accessors emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedSecretMetadata
{
    private static readonly ConditionalWeakTable<Type, SecretMetadata> Accessors = [];
    private static readonly ConditionalWeakTable<Assembly, AssemblyCoverage> AssemblyCoverageByAssembly = [];
    private static readonly ConditionalWeakTable<Assembly, ExternalSecretMetadata> ExternalAccessors = [];

    /// <summary>
    /// Registers that an assembly ran the C# metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        RegisterAssembly(assembly, requiresGeneratedMetadata: false);
    }

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
    /// Registers that a declaring type has no secret properties.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(Type declaringType)
    {
        Register(declaringType, Array.Empty<SecretPropertyAccessor>());
    }

    /// <summary>
    /// Registers generated secret accessors for a declaring type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        RegisterCore(declaringType, accessors, isComplete: true, isLegacy: false);
    }

    /// <summary>
    /// Preserves the registration signature emitted by earlier source-generator versions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors,
        bool isComplete = true)
    {
        RegisterCore(declaringType, accessors, isComplete, isLegacy: true);
    }

    private static void RegisterCore(
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors,
        bool isComplete,
        bool isLegacy)
    {
        try
        {
            Accessors.Add(declaringType, new SecretMetadata(accessors, isComplete, isLegacy));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException(
                $"Secret metadata is already registered for {declaringType}.",
                exception);
        }
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
    /// Registers empty secret metadata emitted by a consuming assembly for an external type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(Assembly consumerAssembly, Type declaringType)
    {
        RegisterExternal(consumerAssembly, declaringType, Array.Empty<SecretPropertyAccessor>());
    }

    /// <summary>
    /// Registers secret metadata emitted by a consuming assembly for an external type.
    /// Registrations are scoped weakly to the consumer so collectible assemblies are not retained.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(
        Assembly consumerAssembly,
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        var registrations = ExternalAccessors.GetValue(
            consumerAssembly,
            static _ => new ExternalSecretMetadata());
        registrations.Accessors.TryAdd(
            declaringType,
            new SecretMetadata(accessors, IsComplete: true, IsLegacy: false));
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
            accessors = Array.Empty<SecretPropertyAccessor>();
            return true;
        }

        Accessors.TryGetValue(type, out var directMetadata);
        if (directMetadata is { IsComplete: true, IsLegacy: false })
        {
            accessors = directMetadata.Accessors;
            return true;
        }

        foreach (var registrations in ExternalAccessors)
        {
            if (registrations.Value.Accessors.TryGetValue(type, out var metadata)
                && metadata.IsComplete)
            {
                accessors = metadata.Accessors;
                return true;
            }
        }

        if (IsCoveredExternalAssembly(type))
        {
            accessors = Array.Empty<SecretPropertyAccessor>();
            return true;
        }

        if (directMetadata is { IsComplete: true })
        {
            accessors = directMetadata.Accessors;
            return true;
        }

        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        if (metadataType.FullName is { } metadataName
            && AssemblyCoverageByAssembly.TryGetValue(type.Assembly, out var coverage)
            && coverage.CoveredTypeNames.ContainsKey(metadataName))
        {
            accessors = Array.Empty<SecretPropertyAccessor>();
            return true;
        }

        accessors = Array.Empty<SecretPropertyAccessor>();
        return false;
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

    internal static bool IsIncomplete(Type type)
    {
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return metadataType.FullName is { } metadataName
               && AssemblyCoverageByAssembly.TryGetValue(type.Assembly, out var coverage)
               && coverage.IncompleteTypeNames.ContainsKey(metadataName);
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) =>
        AssemblyCoverageByAssembly.TryGetValue(assembly, out _);

    internal static bool IsGeneratedMetadataRequired(Assembly assembly)
    {
        var loadContext = AssemblyLoadContext.GetLoadContext(assembly);
        return AssemblyCoverageByAssembly.Any(registration =>
            registration.Value.RequiresGeneratedMetadata
            && ReferenceEquals(AssemblyLoadContext.GetLoadContext(registration.Key), loadContext));
    }

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

    private sealed record SecretMetadata(
        IReadOnlyList<SecretPropertyAccessor> Accessors,
        bool IsComplete,
        bool IsLegacy);

    private sealed class ExternalSecretMetadata
    {
        public ConcurrentDictionary<Type, SecretMetadata> Accessors { get; } = [];

        public ConcurrentDictionary<string, byte> CoveredAssemblyIdentities { get; } = new(StringComparer.Ordinal);
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
