namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when an object's assembly was not processed by the source generator for secrets.
/// </summary>
public sealed class MissingSecretMetadataException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingSecretMetadataException"/> class.
    /// </summary>
    /// <param name="objectType">The object type whose assembly lacks generated metadata.</param>
    public MissingSecretMetadataException(Type objectType)
        : base(
            $"Secret metadata coverage is missing for assembly '{objectType.Assembly.GetName().Name}' " +
            $"while inspecting '{objectType.FullName}'. Ensure ModularPipelines.SourceGenerator is " +
            "referenced and make SecretValue-attributed types and properties accessible and non-generic.")
    {
        ObjectType = objectType;
    }

    /// <summary>
    /// Gets the object type whose assembly lacks generated metadata.
    /// </summary>
    public Type ObjectType { get; }
}
