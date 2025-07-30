namespace ModularPipelines.Models;

/// <summary>
/// Defines when a module should run based on dependency results.
/// </summary>
public enum ModuleRunType
{
    /// <summary>
    /// The module will always run regardless of dependency results.
    /// </summary>
    AlwaysRun,
    
    /// <summary>
    /// The module will only run if all its dependencies completed successfully.
    /// </summary>
    OnSuccessfulDependencies,
}