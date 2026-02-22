namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module requires an artifact produced by another module.
/// The framework automatically downloads the artifact before module execution.
/// In non-distributed mode, this attribute has no effect.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class ConsumesArtifactAttribute : Attribute
{
    /// <summary>
    /// Gets the type of the module that produces the artifact.
    /// </summary>
    public Type ProducerModule { get; }

    /// <summary>
    /// Gets the name of the artifact to consume.
    /// </summary>
    public string ArtifactName { get; }

    /// <summary>
    /// Gets or sets the local path where the artifact should be restored.
    /// If not set, the artifact is restored to the working directory.
    /// </summary>
    public string? RestorePath { get; set; }

    public ConsumesArtifactAttribute(Type producerModule, string artifactName)
    {
        ProducerModule = producerModule;
        ArtifactName = artifactName;
    }
}
