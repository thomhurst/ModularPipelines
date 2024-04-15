using ModularPipelines.Enums;

namespace ModularPipelines.Models;

public interface IModuleResult
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// Gets how long the module ran for.
    /// </summary>
    public TimeSpan ModuleDuration { get; }

    /// <summary>
    /// Gets when the module started.
    /// </summary>
    public DateTimeOffset ModuleStart { get; }

    /// <summary>
    /// Gets when the module ended.
    /// </summary>
    public DateTimeOffset ModuleEnd { get; }
    
    /// <summary>
    /// Gets the exception that occurred in the module, if one was thrown.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Gets the Skip Decision of the module.
    /// </summary>
    public SkipDecision SkipDecision { get; }

    /// <summary>
    /// Gets the type of result that is held.
    /// </summary>
    public ModuleResultType ModuleResultType { get; }
    
    /// <summary>
    /// Gets the status of the module.
    /// </summary>
    public Status ModuleStatus { get; }
}