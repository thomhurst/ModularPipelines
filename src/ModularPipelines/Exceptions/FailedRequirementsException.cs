namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when pipeline requirements are not satisfied before execution can begin.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when one or more pipeline requirements fail validation.
/// Requirements are conditions that must be met before a pipeline can execute, such as
/// operating system checks, permission validations, or environment prerequisites.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>During pipeline startup when requirements are validated</item>
/// <item>When a custom requirement implementation returns a failure</item>
/// <item>When environment prerequisites are not met</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (FailedRequirementsException ex)
/// {
///     Console.WriteLine($"Requirements not met: {ex.Message}");
///     // The message contains details about which requirements failed
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Review the exception message to identify which requirements failed, then ensure the
/// execution environment meets the necessary prerequisites.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="PipelineValidationException"/>
public class FailedRequirementsException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailedRequirementsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing which requirements failed.</param>
    public FailedRequirementsException(string? message) : base(message)
    {
    }
}