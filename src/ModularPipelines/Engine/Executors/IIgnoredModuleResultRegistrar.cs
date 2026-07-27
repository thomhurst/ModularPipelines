using ModularPipelines.Models;

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
}
