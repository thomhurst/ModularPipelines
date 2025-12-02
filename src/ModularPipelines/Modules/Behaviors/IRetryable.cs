using ModularPipelines.Context;
using Polly.Retry;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to define a retry policy for module execution.
/// </summary>
/// <typeparam name="T">The result type of the module.</typeparam>
/// <remarks>
/// If not implemented, modules do not retry on failure.
/// The retry policy is applied to the <see cref="Module{T}.ExecuteAsync"/> call.
/// </remarks>
public interface IRetryable<T>
{
    /// <summary>
    /// Gets the retry policy to apply to this module's execution.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>An async retry policy for the module result type.</returns>
    AsyncRetryPolicy<T?> GetRetryPolicy(IPipelineContext context);
}
