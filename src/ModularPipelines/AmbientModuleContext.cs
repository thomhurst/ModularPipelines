using ModularPipelines.Logging;
using ModularPipelines.Tracing;

namespace ModularPipelines;

/// <summary>
/// Provides static access to the current module execution context.
/// </summary>
/// <remarks>
/// <para>
/// This class exposes ambient context about the currently executing module,
/// useful for external logging enrichers, telemetry providers, and diagnostic tools.
/// </para>
/// <para>
/// The context is set automatically before module construction and during module execution.
/// It uses AsyncLocal storage, so it flows correctly across async/await boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Serilog enricher example
/// public class ModuleEnricher : ILogEventEnricher
/// {
///     public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
///     {
///         var moduleName = AmbientModuleContext.CurrentModuleName ?? "Pipeline";
///         logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Module", moduleName));
///     }
/// }
/// </code>
/// </example>
public static class AmbientModuleContext
{
    /// <summary>
    /// Gets the type of the currently executing module, or null if not within a module context.
    /// </summary>
    /// <remarks>
    /// This property is set:
    /// <list type="bullet">
    /// <item>Before module construction (via dependency injection)</item>
    /// <item>During module execution (via the module runner)</item>
    /// </list>
    /// It returns null when code is executing outside of any module context,
    /// such as during pipeline initialization or in global hooks.
    /// </remarks>
    public static Type? CurrentModuleType => ModuleLogger.CurrentModuleType.Value;

    /// <summary>
    /// Gets the name of the currently executing module, or null if not within a module context.
    /// </summary>
    /// <remarks>
    /// This is a convenience property that returns <c>CurrentModuleType?.Name</c>.
    /// </remarks>
    public static string? CurrentModuleName => CurrentModuleType?.Name;

    /// <summary>
    /// Gets the full name (including namespace) of the currently executing module, or null if not within a module context.
    /// </summary>
    public static string? CurrentModuleFullName => CurrentModuleType?.FullName;

    /// <summary>
    /// Gets whether code is currently executing within a module context.
    /// </summary>
    public static bool IsInModuleContext => CurrentModuleType != null;

    /// <summary>
    /// Gets the current Activity ID if distributed tracing is enabled, or null otherwise.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property requires an ActivityListener to be registered (e.g., via OpenTelemetry).
    /// Without a listener, Activity objects are not created and this will return null.
    /// </para>
    /// <para>
    /// For module context without Activity dependency, use <see cref="CurrentModuleName"/> instead.
    /// </para>
    /// </remarks>
    public static string? CurrentActivityId => ModuleActivityTracing.GetCurrentActivityId();
}
