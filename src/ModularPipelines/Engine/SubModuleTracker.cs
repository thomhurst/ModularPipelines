using System.Diagnostics;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

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
internal sealed class SubModuleTracker : SubModuleBase
{
    private readonly Stopwatch _stopwatch = new();

    public SubModuleTracker(string name, Type parentModuleType)
        : base(parentModuleType, name)
    {
    }

    /// <summary>
    /// Executes an action and tracks its progress.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        StartTime = DateTimeOffset.UtcNow;
        Status = ModuleStatus.Running;
        _stopwatch.Start();

        try
        {
            var result = await action().ConfigureAwait(false);

            RecordCompletion(ModuleStatus.Succeeded);

            return result;
        }
        catch (Exception)
        {
            // Catch ALL exceptions including fatal ones - we need to record completion
            // before re-throwing. The immediate throw ensures propagation.
            RecordCompletion(ModuleStatus.Failed);
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

    private void RecordCompletion(ModuleStatus status)
    {
        _stopwatch.Stop();
        EndTime = DateTimeOffset.UtcNow;
        Duration = _stopwatch.Elapsed;
        Status = status;
    }
}
