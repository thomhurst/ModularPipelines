namespace ModularPipelines.Interfaces;

internal interface IInternalCollapsableLogging
{
    /// <summary>
    /// Start a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void StartConsoleLogGroupDirectToConsole(string name);
    
    /// <summary>
    /// Ends a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void EndConsoleLogGroupDirectToConsole(string name);
    
    /// <summary>
    /// Log to the console.
    /// </summary>
    /// <param name="value">The value to log.</param>
    void LogToConsoleDirect(string value);
    
    /// <summary>
    /// Opens, writes the value, and closes the log group
    /// </summary>
    /// <param name="groupName">The name of the log group.</param>
    /// <param name="value">The value to write inside the log group.</param>
    void WriteConsoleLogGroupInternal(string groupName, string value)
    {
        StartConsoleLogGroupDirectToConsole(groupName);
        LogToConsoleDirect(value);
        EndConsoleLogGroupDirectToConsole(groupName);
    }
}