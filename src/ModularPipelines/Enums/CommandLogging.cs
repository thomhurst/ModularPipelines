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
    None = 1,
    
    /// <summary>
    /// Log command input
    /// </summary>
    Input = 2,
    
    /// <summary>
    /// Log command standard output
    /// </summary>
    Output = 4,
    
    /// <summary>
    /// Log command errors
    /// </summary>
    Error = 8,
}