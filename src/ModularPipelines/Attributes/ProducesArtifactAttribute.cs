namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module produces a file or directory artifact.
/// The framework automatically uploads matching artifacts in distributed mode and stores them
/// with fingerprint-based module cache entries when module caching is enabled.
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
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(pathPattern);

        Name = name;
        PathPattern = pathPattern;
    }
}
