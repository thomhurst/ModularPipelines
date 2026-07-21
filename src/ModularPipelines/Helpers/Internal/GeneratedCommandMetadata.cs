using System.Collections.Concurrent;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Stores command models emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedCommandMetadata
{
    private static readonly ConcurrentDictionary<Type, CommandMetadata> Models = new();

    /// <summary>
    /// Registers the generated command model for an options type.
    /// </summary>
    public static void Register(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        bool isComplete = true)
    {
        Models[optionsType] = new CommandMetadata(model, isComplete);
    }

    internal static bool TryGet(Type optionsType, out IReadOnlyList<PropertyCommandLinePart> model)
    {
        if (Models.TryGetValue(optionsType, out var metadata) && metadata.IsComplete)
        {
            model = metadata.Model;
            return true;
        }

        model = Array.Empty<PropertyCommandLinePart>();
        return false;
    }

    private sealed record CommandMetadata(IReadOnlyList<PropertyCommandLinePart> Model, bool IsComplete);
}
