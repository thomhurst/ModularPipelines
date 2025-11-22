namespace ModularPipelines.Serialization;

/// <summary>
/// Provides a type discriminator for serialization purposes to identify the concrete type.
/// </summary>
public interface ITypeDiscriminator
{
    /// <summary>
    /// Gets the type discriminator value used to identify the concrete type during serialization.
    /// </summary>
    string TypeDiscriminator { get; }
}
