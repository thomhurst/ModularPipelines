using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to provide custom error handling logic for a module.
/// This allows you to ignore certain failures and allow the pipeline to continue.
/// </summary>
public interface IModuleErrorHandling
{
    /// <summary>
    /// Determines whether failures from this module should be ignored.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="exception">The exception that occurred during execution.</param>
    /// <returns>True to ignore the failure and continue the pipeline; false to fail the pipeline.</returns>
    Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception);
}
