using System.Collections.Concurrent;
using System.ComponentModel;

namespace ModularPipelines.Generated;

/// <summary>
/// Stores module attribute factories emitted by ModularPipelines.SourceGenerator.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class GeneratedModuleEventMetadata
{
    private static readonly ConcurrentDictionary<Type, AttributeMetadata> Factories = new();

    /// <summary>
    /// Registers a generated attribute factory for a module type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type moduleType,
        Func<IReadOnlyList<Attribute>> attributeFactory,
        bool isComplete = true)
    {
        Factories.TryAdd(moduleType, new AttributeMetadata(attributeFactory, isComplete));
    }

    internal static bool TryCreateAttributes(
        Type moduleType,
        out IReadOnlyList<Attribute> attributes)
    {
        if (Factories.TryGetValue(moduleType, out var metadata) && metadata.IsComplete)
        {
            attributes = metadata.AttributeFactory();
            return true;
        }

        attributes = [];
        return false;
    }

    private sealed record AttributeMetadata(
        Func<IReadOnlyList<Attribute>> AttributeFactory,
        bool IsComplete);
}
