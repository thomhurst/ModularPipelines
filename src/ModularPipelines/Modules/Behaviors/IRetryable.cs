using ModularPipelines.Context;
using Polly.Retry;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to define a retry policy for module execution.
/// </summary>
/// <typeparam name="T">The result type of the module.</typeparam>
/// <remarks>
/// <para>
/// <strong>Configuration Precedence:</strong>
/// Implementing this interface on a module takes precedence over the global
/// <see cref="Options.PipelineOptions.DefaultRetryCount"/> setting.
/// </para>
/// <para>
/// If not implemented, modules will use <see cref="Options.PipelineOptions.DefaultRetryCount"/>
/// if set to a value greater than 0. Otherwise, no retries are attempted.
/// </para>
/// <para>
/// The retry policy is applied to the <see cref="Module{T}.ExecuteAsync"/> call.
/// </para>
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
