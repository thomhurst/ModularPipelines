using System.Collections.Concurrent;
using System.ComponentModel;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores secret-property accessors emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedSecretMetadata
{
    private static readonly ConcurrentDictionary<Type, SecretMetadata> Accessors = new();

    /// <summary>
    /// Registers generated secret accessors for a declaring type.
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

    internal static bool TryGetAccessors(Type type, out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        if (Accessors.TryGetValue(type, out var metadata) && metadata.IsComplete)
        {
            accessors = metadata.Accessors;
            return true;
        }

        accessors = Array.Empty<SecretPropertyAccessor>();
        return false;
    }

    private sealed record SecretMetadata(IReadOnlyList<SecretPropertyAccessor> Accessors, bool IsComplete);
}

/// <summary>
/// Provides direct access to a property marked with SecretValueAttribute.
/// </summary>
public sealed record SecretPropertyAccessor(string PropertyName, Func<object, object?> Getter);
