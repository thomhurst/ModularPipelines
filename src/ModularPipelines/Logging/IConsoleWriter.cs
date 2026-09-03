using Microsoft.Extensions.Logging;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides module-aware plain text, Spectre.Console markup, and rich console output.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IConsoleWriter"/> when you need rich console formatting such as
/// colors, tables, progress bars, or other Spectre.Console features. Output from
/// this interface is captured in the current module's ordered output group and is not
/// delivered to logging providers.
/// </para>
/// <para>
/// For structured logging with log levels that flows to configured log sinks
/// (file, Application Insights, etc.), use <see cref="ILogger"/> instead.
/// </para>
/// <para><b>Example usage:</b></para>
/// <code>
/// // Rich console output with markup
/// consoleWriter.WriteLine("Build succeeded!");
/// consoleWriter.WriteMarkupLine("[red]Error:[/] Something went wrong");
/// </code>
/// </remarks>
/// <seealso cref="ILogger"/>
public interface IConsoleWriter
{
    /// <summary>
    /// Writes a plain-text line to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string value);

    /// <summary>
    /// Writes a line containing Spectre.Console markup to the console.
    /// </summary>
    /// <param name="value">The markup value to write.</param>
    void WriteMarkupLine(string value);

    /// <summary>
    /// Writes a Spectre.Console renderable to the console.
    /// </summary>
    /// <param name="renderable">The renderable object to write (Tree, Table, Panel, etc.).</param>
    void Write(IRenderable renderable);
}
