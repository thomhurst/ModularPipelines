using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Depend on all modules that have the specified tag.
/// </summary>
/// <example>
/// <code>
/// [DependsOnModulesWithTag("database")]
/// public class AfterDatabaseModule : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class DependsOnModulesWithTagAttribute : DependsOnBaseAttribute
{
    /// <summary>
    /// Gets the tag to match.
    /// </summary>
    public string Tag { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependsOnModulesWithTagAttribute"/> class.
    /// </summary>
    /// <param name="tag">The tag that target modules must have.</param>
    public DependsOnModulesWithTagAttribute(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        Tag = tag;
    }

    /// <inheritdoc />
    public override bool ShouldDependOn(Type candidateModule, IDependencyContext context)
        => context.GetTags(candidateModule).Contains(Tag);
}
