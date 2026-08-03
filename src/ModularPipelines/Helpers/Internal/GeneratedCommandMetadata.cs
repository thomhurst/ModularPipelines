using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Stores command models emitted by ModularPipelines.SourceGenerator.
/// </summary>
public static class GeneratedCommandMetadata
{
    private static readonly ConditionalWeakTable<Type, CommandMetadata> Models = [];
    private static readonly ConditionalWeakTable<Assembly, ProcessedAssembly> ProcessedAssemblies = [];
    private static readonly ConditionalWeakTable<Assembly, ExternalCommandMetadata> ExternalModels = [];

    /// <summary>
    /// Registers that an assembly ran the C# command metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        _ = ProcessedAssemblies.GetValue(assembly, static _ => new ProcessedAssembly());
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
        try
        {
            Models.Add(optionsType, new CommandMetadata(model, isComplete));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException(
                $"Command metadata is already registered for {optionsType}.",
                exception);
        }
    }

    /// <summary>
    /// Registers command metadata emitted by a consuming assembly for an external options type.
    /// Registrations are scoped weakly to the consumer so collectible assemblies are not retained.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(
        Assembly consumerAssembly,
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model)
    {
        var registrations = ExternalModels.GetValue(
            consumerAssembly,
            static _ => new ExternalCommandMetadata());
        registrations.Models.TryAdd(optionsType, new CommandMetadata(model, IsComplete: true));
    }

    internal static bool TryGet(Type optionsType, out IReadOnlyList<PropertyCommandLinePart> model)
    {
        if (Models.TryGetValue(optionsType, out var metadata) && metadata.IsComplete)
        {
            model = metadata.Model;
            return true;
        }

        foreach (var registrations in ExternalModels)
        {
            if (registrations.Value.Models.TryGetValue(optionsType, out metadata)
                && metadata.IsComplete)
            {
                model = metadata.Model;
                return true;
            }
        }

        model = Array.Empty<PropertyCommandLinePart>();
        return false;
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) =>
        ProcessedAssemblies.TryGetValue(assembly, out _);

    private sealed record CommandMetadata(IReadOnlyList<PropertyCommandLinePart> Model, bool IsComplete);

    private sealed class ExternalCommandMetadata
    {
        public ConcurrentDictionary<Type, CommandMetadata> Models { get; } = [];
    }

    private sealed class ProcessedAssembly;
}
