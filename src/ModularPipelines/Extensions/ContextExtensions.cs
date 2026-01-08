using ModularPipelines.Context;
using ModularPipelines.Logging;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extension methods for pipeline context operations.
/// </summary>
public static class ContextExtensions
{
    /// <summary>
    /// Logs a message that will be displayed after the pipeline execution completes.
    /// </summary>
    /// <param name="pipelineContext">The pipeline hook context.</param>
    /// <param name="value">The message to log at the end of pipeline execution.</param>
    /// <remarks>
    /// This is useful for logging summary information or final status messages
    /// that should appear after all modules have completed.
    /// </remarks>
    public static void LogOnPipelineEnd(this IPipelineHookContext pipelineContext, string value)
    {
        var afterPipelineLogger = pipelineContext.Get<IAfterPipelineLogger>()
            ?? throw new InvalidOperationException(
                $"Service {nameof(IAfterPipelineLogger)} is not registered. Ensure the pipeline is properly configured.");
        afterPipelineLogger.LogOnPipelineEnd(value);
    }
}