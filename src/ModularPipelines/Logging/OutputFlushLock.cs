namespace ModularPipelines.Logging;

/// <summary>
/// Singleton implementation of <see cref="IOutputFlushLock"/> that provides
/// a shared lock for coordinating console output flushing across modules.
/// </summary>
internal sealed class OutputFlushLock : IOutputFlushLock
{
    /// <inheritdoc />
    public object Lock { get; } = new();
}
