using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Initializes the pipeline.
/// </summary>
internal interface IPipelineInitializer
{
    /// <summary>
    /// Initializes the pipeline.
    /// </summary>
    /// <returns>The modules to run.</returns>
    Task<OrganizedModules> Initialize();
}