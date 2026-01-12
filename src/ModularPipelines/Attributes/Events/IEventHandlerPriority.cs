namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an event receiver attribute to specify its execution priority.
/// Lower priority values run first (priority 0 runs before priority 100).
/// </summary>
/// <remarks>
/// <para>
/// When multiple event receivers of the same type are applied to a module,
/// they are invoked in order of their priority. Receivers with lower priority
/// values execute before those with higher values.
/// </para>
/// <para>
/// Receivers that do not implement this interface default to priority 0.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LoggingEventHandler : Attribute, IModuleStartHandler, IEventHandlerPriority
/// {
///     public int Priority => 100;
///     public Task OnModuleStartAsync(IModuleHookContext context) => /* log */;
/// }
///
/// public class MetricsEventHandler : Attribute, IModuleStartHandler, IEventHandlerPriority
/// {
///     public int Priority => 200;
///     public Task OnModuleStartAsync(IModuleHookContext context) => /* record metrics */;
/// }
/// // LoggingEventHandler (100) runs before MetricsEventHandler (200)
/// </code>
/// </example>
public interface IEventHandlerPriority
{
    /// <summary>
    /// Gets the execution priority of this event handler.
    /// Lower values execute first. Default is 0.
    /// </summary>
    int Priority { get; }
}
