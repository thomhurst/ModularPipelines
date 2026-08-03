using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Stores command models emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedCommandMetadata
{
    private static readonly ConcurrentDictionary<Type, CommandMetadata> Models = new();
    private static readonly ConcurrentDictionary<Assembly, byte> ProcessedAssemblies = new();

    /// <summary>
    /// Registers that an assembly ran the C# command metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        ProcessedAssemblies.TryAdd(assembly, 0);
    }

    /// <summary>
    /// Registers the generated command model for an options type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model)
    {
        Register(optionsType, model, isComplete: true);
    }

    /// <summary>
    /// Preserves the registration signature emitted by earlier source-generator versions.
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

    internal static bool IsAssemblyProcessed(Assembly assembly) => ProcessedAssemblies.ContainsKey(assembly);

    private sealed record CommandMetadata(IReadOnlyList<PropertyCommandLinePart> Model, bool IsComplete);
}
