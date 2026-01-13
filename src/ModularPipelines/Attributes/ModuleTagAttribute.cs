namespace ModularPipelines.Attributes;

/// <summary>
/// Declares a tag on a module. Tags describe characteristics of the module.
/// Multiple tags can be applied to a single module.
/// </summary>
/// <remarks>
/// Tags are used with <see cref="DependsOnModulesWithTagAttribute"/> to create
/// dependencies based on module characteristics like "slow", "critical", "database", etc.
/// </remarks>
/// <example>
/// <code>
/// [ModuleTag("database")]
/// [ModuleTag("slow")]
/// public class DatabaseMigrationModule : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class ModuleTagAttribute : Attribute
{
    /// <summary>
    /// Gets the tag value.
    /// </summary>
    public string Tag { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleTagAttribute"/> class.
    /// </summary>
    /// <param name="tag">The tag to apply to the module.</param>
    public ModuleTagAttribute(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        Tag = tag;
    }
}
