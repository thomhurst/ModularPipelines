namespace ModularPipelines.Generated;

/// <summary>
/// Marks a type whose generated runtime metadata is incomplete.
/// </summary>
/// <remarks>
/// Public so generated assembly-level markers can share one type across project boundaries.
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class IncompleteRuntimeMetadataAttribute(string metadataName) : Attribute
{
    /// <summary>
    /// Gets the metadata name of the incomplete type.
    /// </summary>
    public string MetadataName { get; } = metadataName;
}
