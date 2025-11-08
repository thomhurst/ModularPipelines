using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service responsible for tracking module state (status, timing, exceptions).
/// This replaces the state management previously embedded in ModuleBase.
/// </summary>
public interface IModuleStateTracker
{
    /// <summary>
    /// Gets the current status of a module.
    /// </summary>
    /// <param name="module">The module to query.</param>
    /// <returns>The module's current status.</returns>
    Status GetStatus(IModule module);

    /// <summary>
    /// Sets the status of a module.
    /// </summary>
    /// <param name="module">The module to update.</param>
    /// <param name="status">The new status.</param>
    void SetStatus(IModule module, Status status);

    /// <summary>
    /// Gets the start time of a module's execution.
    /// </summary>
    /// <param name="module">The module to query.</param>
    /// <returns>The start time, or null if not yet started.</returns>
    DateTimeOffset? GetStartTime(IModule module);

    /// <summary>
    /// Sets the start time of a module's execution.
    /// </summary>
    /// <param name="module">The module to update.</param>
    /// <param name="startTime">The start time.</param>
    void SetStartTime(IModule module, DateTimeOffset startTime);

    /// <summary>
    /// Gets the end time of a module's execution.
    /// </summary>
    /// <param name="module">The module to query.</param>
    /// <returns>The end time, or null if not yet completed.</returns>
    DateTimeOffset? GetEndTime(IModule module);

    /// <summary>
    /// Sets the end time of a module's execution.
    /// </summary>
    /// <param name="module">The module to update.</param>
    /// <param name="endTime">The end time.</param>
    void SetEndTime(IModule module, DateTimeOffset endTime);

    /// <summary>
    /// Gets the duration of a module's execution.
    /// </summary>
    /// <param name="module">The module to query.</param>
    /// <returns>The duration, or null if not yet completed.</returns>
    TimeSpan? GetDuration(IModule module);

    /// <summary>
    /// Gets the exception thrown by a module (if any).
    /// </summary>
    /// <param name="module">The module to query.</param>
    /// <returns>The exception, or null if no exception occurred.</returns>
    Exception? GetException(IModule module);

    /// <summary>
    /// Sets the exception thrown by a module.
    /// </summary>
    /// <param name="module">The module to update.</param>
    /// <param name="exception">The exception.</param>
    void SetException(IModule module, Exception exception);
}
