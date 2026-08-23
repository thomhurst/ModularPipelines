namespace ModularPipelines.Attributes;

/// <summary>
/// Base class for dependency predicates that are safe to evaluate during graph planning.
/// </summary>
/// <remarks>
/// Planning-safe predicates must be deterministic and free of observable side effects.
/// During graph planning, predicates can inspect tags, categories, and attribute presence.
/// Reading values from other custom attributes is unavailable because doing so can invoke
/// arbitrary attribute constructors. Such access fails graph export rather than omitting
/// a dependency that may be present at runtime.
/// Predicates derived directly from <see cref="DependsOnBaseAttribute"/> are deferred until runtime.
/// </remarks>
public abstract class PlanningSafeDependsOnBaseAttribute : DependsOnBaseAttribute;
