namespace ModularPipelines.Interfaces;

/// <summary>
/// Defines a limit for parallel execution.
/// </summary>
public interface IParallelLimit
{
    /// <summary>
    /// Gets the maximum number of operations that can execute in parallel.
    /// </summary>
    int Limit { get; }
}