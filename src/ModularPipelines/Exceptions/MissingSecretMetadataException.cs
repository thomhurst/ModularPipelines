namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when an object's exact runtime type was not processed by the source generator for secrets.
/// </summary>
public sealed class MissingSecretMetadataException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingSecretMetadataException"/> class.
    /// </summary>
    /// <param name="objectType">The object type that lacks generated metadata.</param>
    public MissingSecretMetadataException(Type objectType)
        : base(
            $"Secret metadata coverage is missing for type '{objectType.FullName}' in assembly " +
            $"'{objectType.Assembly.GetName().Name}'. Ensure ModularPipelines.SourceGenerator is " +
            "referenced and make SecretValue-attributed types and properties accessible and non-generic. " +
            "Generators that emit entire types must register static accessors through " +
            "ModularPipelines.Generated.RuntimeMetadataRegistry from a module initializer.")
    {
        ObjectType = objectType;
    }

    /// <summary>
    /// Gets the object type that lacks generated metadata.
    /// </summary>
    public Type ObjectType { get; }
}
