namespace ModularPipelines;

/// <summary>
/// Marks a dependency-selector attribute as safe to construct during dependency graph planning.
/// </summary>
/// <remarks>
/// Implement this interface only when construction is deterministic, idempotent, and free of
/// observable side effects. Built-in selectors are trusted without this marker.
/// </remarks>
public interface IPlanningSafeDependencySelector;
