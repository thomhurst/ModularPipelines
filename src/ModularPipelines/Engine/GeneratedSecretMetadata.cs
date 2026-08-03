using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores secret-property accessors emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedSecretMetadata
{
    private static readonly ConditionalWeakTable<Type, SecretMetadata> Accessors = new();
    private static readonly ConditionalWeakTable<Assembly, AssemblyCoverage> AssemblyCoverageByAssembly = new();

    /// <summary>
    /// Registers that an assembly ran the C# metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        _ = AssemblyCoverageByAssembly.GetValue(assembly, static _ => new AssemblyCoverage());
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
        Register(declaringType, accessors, isComplete: true);
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
        try
        {
            Accessors.Add(declaringType, new SecretMetadata(accessors, isComplete));
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

        if (Accessors.TryGetValue(type, out var metadata) && metadata.IsComplete)
        {
            accessors = metadata.Accessors;
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

    internal static bool IsIncomplete(Type type)
    {
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return metadataType.FullName is { } metadataName
               && AssemblyCoverageByAssembly.TryGetValue(type.Assembly, out var coverage)
               && coverage.IncompleteTypeNames.ContainsKey(metadataName);
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) =>
        AssemblyCoverageByAssembly.TryGetValue(assembly, out _);

    private static bool IsKnownCompilerGeneratedInfrastructure(Type type)
    {
        if (!type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
        {
            return false;
        }

        return type.Name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal)
               || type.Name.StartsWith("VB$AnonymousType_", StringComparison.Ordinal)
               || type.Name.StartsWith("<>c__DisplayClass", StringComparison.Ordinal)
               || (type.Name.Contains(">d__", StringComparison.Ordinal)
                   && typeof(IEnumerable).IsAssignableFrom(type));
    }

    private sealed record SecretMetadata(IReadOnlyList<SecretPropertyAccessor> Accessors, bool IsComplete);

    private sealed class AssemblyCoverage
    {
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
