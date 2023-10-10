namespace ModularPipelines.Interfaces;

/// <summary>
/// For writing collapsable/grouped logs in supported build agents
/// </summary>
public interface ICollapsableLogging : IConsoleWriter
{
    /// <summary>
    /// Start a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void StartConsoleLogGroup(string name);

    /// <summary>
    /// Ends a collapsable log group
    /// </summary>
    /// <param name="name">The name of the log group.</param>
    void EndConsoleLogGroup(string name);

    /// <summary>
    /// Opens, writes the value, and closes the log group
    /// </summary>
    /// <param name="groupName">The name of the log group.</param>
    /// <param name="value">The value to write inside the log group.</param>
    void WriteConsoleLogGroup(string groupName, string value)
    {
        StartConsoleLogGroup(groupName);
        LogToConsole(value);
        EndConsoleLogGroup(groupName);
    }
}