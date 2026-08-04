using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Initializes the pipeline.
/// </summary>
internal interface IPipelineInitializer
{
    /// <summary>
    /// Gets the registered modules before initialization performs fallible startup work.
    /// </summary>
    IReadOnlyList<IModule> RegisteredModules { get; }

    /// <summary>
    /// Initializes the pipeline.
    /// </summary>
    /// <returns>The modules to run.</returns>
    Task<OrganizedModules> Initialize(CancellationToken cancellationToken = default);
}
