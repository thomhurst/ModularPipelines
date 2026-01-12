using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a type is expected to be a module but does not implement IModule.
/// </summary>
[ExcludeFromCodeCoverage]
public class InvalidModuleTypeException : PipelineException
{
    /// <summary>
    /// Gets the type that was expected to be a module but is not.
    /// </summary>
    public Type InvalidType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidModuleTypeException"/> class.
    /// </summary>
    /// <param name="invalidType">The type that does not implement IModule.</param>
    public InvalidModuleTypeException(Type invalidType)
        : base($"{invalidType.FullName} is not a Module (does not implement IModule)")
    {
        InvalidType = invalidType;
    }
}
