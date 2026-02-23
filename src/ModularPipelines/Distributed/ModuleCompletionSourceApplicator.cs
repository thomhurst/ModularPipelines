using System.Reflection;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

/// <summary>
/// Applies a deserialized <see cref="IModuleResult"/> to a module's internal <c>CompletionSource</c>
/// via reflection, since the generic type parameter T is not known at compile time.
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
    /// <returns>True if the result was successfully applied; false if the CompletionSource could not be found.</returns>
    public static bool TryApply(IModule module, IModuleResult result)
    {
        var completionSource = module.GetType()
            .GetProperty("CompletionSource", BindingFlags.Instance | BindingFlags.NonPublic)?
            .GetValue(module);

        if (completionSource is null)
        {
            return false;
        }

        completionSource.GetType()
            .GetMethod("TrySetResult")?
            .Invoke(completionSource, [result]);

        return true;
    }
}
