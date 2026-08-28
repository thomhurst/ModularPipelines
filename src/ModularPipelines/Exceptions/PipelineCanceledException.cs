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
/// catch (PipelineCanceledException ex)
/// {
///     Console.WriteLine($"Pipeline was cancelled: {ex.Message}");
///     // The message may contain the cancellation reason
/// }
/// </code>
/// <para><b>Note:</b></para>
/// <para>
/// This type derives from <see cref="OperationCanceledException"/> so idiomatic cancellation
/// handlers catch it. Consequently, it does not derive from <see cref="PipelineException"/>.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="ModuleTimeoutException"/>
public class PipelineCanceledException : OperationCanceledException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCanceledException"/> class.
    /// </summary>
    public PipelineCanceledException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCanceledException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the cancellation.</param>
    public PipelineCanceledException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCanceledException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the cancellation.</param>
    /// <param name="cancellationToken">The cancellation token associated with the operation.</param>
    public PipelineCanceledException(string? message, CancellationToken cancellationToken)
        : base(message, cancellationToken)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCanceledException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the cancellation.</param>
    /// <param name="innerException">The exception that caused the cancellation.</param>
    /// <param name="cancellationToken">The cancellation token associated with the operation.</param>
    public PipelineCanceledException(
        string? message,
        Exception? innerException,
        CancellationToken cancellationToken)
        : base(message, innerException, cancellationToken)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineCanceledException"/> class.
    /// </summary>
    /// <param name="engineCancellationToken">The engine cancellation token containing the reason for cancellation.</param>
    internal PipelineCanceledException(EngineCancellationToken engineCancellationToken)
        : base(GenerateMessage(engineCancellationToken), engineCancellationToken.Token)
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
