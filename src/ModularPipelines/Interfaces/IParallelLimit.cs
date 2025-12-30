namespace ModularPipelines.Interfaces;

/// <summary>
/// Defines a limit for parallel execution.
/// </summary>
/// <remarks>
/// Implementations must provide a static <see cref="Limit"/> property.
/// This is a breaking change from the previous instance-based design,
/// enabling compile-time resolution without reflection.
/// </remarks>
/// <example>
/// <code>
/// public class MyDatabaseLimit : IParallelLimit
/// {
///     public static int Limit => 5;
/// }
/// </code>
/// </example>
public interface IParallelLimit
{
    /// <summary>
    /// Gets the maximum number of operations that can execute in parallel.
    /// </summary>
    static abstract int Limit { get; }
}