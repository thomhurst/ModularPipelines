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
    /// <param name="pipelineContext">The pipeline context.</param>
    /// <param name="value">The message to log at the end of pipeline execution.</param>
    /// <remarks>
    /// <para>
    /// This method is provided for backward compatibility. For new code, prefer using
    /// <c>context.Summary.Info()</c>, <c>context.Summary.Success()</c>, or other typed methods.
    /// </para>
    /// <para><b>Example migration:</b></para>
    /// <code>
    /// // Old way (deprecated)
    /// context.LogOnPipelineEnd($"Version: {version}");
    ///
    /// // New way (preferred)
    /// context.Summary.KeyValue("Version", version);
    /// // or
    /// context.Summary.Info($"Version: {version}");
    /// </code>
    /// </remarks>
    [Obsolete("Use context.Summary.Info(), context.Summary.KeyValue(), or other ISummaryLogger methods instead. This method will be removed in a future version.")]
    public static void LogOnPipelineEnd(this IPipelineContext pipelineContext, string value)
    {
        pipelineContext.Summary.Info(value);
    }
}
