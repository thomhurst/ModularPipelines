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
/// <para><b>Thread Safety:</b></para>
/// <para>
/// <see cref="IModuleLogger"/> implementations are thread-safe. All inherited <see cref="ILogger"/>
/// methods (Log, IsEnabled, BeginScope) can be called concurrently from multiple threads without
/// external synchronization. This is essential for modules that perform parallel operations.
/// </para>
/// <para>
/// <b>Note:</b> The <see cref="IDisposable.Dispose"/> method should only be called once, typically
/// by the framework when the module completes. Calling Dispose concurrently or multiple times
/// may result in undefined behavior.
/// </para>
/// </remarks>
/// <seealso cref="IConsoleWriter"/>
public interface IModuleLogger : ILogger, IDisposable
{
}