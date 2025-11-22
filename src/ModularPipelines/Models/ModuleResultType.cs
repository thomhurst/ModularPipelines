namespace ModularPipelines.Models;

/// <summary>
/// Represents the result type of a module execution.
/// </summary>
public enum ModuleResultType
{
    /// <summary>
    /// The module executed successfully.
    /// </summary>
    Success,

    /// <summary>
    /// The module execution failed.
    /// </summary>
    Failure,

    /// <summary>
    /// The module was skipped and did not execute.
    /// </summary>
    Skipped,
}
