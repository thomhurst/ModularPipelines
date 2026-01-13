namespace ModularPipelines.Exceptions;

/// <summary>
/// An internal exception used to track deferred exceptions from dependencies of AlwaysRun modules.
/// </summary>
/// <remarks>
/// <para>
/// This exception is used internally by the pipeline engine when a module with
/// <c>ModuleRunType.AlwaysRun</c> has a dependency that throws an exception.
/// Instead of immediately failing the AlwaysRun module, the exception is captured
/// and the module is allowed to execute. The captured exceptions are reported
/// after pipeline completion.
/// </para>
/// <para><b>When this is used:</b></para>
/// <list type="bullet">
/// <item>When an AlwaysRun module's dependency fails</item>
/// <item>Internally to defer exception handling until after the AlwaysRun module executes</item>
/// </list>
/// <para>
/// <b>Note:</b> This is an internal exception used by the pipeline engine for exception
/// aggregation. External code typically does not need to catch this exception directly.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="DependencyFailedException"/>
public class AlwaysRunPostponedException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlwaysRunPostponedException"/> class.
    /// </summary>
    /// <param name="message">The message describing the postponed exception.</param>
    /// <param name="innerException">The original exception that was postponed.</param>
    public AlwaysRunPostponedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}