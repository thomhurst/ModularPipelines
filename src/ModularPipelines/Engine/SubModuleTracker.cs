using System.Diagnostics;
using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

/// <summary>
/// Lightweight tracker for sub-operations within a module.
/// </summary>
/// <remarks>
/// SubModuleTracker provides progress tracking for nested operations without the
/// full complexity of a module. It tracks status, timing, and completion.
/// This class is internal because it is only used within the engine infrastructure
/// and is not intended for direct use by external consumers.
/// </remarks>
internal class SubModuleTracker
{
    private readonly Stopwatch _stopwatch = new();

    public SubModuleTracker(string name, Type parentModuleType)
    {
        Name = name;
        ParentModuleType = parentModuleType;
    }

    /// <summary>
    /// Gets the name of this sub-operation.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the parent module.
    /// </summary>
    public Type ParentModuleType { get; }

    /// <summary>
    /// Gets the current status.
    /// </summary>
    public Status Status { get; private set; } = Status.NotYetStarted;

    /// <summary>
    /// Gets when the sub-operation started.
    /// </summary>
    public DateTimeOffset StartTime { get; private set; }

    /// <summary>
    /// Gets when the sub-operation ended.
    /// </summary>
    public DateTimeOffset EndTime { get; private set; }

    /// <summary>
    /// Gets the duration of the sub-operation.
    /// </summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>
    /// Executes an action and tracks its progress.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        StartTime = DateTimeOffset.UtcNow;
        Status = Status.Processing;
        _stopwatch.Start();

        try
        {
            var result = await action().ConfigureAwait(false);

            RecordCompletion(Status.Successful);

            return result;
        }
        catch (Exception)
        {
            // Catch ALL exceptions including fatal ones - we need to record completion
            // before re-throwing. The immediate throw ensures propagation.
            RecordCompletion(Status.Failed);
            throw;
        }
    }

    /// <summary>
    /// Executes an action and tracks its progress.
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action)
    {
        await ExecuteAsync(async () =>
        {
            await action().ConfigureAwait(false);
            return 0;
        }).ConfigureAwait(false);
    }

    private void RecordCompletion(Status status)
    {
        _stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = _stopwatch.Elapsed;
        Status = status;
    }
}
