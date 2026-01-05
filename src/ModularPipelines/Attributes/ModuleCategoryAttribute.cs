namespace ModularPipelines.Attributes;

/// <summary>
/// Assigns a category to a module for grouping and filtering purposes.
/// </summary>
/// <remarks>
/// <para>
/// Categories are used to organize modules and can be filtered at runtime using
/// <see cref="Options.PipelineOptions.RunOnlyCategories"/> or <see cref="Options.PipelineOptions.IgnoreCategories"/>.
/// </para>
/// <para>
/// <strong>Note:</strong> Categories are currently used for module filtering only.
/// There is no category-level configuration API; configuration follows the standard precedence:
/// Global (PipelineOptions) -> Module (behavior interfaces) -> Per-Call.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [ModuleCategory("Build")]
/// public class BuildModule : Module&lt;string&gt;
/// {
///     // This module belongs to the "Build" category
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleCategoryAttribute : Attribute
{
    public string Category { get; }

    public ModuleCategoryAttribute(string category)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
    }
}