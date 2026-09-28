using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Base class for all predicate-based dependency declaration attributes.
/// Implement <see cref="ShouldDependOn"/> to define custom dependency selection logic.
/// </summary>
/// <remarks>
/// <para>
/// This base class enables flexible dependency declaration based on runtime metadata
/// such as tags, categories, or custom attributes rather than compile-time type references.
/// </para>
/// <para>
/// For compile-time type-safe dependencies, use <see cref="DependsOnAttribute{T}"/> instead.
/// </para>
/// <para>
/// Like <see cref="DependsOnAttribute"/>, selectors can be applied to a module class or to an interface the
/// module implements.
/// </para>
/// <para>
/// Implement <see cref="IPlanningSafe"/> on a selector that is deterministic and free of observable side
/// effects so dry-run planning and dependency-graph export can evaluate it. During planning, selectors can
/// inspect tags, categories, and attribute presence; reading values from other custom attributes fails graph
/// export, because doing so can invoke arbitrary attribute constructors. Other selectors are deferred until
/// runtime.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public abstract class DependsOnBaseAttribute : Attribute
{
    /// <summary>
    /// Determines whether the decorated module should depend on the candidate module.
    /// </summary>
    /// <param name="candidateModule">The module type being evaluated as a potential dependency.</param>
    /// <param name="context">Context providing access to module metadata (tags, categories, attributes).</param>
    /// <returns>True if the decorated module should depend on the candidate module.</returns>
    public abstract bool ShouldDependOn(Type candidateModule, IDependencyContext context);
}
