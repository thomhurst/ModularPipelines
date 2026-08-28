using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Non-generic interface for type-erased module result access.
/// </summary>
public interface IModuleResult
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Gets the fully qualified module type name, when available.
    /// </summary>
    string? ModuleTypeName => null;

    /// <summary>
    /// Gets how long the module ran for.
    /// </summary>
    TimeSpan ModuleDuration { get; }

    /// <summary>
    /// Gets when the module started.
    /// </summary>
    DateTimeOffset ModuleStart { get; }

    /// <summary>
    /// Gets when the module ended.
    /// </summary>
    DateTimeOffset ModuleEnd { get; }

    /// <summary>
    /// Gets the status of the module.
    /// </summary>
    ModuleStatus Status { get; }

    /// <summary>
    /// Gets the value if successful, or null/default otherwise. Does not throw.
    /// </summary>
    object? ValueOrDefault { get; }

    /// <summary>
    /// Gets the exception if failed, or null otherwise.
    /// </summary>
    Exception? ExceptionOrDefault { get; }

    /// <summary>
    /// Gets the skip decision if skipped, or null otherwise.
    /// </summary>
    SkipDecision? SkipDecisionOrDefault { get; }
}
