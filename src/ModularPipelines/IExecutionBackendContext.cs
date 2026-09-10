using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Provides engine operations needed by execution backends.
/// </summary>
public interface IExecutionBackendContext
{
    /// <summary>
    /// Applies a backend-produced result to its local module awaitable and result registry.
    /// </summary>
    /// <param name="module">The local module represented by the result.</param>
    /// <param name="result">The completed module result.</param>
    /// <returns><see langword="true"/> when the module awaitable accepted the result; otherwise <see langword="false"/>.</returns>
    bool TryApplyResult(IModule module, IModuleResult result);
}
