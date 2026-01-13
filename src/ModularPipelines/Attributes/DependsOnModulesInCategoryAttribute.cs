using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Depend on all modules in the specified category.
/// </summary>
/// <example>
/// <code>
/// [DependsOnModulesInCategory("infrastructure")]
/// public class AfterInfrastructureModule : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class DependsOnModulesInCategoryAttribute : DependsOnBaseAttribute
{
    /// <summary>
    /// Gets the category to match.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnModulesInCategoryAttribute"/> class.
    /// </summary>
    /// <param name="category">The category that target modules must be in.</param>
    public DependsOnModulesInCategoryAttribute(string category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        Category = category;
    }

    /// <inheritdoc />
    public override bool ShouldDependOn(Type candidateModule, IDependencyContext context)
        => string.Equals(context.GetCategory(candidateModule), Category, StringComparison.OrdinalIgnoreCase);
}
