using System.Collections.Concurrent;

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
    public static void Register(
        Type declaringType,
        IReadOnlyList<SecretPropertyAccessor> accessors,
        bool isComplete = true)
    {
        Accessors[declaringType] = new SecretMetadata(accessors, isComplete);
    }

    internal static bool TryGetAccessors(Type type, out IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        var generatedAccessors = new List<SecretPropertyAccessor>();
        var found = false;

        for (var current = type; current is not null; current = current.BaseType)
        {
            if (Accessors.TryGetValue(current, out var metadata))
            {
                if (!metadata.IsComplete)
                {
                    accessors = Array.Empty<SecretPropertyAccessor>();
                    return false;
                }

                found = true;
                generatedAccessors.AddRange(metadata.Accessors);
            }
        }

        accessors = generatedAccessors;
        return found;
    }

    private sealed record SecretMetadata(IReadOnlyList<SecretPropertyAccessor> Accessors, bool IsComplete);
}

/// <summary>
/// Provides direct access to a property marked with SecretValueAttribute.
/// </summary>
public sealed record SecretPropertyAccessor(string PropertyName, Func<object, object?> Getter);
