using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

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
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
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
