using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

/// <summary>
/// Applies a deserialized <see cref="IModuleResult"/> to a module's <c>CompletionSource</c>
/// via the <see cref="IModule.TrySetDistributedResult"/> interface method.
/// </summary>
/// <remarks>
/// Used by both the master (collecting results from workers) and workers (applying dependency
/// results received in <see cref="ModuleAssignment.DependencyResults"/>).
/// <c>TrySetResult</c> is idempotent — safe to call even if the CompletionSource was already set.
/// </remarks>
internal static class ModuleCompletionSourceApplicator
{
    /// <summary>
    /// Sets the deserialized result on the module's internal <c>CompletionSource</c>.
    /// </summary>
    /// <param name="module">The module instance whose CompletionSource should be set.</param>
    /// <param name="result">The deserialized module result.</param>
    /// <returns>True if the result was successfully applied.</returns>
    public static bool TryApply(IModule module, IModuleResult result)
    {
        return module.TrySetDistributedResult(result);
    }
}
