namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Marks a module registration receiver as safe to invoke during dependency graph planning.
/// </summary>
/// <remarks>
/// Planning occurs before pipeline startup and the receiver is invoked again during execution.
/// Implementations must therefore be deterministic, idempotent, and free of external side effects.
/// </remarks>
public interface IPlanningSafeModuleRegistrationEventReceiver : IModuleRegistrationEventReceiver;
