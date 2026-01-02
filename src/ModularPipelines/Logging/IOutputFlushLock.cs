namespace ModularPipelines.Logging;

/// <summary>
/// Provides a shared lock for coordinating console output flushing across modules.
/// This ensures that module outputs do not interleave when flushing to the console.
/// </summary>
/// <remarks>
/// Registered as a singleton to share the lock across all <see cref="ModuleOutputWriter"/> instances.
/// This replaces the static lock that was previously in <see cref="ModuleOutputWriter"/>,
/// following proper DI patterns and enabling testability.
/// </remarks>
internal interface IOutputFlushLock
{
    /// <summary>
    /// Gets the lock object used to synchronize console output flushing.
    /// Use this with <c>lock</c> statement to ensure atomic output blocks.
    /// </summary>
    object Lock { get; }
}
