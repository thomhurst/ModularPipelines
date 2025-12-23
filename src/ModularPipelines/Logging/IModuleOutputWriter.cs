namespace ModularPipelines.Logging;

/// <summary>
/// Writes grouped/collapsible output for modules.
/// Automatically handles CI-specific formatting, buffering, and section headers with duration/status.
/// </summary>
/// <remarks>
/// This interface replaces <c>ICollapsableLogging</c> with a cleaner API.
/// Output is buffered during module execution and flushed as a grouped block when the module completes.
/// Section headers include module name, status (success/failure), and duration.
/// </remarks>
/// <example>
/// <code>
/// // Write to module output buffer
/// outputWriter.WriteLine("Building project...");
/// outputWriter.WriteLine("Build completed successfully");
///
/// // Create nested section within module output
/// using (outputWriter.BeginSection("Compile"))
/// {
///     outputWriter.WriteLine("Compiling source files...");
/// }
///
/// // Write directly to console (use sparingly)
/// outputWriter.WriteLineDirect("Critical: Starting long operation...");
/// </code>
/// </example>
public interface IModuleOutputWriter
{
    /// <summary>
    /// Writes a line to the module's output buffer.
    /// Content is flushed when the module completes, grouped under a collapsible section.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLine(string message);

    /// <summary>
    /// Writes a line directly to console, bypassing the buffer.
    /// Use sparingly - for critical messages during long-running operations.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLineDirect(string message);

    /// <summary>
    /// Creates a nested collapsible section within the module's output.
    /// </summary>
    /// <param name="name">The name of the section.</param>
    /// <returns>A disposable that ends the section when disposed.</returns>
    IDisposable BeginSection(string name);
}
