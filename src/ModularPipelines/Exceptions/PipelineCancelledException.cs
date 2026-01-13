using ModularPipelines.Engine;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when the pipeline execution is cancelled.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when the pipeline is terminated before completion, either due to
/// explicit cancellation, a cancellation token being triggered, or an unrecoverable error
/// that requires pipeline termination.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When <c>CancellationToken.Cancel()</c> is called on the pipeline's token</item>
/// <item>When the pipeline engine determines execution should be terminated</item>
/// <item>When a critical error occurs that prevents pipeline continuation</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync(cancellationToken);
/// }
/// catch (PipelineCancelledException ex)
/// {
///     Console.WriteLine($"Pipeline was cancelled: {ex.Message}");
///     // The message may contain the cancellation reason
/// }
/// catch (OperationCanceledException)
/// {
///     Console.WriteLine("Pipeline was cancelled via cancellation token");
/// }
/// </code>
/// <para><b>Note:</b></para>
/// <para>
/// When handling cancellation, you may also need to catch <see cref="OperationCanceledException"/>
/// and <see cref="TaskCanceledException"/> as these can also be thrown during cancellation.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="ModuleTimeoutException"/>
public class PipelineCancelledException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCancelledException"/> class.
    /// </summary>
    /// <param name="engineCancellationToken">The engine cancellation token containing the reason for cancellation.</param>
    internal PipelineCancelledException(EngineCancellationToken engineCancellationToken) : base(GenerateMessage(engineCancellationToken))
    {
    }

    private static string? GenerateMessage(EngineCancellationToken engineCancellationToken)
    {
        if (string.IsNullOrWhiteSpace(engineCancellationToken.Reason))
        {
            return "The pipeline has been terminated.";
        }

        return
            $"The pipeline has been terminated. {Environment.NewLine}Cancellation Reason: {engineCancellationToken.Reason}";
    }
}