using System.ComponentModel.DataAnnotations;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when command-line options fail DataAnnotations validation.
/// </summary>
public class CommandOptionsValidationException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandOptionsValidationException"/> class.
    /// </summary>
    /// <param name="message">The aggregated validation error message.</param>
    /// <param name="innerException">The underlying DataAnnotations validation exception.</param>
    public CommandOptionsValidationException(string message, ValidationException innerException)
        : base(message, innerException)
    {
    }
}
