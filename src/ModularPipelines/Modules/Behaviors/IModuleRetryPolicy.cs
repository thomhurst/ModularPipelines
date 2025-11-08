using Polly;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to provide a custom retry policy for a module.
/// If not implemented, no retry logic will be applied (unless configured via attributes).
/// </summary>
public interface IModuleRetryPolicy
{
    /// <summary>
    /// Gets the Polly retry policy to apply to this module's execution.
    /// </summary>
    /// <returns>An async policy that defines retry behavior.</returns>
    IAsyncPolicy GetRetryPolicy();
}
