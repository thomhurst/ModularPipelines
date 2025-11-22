namespace ModularPipelines;

/// <summary>
/// Used for writing to the console.
/// </summary>
public interface IConsoleWriter
{
    /// <summary>
    /// Writes a value to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void LogToConsole(string value);
}
