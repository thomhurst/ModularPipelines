namespace ModularPipelines.Logging;

/// <summary>
/// Buffers exceptions and console output that would normally be printed immediately,
/// allowing them to be printed after the pipeline results table.
/// </summary>
internal interface IExceptionBuffer
{
    /// <summary>
    /// Adds an exception message to the buffer to be printed later.
    /// </summary>
    /// <param name="message">The exception message to buffer.</param>
    void AddExceptionMessage(string message);

    /// <summary>
    /// Prints all buffered exception messages to the console.
    /// Should be called after the pipeline results table has been printed.
    /// </summary>
    void FlushExceptions();

    /// <summary>
    /// Gets a value indicating whether gets whether there are any buffered exceptions.
    /// </summary>
    bool HasExceptions { get; }
}