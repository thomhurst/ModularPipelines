namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Reports scheduler status at regular intervals for diagnostic purposes.
/// </summary>
/// <remarks>
/// This interface encapsulates the periodic status logging responsibility
/// previously embedded in ModuleScheduler, following the Single Responsibility Principle.
/// The 15-second interval status reporting is a cross-cutting concern that helps
/// diagnose pipeline progress and identify blocked modules.
/// </remarks>
internal interface ISchedulerStatusReporter
{
    /// <summary>
    /// Logs the current scheduler status if the reporting interval has elapsed.
    /// </summary>
    /// <remarks>
    /// This method is designed to be called frequently (e.g., each scheduler iteration)
    /// but only produces output when the configured interval has passed since the last report.
    /// </remarks>
    /// <param name="stateQueries">Query helper for accessing module states.</param>
    /// <param name="stateLock">Lock protecting state access.</param>
    void LogStatusIfIntervalElapsed(ModuleStateQueries stateQueries, ReaderWriterLockSlim stateLock);
}
