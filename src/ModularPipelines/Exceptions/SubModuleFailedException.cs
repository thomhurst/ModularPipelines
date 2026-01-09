using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a sub-module fails execution.
/// </summary>
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
