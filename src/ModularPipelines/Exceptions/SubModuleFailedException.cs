using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a sub-module fails during execution within a parent module.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a sub-module (a module executed within another module)
/// fails during execution. Sub-modules allow for hierarchical module composition where
/// a parent module can execute child modules as part of its work.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a sub-module's execution throws an exception</item>
/// <item>When <c>SubModule.ExecuteAsync</c> fails within a parent module</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="SubModuleName"/> - The name of the sub-module that failed</item>
/// <item><see cref="ParentModuleType"/> - The type of the parent module containing the sub-module</item>
/// <item><see cref="Exception.InnerException"/> - The original exception from the sub-module</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (SubModuleFailedException ex)
/// {
///     Console.WriteLine($"Sub-module '{ex.SubModuleName}' failed");
///     if (ex.ParentModuleType != null)
///     {
///         Console.WriteLine($"Parent module: {ex.ParentModuleType.Name}");
///     }
///     Console.WriteLine($"Error: {ex.InnerException?.Message}");
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="DependencyFailedException"/>
public class SubModuleFailedException : PipelineException
{
    /// <summary>
    /// Gets the name of the sub-module that failed.
    /// </summary>
    public string? SubModuleName { get; }

    /// <summary>
    /// Gets the type of the parent module.
    /// </summary>
    public Type? ParentModuleType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubModuleFailedException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SubModuleFailedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubModuleFailedException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception that caused this failure.</param>
    public SubModuleFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubModuleFailedException"/> class.
    /// </summary>
    /// <param name="submodule">The sub-module that failed.</param>
    /// <param name="exception">The inner exception that caused this failure.</param>
    public SubModuleFailedException(SubModuleBase submodule, Exception exception) :
        base($"The Sub-Module {submodule.Name} has failed.", exception)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubModuleFailedException"/> class.
    /// </summary>
    /// <param name="subModuleName">The name of the sub-module that failed.</param>
    /// <param name="parentModuleType">The type of the parent module.</param>
    /// <param name="innerException">The inner exception that caused this failure.</param>
    public SubModuleFailedException(string subModuleName, Type parentModuleType, Exception innerException)
        : base($"SubModule '{subModuleName}' in module '{parentModuleType.Name}' failed.", innerException)
    {
        SubModuleName = subModuleName;
        ParentModuleType = parentModuleType;
    }
}
