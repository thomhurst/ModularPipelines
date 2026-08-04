namespace ModularPipelines.Attributes;

/// <summary>
/// Base class for dependency predicates that are safe to evaluate during graph planning.
/// </summary>
/// <remarks>
/// Planning-safe predicates must be deterministic and free of observable side effects.
/// Predicates derived directly from <see cref="DependsOnBaseAttribute"/> are deferred until runtime.
/// </remarks>
public abstract class PlanningSafeDependsOnBaseAttribute : DependsOnBaseAttribute;
