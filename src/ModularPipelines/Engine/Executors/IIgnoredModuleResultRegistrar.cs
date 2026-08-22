using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Registers skipped results for modules that were ignored via Category or RunCondition.
/// This ensures tests and other code can retrieve results for these modules.
/// If a history repository is configured and has a cached result, it will be used.
/// </summary>
internal interface IIgnoredModuleResultRegistrar
{
    /// <summary>
    /// Registers ignored results and cascades modules whose required dependencies
    /// remain skipped after history restoration.
    /// </summary>
    Task<OrganizedModules> RegisterIgnoredModuleResultsAsync(OrganizedModules organizedModules);

    /// <summary>
    /// Resolves ignored modules and history for planning without mutating runtime results.
    /// </summary>
    Task<IgnoredModuleResolution> ResolveIgnoredModuleResultsAsync(
        OrganizedModules organizedModules,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IReadOnlyDictionary<IModule, IModule> historyModules,
        CancellationToken cancellationToken);

    /// <summary>
    /// Resolves which newly ignored planning modules have historical results.
    /// </summary>
    Task<IReadOnlySet<Type>> ResolveHistoryModuleTypesAsync(
        IEnumerable<IModule> ignoredModules,
        IReadOnlyDictionary<IModule, IModule> historyModules,
        CancellationToken cancellationToken);
}

internal sealed record IgnoredModuleResolution(
    OrganizedModules OrganizedModules,
    IReadOnlySet<Type> UsedHistoryModuleTypes);
