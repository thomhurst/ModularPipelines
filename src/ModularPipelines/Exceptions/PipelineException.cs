namespace ModularPipelines.Exceptions;

/// <summary>
/// The base exception type for all pipeline-related exceptions.
/// </summary>
public class PipelineException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineException"/> class.
    /// </summary>
    public PipelineException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PipelineException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PipelineException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
