using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides structured logging capabilities for modules, extending <see cref="ILogger"/>.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IModuleLogger"/> for structured logging that should flow to
/// configured logging providers (console, files, Application Insights, etc.).
/// Supports log levels (Debug, Information, Warning, Error) and structured data.
/// </para>
/// <para>
/// For rich console output with Spectre.Console markup (colors, tables, etc.),
/// use <see cref="IConsoleWriter"/> instead.
/// </para>
/// <para><b>Example usage:</b></para>
/// <code>
/// // Structured logging
/// logger.LogInformation("Processing {ItemCount} items", items.Count);
/// logger.LogError(ex, "Failed to process item {ItemId}", itemId);
/// </code>
/// </remarks>
/// <seealso cref="IConsoleWriter"/>
public interface IModuleLogger : ILogger, IDisposable
{
}