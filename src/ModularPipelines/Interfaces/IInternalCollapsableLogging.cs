namespace ModularPipelines.Interfaces;

internal interface IInternalCollapsableLogging
{
    /// <summary>
    /// Start a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void StartConsoleLogGroupInternal(string name);
    
    /// <summary>
    /// Ends a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void EndConsoleLogGroupInternal(string name);
    
    /// <summary>
    /// Log to the console.
    /// </summary>
    /// <param name="value">The value to log.</param>
    void LogToConsoleInternal(string value);
    
    /// <summary>
    /// Opens, writes the value, and closes the log group
    /// </summary>
    /// <param name="groupName">The name of the log group.</param>
    /// <param name="value">The value to write inside the log group.</param>
    void WriteConsoleLogGroupInternal(string groupName, string value)
    {
        StartConsoleLogGroupInternal(groupName);
        LogToConsoleInternal(value);
        EndConsoleLogGroupInternal(groupName);
    }
}