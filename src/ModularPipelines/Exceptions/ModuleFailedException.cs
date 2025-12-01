using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// An exception that occurs when a module execution fails.
/// </summary>
public class ModuleFailedException : PipelineException
{
    /// <summary>
    /// Gets the module that failed to execute (may be null for composition-based modules).
    /// </summary>
    public IModule? Module { get; }

    /// <summary>
    /// Gets the type of the module that failed.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleFailedException"/> class.
    /// </summary>
    /// <param name="module">The module that failed to execute.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    public ModuleFailedException(IModule module, Exception exception)
        : base($"The module {module.GetType().Name} has failed.{GetInnerMessage(exception)}", exception)
    {
        Module = module;
        ModuleType = module.GetType();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleFailedException"/> class.
    /// </summary>
    /// <param name="moduleType">The type of module that failed to execute.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    public ModuleFailedException(Type moduleType, Exception exception)
        : base($"The module {moduleType.Name} has failed.{GetInnerMessage(exception)}", exception)
    {
        ModuleType = moduleType;
    }

    private static string GetInnerMessage(Exception exception)
    {
        if (!string.IsNullOrEmpty(exception.Message))
        {
            return $"{Environment.NewLine}{Environment.NewLine}{exception.Message}";
        }

        return string.Empty;
    }
}
