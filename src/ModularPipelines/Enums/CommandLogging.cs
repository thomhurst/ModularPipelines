using System;

namespace ModularPipelines.Enums;

/// <summary>
/// Enum to control the level of logging a command should do
/// Can combine multiple e.g. Input | Error
/// </summary>
[Flags]
public enum CommandLogging
{
    /// <summary>
    /// No logging
    /// </summary>
    None = 0,

    /// <summary>
    /// Log command input
    /// </summary>
    Input = 1 << 0,

    /// <summary>
    /// Log command standard output
    /// </summary>
    Output = 1 << 1,

    /// <summary>
    /// Log command errors
    /// </summary>
    Error = 1 << 2,

    /// <summary>
    /// Log command duration
    /// </summary>
    Duration = 1 << 3,

    /// <summary>
    /// Log command exit code
    /// </summary>
    ExitCode = 1 << 4,
}