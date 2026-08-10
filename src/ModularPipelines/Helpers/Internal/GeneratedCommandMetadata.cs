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
    internal const int CurrentSchemaVersion = 3;

    private static readonly ConditionalWeakTable<Type, CommandMetadata> Models = [];
    private static readonly ConditionalWeakTable<Assembly, ProcessedAssembly> ProcessedAssemblies = [];
    private static readonly ConditionalWeakTable<Assembly, ExternalCommandMetadata> ExternalModels = [];

    /// <summary>
    /// Registers that an assembly ran the C# command metadata generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly)
    {
        RegisterAssembly(assembly, requiresGeneratedMetadata: false);
    }

    /// <summary>
    /// Registers that an assembly ran the C# metadata generator and whether reflection fallback is unsafe.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterAssembly(Assembly assembly, bool requiresGeneratedMetadata)
    {
        var processedAssembly = ProcessedAssemblies.GetValue(
            assembly,
            static _ => new ProcessedAssembly());
        if (requiresGeneratedMetadata)
        {
            processedAssembly.RequiresGeneratedMetadata = true;
        }
    }

    /// <summary>
    /// Registers command option types observed by the source generator.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterCoveredTypeNames(
        Assembly assembly,
        IReadOnlyList<string> metadataNames)
    {
        var processedAssembly = ProcessedAssemblies.GetValue(
            assembly,
            static _ => new ProcessedAssembly());
        foreach (var metadataName in metadataNames)
        {
            processedAssembly.CoveredTypeNames.TryAdd(metadataName, 0);
        }
    }

    /// <summary>
    /// Preserves the schema-1 registration signature emitted by earlier generator versions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model)
    {
        RegisterCore(optionsType, model, isComplete: true, schemaVersion: 1);
    }

    /// <summary>
    /// Registers the generated command model and its metadata schema version.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        int schemaVersion)
    {
        RegisterCore(optionsType, model, isComplete: true, schemaVersion);
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
        RegisterCore(optionsType, model, isComplete, schemaVersion: 0);
    }

    private static void RegisterCore(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        bool isComplete,
        int schemaVersion)
    {
        try
        {
            Models.Add(optionsType, new CommandMetadata(model, isComplete, schemaVersion));
        }
        catch (ArgumentException exception)
        {
            throw new InvalidOperationException(
                $"Command metadata is already registered for {optionsType}.",
                exception);
        }
    }

    /// <summary>
    /// Preserves the schema-1 external registration signature emitted by earlier generator versions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(
        Assembly consumerAssembly,
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model)
    {
        RegisterExternal(consumerAssembly, optionsType, model, schemaVersion: 1);
    }

    /// <summary>
    /// Registers versioned command metadata emitted by a consuming assembly for an external options type.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void RegisterExternal(
        Assembly consumerAssembly,
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model,
        int schemaVersion)
    {
        var registrations = ExternalModels.GetValue(
            consumerAssembly,
            static _ => new ExternalCommandMetadata());
        registrations.Models.TryAdd(
            optionsType,
            new CommandMetadata(model, IsComplete: true, schemaVersion));
    }

    internal static bool TryGet(Type optionsType, out IReadOnlyList<PropertyCommandLinePart> model)
    {
        Models.TryGetValue(optionsType, out var directMetadata);
        if (directMetadata is { IsComplete: true, SchemaVersion: CurrentSchemaVersion })
        {
            model = directMetadata.Model;
            return true;
        }

        foreach (var registrations in ExternalModels)
        {
            if (registrations.Value.Models.TryGetValue(optionsType, out var metadata)
                && metadata is { IsComplete: true, SchemaVersion: CurrentSchemaVersion })
            {
                model = metadata.Model;
                return true;
            }
        }

        if (directMetadata is { IsComplete: true })
        {
            model = directMetadata.Model;
            return true;
        }

        model = Array.Empty<PropertyCommandLinePart>();
        return false;
    }

    internal static bool IsAssemblyProcessed(Assembly assembly) =>
        ProcessedAssemblies.TryGetValue(assembly, out _);

    internal static bool IsGeneratedMetadataRequired(Assembly assembly) =>
        ProcessedAssemblies.TryGetValue(assembly, out var processedAssembly)
        && processedAssembly.RequiresGeneratedMetadata;

    internal static bool IsTypeCovered(Type type)
    {
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return metadataType.FullName is { } metadataName
               && ProcessedAssemblies.TryGetValue(type.Assembly, out var processedAssembly)
               && processedAssembly.CoveredTypeNames.ContainsKey(metadataName);
    }

    private sealed record CommandMetadata(
        IReadOnlyList<PropertyCommandLinePart> Model,
        bool IsComplete,
        int SchemaVersion);

    private sealed class ExternalCommandMetadata
    {
        public ConcurrentDictionary<Type, CommandMetadata> Models { get; } = [];
    }

    private sealed class ProcessedAssembly
    {
        public bool RequiresGeneratedMetadata { get; set; }

        public ConcurrentDictionary<string, byte> CoveredTypeNames { get; } = new(StringComparer.Ordinal);
    }
}
