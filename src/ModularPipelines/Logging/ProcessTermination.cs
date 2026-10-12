namespace ModularPipelines.Logging;

/// <summary>
/// Identifies a process that the framework is about to terminate.
/// </summary>
/// <param name="ProcessId">The process ID, or null when it could not be read.</param>
/// <param name="ProcessName">The process name, or null when it could not be read.</param>
internal readonly record struct ProcessTerminationTarget(int? ProcessId, string? ProcessName);

/// <summary>
/// Why the framework terminates a command process or one of its descendants.
/// </summary>
internal enum ProcessTerminationReason
{
    /// <summary>The graceful shutdown window elapsed after cancellation.</summary>
    ForcefulCancellation,

    /// <summary>The command process started after forceful cancellation was already requested.</summary>
    StartedAfterCancellation,

    /// <summary>A descendant process was discovered after forceful cancellation was requested.</summary>
    DescendantCapturedAfterCancellation,
}

/// <summary>
/// How the framework terminates a process.
/// </summary>
internal enum ProcessTerminationMethod
{
    /// <summary>Kills the process and every descendant the runtime can find.</summary>
    ProcessTree,

    /// <summary>Terminates the Windows job object that contains the process tree.</summary>
    WindowsJob,
}
