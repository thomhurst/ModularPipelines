namespace ModularPipelines.Logging;

/// <summary>
/// Defines the contract for formatting and outputting deferred exception messages.
/// Implementations can provide different output strategies (console, file, structured logging, etc.).
/// </summary>
internal interface IExceptionOutputFormatter
{
    /// <summary>
    /// Formats and outputs the provided exception messages.
    /// </summary>
    /// <param name="exceptionMessages">The exception messages to format and output.</param>
    void FormatAndOutput(IEnumerable<string> exceptionMessages);
}
