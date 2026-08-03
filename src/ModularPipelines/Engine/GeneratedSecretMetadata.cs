using System.Collections.Concurrent;
using System.ComponentModel;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores secret-property accessors emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedSecretMetadata
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<SecretPropertyAccessor>> Accessors = new();

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
        if (!Accessors.TryAdd(declaringType, accessors))
        {
            throw new InvalidOperationException($"Secret metadata is already registered for {declaringType}.");
        }
    }

    internal static bool TryGetAccessors(Type type, out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        if (Accessors.TryGetValue(type, out var metadata))
        {
            accessors = metadata;
            return true;
        }

        accessors = Array.Empty<SecretPropertyAccessor>();
        return false;
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
