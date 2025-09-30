namespace ModularPipelines.Distributed.Attributes;

/// <summary>
/// Specifies that a module requires a worker with a specific tag.
/// The distributed scheduler will only assign this module to workers that have the tag in their Tags list.
/// </summary>
/// <example>
/// <code>
/// [RequiresTag("gpu-enabled")]
/// [RequiresTag("high-memory")]
/// public class MachineLearningModule : Module&lt;ModelResult&gt;
/// {
///     // This module will only run on workers tagged with both gpu-enabled and high-memory
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RequiresTagAttribute : ModuleRequirementAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiresTagAttribute"/> class.
    /// </summary>
    /// <param name="tag">The required worker tag (e.g., "gpu-enabled", "high-memory", "build-agent").</param>
    public RequiresTagAttribute(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        Tag = tag;
    }

    /// <summary>
    /// Gets the required tag.
    /// </summary>
    public string Tag { get; }
}
