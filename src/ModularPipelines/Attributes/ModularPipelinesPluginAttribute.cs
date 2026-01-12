// src/ModularPipelines/Attributes/ModularPipelinesPluginAttribute.cs
namespace ModularPipelines.Attributes;

/// <summary>
/// Declares the ModularPipelines major version this plugin is compatible with.
/// Plugins are compatible with any minor/patch version of the declared major version.
/// </summary>
/// <example>
/// [assembly: ModularPipelinesPlugin(5)]  // Compatible with 5.0, 5.1, 5.2, etc.
/// </example>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class ModularPipelinesPluginAttribute : Attribute
{
    /// <summary>
    /// The major version of ModularPipelines this plugin is compatible with.
    /// </summary>
    public int CompatibleMajorVersion { get; }

    /// <summary>
    /// Creates a new plugin compatibility declaration.
    /// </summary>
    /// <param name="compatibleMajorVersion">The major version (e.g., 5 for 5.x compatibility).</param>
    public ModularPipelinesPluginAttribute(int compatibleMajorVersion)
    {
        if (compatibleMajorVersion < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(compatibleMajorVersion), "Major version must be non-negative.");
        }

        CompatibleMajorVersion = compatibleMajorVersion;
    }
}
