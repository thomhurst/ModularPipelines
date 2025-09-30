namespace ModularPipelines.Logging;

/// <summary>
/// Defines the contract for formatting and outputting deferred exception messages.
/// Implementations can provide different output strategies (console, file, structured logging, etc.).
/// </summary>
/// <example>
/// <code>
/// public class CustomExceptionFormatter : IExceptionOutputFormatter
/// {
///     public void FormatAndOutput(IEnumerable&lt;string&gt; exceptionMessages)
///     {
///         foreach (var message in exceptionMessages)
///         {
///             // Custom formatting logic
///             File.AppendAllText("errors.log", message + Environment.NewLine);
///         }
///     }
/// }
/// </code>
/// </example>
public interface IExceptionOutputFormatter
{
    /// <summary>
    /// Formats and outputs the provided exception messages.
    /// </summary>
    /// <param name="exceptionMessages">The exception messages to format and output.</param>
    void FormatAndOutput(IEnumerable<string> exceptionMessages);
}
