namespace ModularPipelines.Events;

/// <summary>
/// Marks a module registration handler as safe to invoke while planning a dependency graph.
/// </summary>
/// <remarks>
/// Implement this interface only when the handler is deterministic, idempotent, and free of
/// external side effects.
/// </remarks>
public interface IPlanningSafeModuleRegistrationHandler : IModuleRegistrationHandler;
