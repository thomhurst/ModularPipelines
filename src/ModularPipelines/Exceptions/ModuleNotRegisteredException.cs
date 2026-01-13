namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module depends on another module that has not been registered in the pipeline.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a module declares a dependency (via <c>[DependsOn]</c> attribute
/// or programmatic dependencies) on another module that has not been added to the pipeline.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a module's <c>[DependsOn&lt;T&gt;]</c> attribute references an unregistered module</item>
/// <item>When programmatic dependencies reference unregistered modules</item>
/// <item>When <c>context.GetModule&lt;T&gt;()</c> is called for an unregistered module</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (ModuleNotRegisteredException ex)
/// {
///     Console.WriteLine($"Missing module: {ex.Message}");
///     // The message contains suggestions for resolution
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <list type="number">
/// <item>Add the missing module: <c>.AddModule&lt;MissingModule&gt;()</c></item>
/// <item>Use optional dependencies: <c>[DependsOn&lt;T&gt;(IgnoreIfNotRegistered = true)]</c></item>
/// <item>Check for typos in the dependency type name</item>
/// <item>Verify the dependency module is in a referenced project</item>
/// </list>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="DependencyFailedException"/>
public class ModuleNotRegisteredException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleNotRegisteredException"/> class.
    /// </summary>
    /// <param name="message">The message describing which module is not registered.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public ModuleNotRegisteredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}