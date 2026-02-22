namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module produces a file or directory artifact that should be shared in distributed mode.
/// The framework automatically uploads matching artifacts after module completion.
/// In non-distributed mode, this attribute has no effect.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class ProducesArtifactAttribute : Attribute
{
    /// <summary>
    /// Gets the name used to identify this artifact.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the glob pattern for matching files/directories to upload.
    /// </summary>
    public string PathPattern { get; }

    public ProducesArtifactAttribute(string name, string pathPattern)
    {
        Name = name;
        PathPattern = pathPattern;
    }
}
