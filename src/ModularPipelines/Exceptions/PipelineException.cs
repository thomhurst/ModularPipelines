namespace ModularPipelines.Exceptions;

/// <summary>
/// The base exception type for all pipeline-related exceptions in ModularPipelines.
/// </summary>
/// <remarks>
/// <para>
/// This is the root exception class from which all ModularPipelines-specific exceptions derive.
/// It provides a common base for catching and handling any pipeline-related error.
/// </para>
/// <para><b>Exception Hierarchy:</b></para>
/// <list type="bullet">
/// <item><see cref="PipelineException"/> - Base exception for all pipeline errors</item>
/// <item><see cref="CommandException"/> - CLI command execution failures</item>
/// <item><see cref="CircularDependencyException"/> - Circular module dependency detected</item>
/// <item><see cref="DependencyCollisionException"/> - Module dependency conflicts</item>
/// <item><see cref="DependencyFailedException"/> - A dependency module failed</item>
/// <item><see cref="FailedRequirementsException"/> - Pipeline requirements not met</item>
/// <item><see cref="HttpResponseException"/> - HTTP request failures</item>
/// <item><see cref="InvalidModuleTypeException"/> - Type does not implement IModule</item>
/// <item><see cref="ModuleNotRegisteredException"/> - Module dependency not registered</item>
/// <item><see cref="ModuleReferencingSelfException"/> - Module depends on itself</item>
/// <item><see cref="ModuleTimeoutException"/> - Module execution timed out</item>
/// <item><see cref="PipelineCancelledException"/> - Pipeline was cancelled</item>
/// <item><see cref="PipelineValidationException"/> - Pipeline validation failed</item>
/// <item><see cref="PluginVersionMismatchException"/> - Plugin version incompatibility</item>
/// <item><see cref="SubModuleFailedException"/> - Sub-module execution failed</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (PipelineException ex)
/// {
///     // Handle any pipeline-related error
///     Console.WriteLine($"Pipeline error: {ex.Message}");
/// }
/// </code>
/// </remarks>
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