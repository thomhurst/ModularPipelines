namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when command options do not have source-generated runtime metadata.
/// </summary>
public sealed class MissingCommandMetadataException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingCommandMetadataException"/> class.
    /// </summary>
    /// <param name="optionsType">The options type missing generated metadata.</param>
    public MissingCommandMetadataException(Type optionsType)
        : base(
            $"Command metadata is missing for '{optionsType.FullName}'. Ensure " +
            "ModularPipelines.SourceGenerator is referenced and make the options type, " +
            "its containing types, and CLI-attributed properties accessible and non-generic. " +
            "Generators that emit entire types must register static accessors through " +
            "ModularPipelines.Metadata.RuntimeMetadataRegistry from a module initializer.")
    {
        OptionsType = optionsType;
    }

    /// <summary>
    /// Gets the options type missing generated metadata.
    /// </summary>
    public Type OptionsType { get; }
}
