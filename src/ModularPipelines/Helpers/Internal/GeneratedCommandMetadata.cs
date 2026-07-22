using System.Collections.Concurrent;
using System.ComponentModel;

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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        bool isComplete = true)
    {
        if (!Models.TryAdd(optionsType, new CommandMetadata(model, isComplete)))
        {
            throw new InvalidOperationException($"Command metadata is already registered for {optionsType}.");
        }
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
