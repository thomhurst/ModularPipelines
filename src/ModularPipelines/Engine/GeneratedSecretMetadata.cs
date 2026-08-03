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
    private static readonly ConcurrentDictionary<Type, SecretMetadata> Accessors = new();
    private static readonly ConcurrentDictionary<(Assembly Assembly, string MetadataName), byte> CoveredTypeNames = new();
    private static readonly ConcurrentDictionary<Assembly, byte> ProcessedAssemblies = new();

    /// <summary>
    /// Registers that an assembly ran the C# metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        ProcessedAssemblies.TryAdd(assembly, 0);
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
        if (!Accessors.TryAdd(declaringType, new SecretMetadata(accessors, isComplete)))
        {
            throw new InvalidOperationException($"Secret metadata is already registered for {declaringType}.");
        }
    }

    /// <summary>
    /// Registers source coverage for a type that generated code cannot reference directly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredTypeName(Assembly assembly, string metadataName)
    {
        CoveredTypeNames.TryAdd((assembly, metadataName), 0);
    }

    internal static bool TryGetAccessors(Type type, out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        if (type.IsArray || IsAnonymousType(type))
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
            && CoveredTypeNames.ContainsKey((type.Assembly, metadataName)))
        {
            accessors = Array.Empty<SecretPropertyAccessor>();
            return true;
        }

        accessors = Array.Empty<SecretPropertyAccessor>();
        return false;
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) => ProcessedAssemblies.ContainsKey(assembly);

    private static bool IsAnonymousType(Type type) =>
        type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
        && (type.Name.StartsWith("<>f__AnonymousType", StringComparison.Ordinal)
            || type.Name.StartsWith("VB$AnonymousType_", StringComparison.Ordinal));

    private sealed record SecretMetadata(IReadOnlyList<SecretPropertyAccessor> Accessors, bool IsComplete);
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
