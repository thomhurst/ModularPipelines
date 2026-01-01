using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleResultRepository
{
    /// <summary>
    /// Gets a value indicating whether this repository is enabled and should be used for storing/retrieving results.
    /// </summary>
    /// <remarks>
    /// When <c>false</c>, the pipeline will skip history-related operations.
    /// Implementations that provide actual storage should return <c>true</c>.
    /// </remarks>
    bool IsEnabled { get; }

    Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineHookContext pipelineContext);

    Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineHookContext pipelineContext);
}