using ModularPipelines.Logging;
using Spectre.Console.Rendering;

namespace ModularPipelines;

/// <summary>
/// Provides direct console output with Spectre.Console markup support.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IConsoleWriter"/> when you need rich console formatting such as
/// colors, tables, progress bars, or other Spectre.Console features. Output from
/// this interface goes directly to the console and is not captured by logging providers.
/// </para>
/// <para>
/// For structured logging with log levels that flows to configured log sinks
/// (file, Application Insights, etc.), use <see cref="IModuleLogger"/> instead.
/// </para>
/// <para><b>Example usage:</b></para>
/// <code>
/// // Rich console output with markup
/// consoleWriter.LogToConsole("[green]Build succeeded![/]");
/// consoleWriter.LogToConsole("[red]Error:[/] Something went wrong");
/// </code>
/// </remarks>
/// <seealso cref="IModuleLogger"/>
public interface IConsoleWriter
{
    /// <summary>
    /// Writes a value to the console.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void LogToConsole(string value);

    /// <summary>
    /// Writes a Spectre.Console renderable to the console.
    /// </summary>
    /// <param name="renderable">The renderable object to write (Tree, Table, Panel, etc.).</param>
    void Write(IRenderable renderable);
}