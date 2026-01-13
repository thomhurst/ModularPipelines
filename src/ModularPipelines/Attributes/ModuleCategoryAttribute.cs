namespace ModularPipelines.Attributes;

/// <summary>
/// Declares the category of a module. Categories identify what a module is.
/// Only one category can be applied per module.
/// </summary>
/// <remarks>
/// Categories are used with <see cref="DependsOnModulesInCategoryAttribute"/> to create
/// dependencies based on module identity like "infrastructure", "build", "test", etc.
/// </remarks>
/// <example>
/// <code>
/// [ModuleCategory("infrastructure")]
/// public class DatabaseMigrationModule : Module&lt;string&gt; { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ModuleCategoryAttribute : Attribute
{
    /// <summary>
    /// Gets the category value.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleCategoryAttribute"/> class.
    /// </summary>
    /// <param name="category">The category to apply to the module.</param>
    public ModuleCategoryAttribute(string category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        Category = category;
    }
}
