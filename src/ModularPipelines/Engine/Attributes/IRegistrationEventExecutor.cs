using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Executes registration events for module attributes.
/// </summary>
internal interface IRegistrationEventExecutor
{
    /// <summary>
    /// Invokes registration event handlers for all modules.
    /// This should be called before dependency resolution.
    /// </summary>
    Task InvokeRegistrationEventsAsync(IEnumerable<IModule> modules);
}
