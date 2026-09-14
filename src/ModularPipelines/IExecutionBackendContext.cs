using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines;

/// <summary>
/// Provides engine operations needed by execution backends.
/// </summary>
public interface IExecutionBackendContext
{
    /// <summary>
    /// Executes a planned module in this process through the engine's dependency, scope,
    /// hook, retry, artifact, logging, and result lifecycle.
    /// </summary>
    /// <remarks>
    /// Use the context passed to <see cref="IExecutionBackend.ExecuteAsync"/>. The backend
    /// must request execution of dependencies or apply their remote results, and await its
    /// execution requests before returning. Concurrent requests for the same module share
    /// one execution; the first request's token controls that execution.
    /// </remarks>
    /// <param name="module">The exact module instance supplied in the execution plan.</param>
    /// <param name="cancellationToken">Requests cancellation of the module execution.</param>
    /// <returns>The result after the module lifecycle and its scope have completed.</returns>
    Task<IModuleResult> ExecuteModuleAsync(IModule module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Applies a backend-produced result to its local module awaitable and result registry.
    /// </summary>
    /// <param name="module">The local module represented by the result.</param>
    /// <param name="result">The completed module result.</param>
    /// <returns><see langword="true"/> when the module awaitable accepted the result; otherwise <see langword="false"/>.</returns>
    bool TryApplyResult(IModule module, IModuleResult result);
}
