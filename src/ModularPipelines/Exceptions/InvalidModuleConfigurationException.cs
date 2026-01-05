namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when module configuration is invalid.
/// This includes missing dependencies, circular dependencies, and self-referencing modules.
/// </summary>
public class InvalidModuleConfigurationException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidModuleConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidModuleConfigurationException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidModuleConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidModuleConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
