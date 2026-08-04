using System.ComponentModel;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.Metadata;

/// <summary>
/// Accepts AOT-safe runtime metadata emitted by cooperating source generators.
/// </summary>
/// <remarks>
/// A generator that emits an entire options type must also emit a module initializer that
/// registers static property accessors here. This avoids runtime type discovery and reflection,
/// which are unavailable after Native AOT trimming.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Advanced)]
public static class RuntimeMetadataRegistry
{
    /// <summary>
    /// Registers command metadata for a type emitted by another source generator.
    /// </summary>
    /// <param name="optionsType">The exact generated options type.</param>
    /// <param name="model">The generated command-property model.</param>
    public static void RegisterCommandOptions(
        Type optionsType,
        IReadOnlyList<PropertyCommandLinePart> model)
    {
        ArgumentNullException.ThrowIfNull(optionsType);
        ArgumentNullException.ThrowIfNull(model);
        GeneratedCommandMetadata.Register(optionsType, model);
    }

    /// <summary>
    /// Registers secret metadata for a type emitted by another source generator.
    /// </summary>
    /// <param name="objectType">The exact generated object type.</param>
    /// <param name="accessors">The generated secret-property accessors, or an empty list.</param>
    public static void RegisterSecrets(
        Type objectType,
        IReadOnlyList<SecretPropertyAccessor> accessors)
    {
        ArgumentNullException.ThrowIfNull(objectType);
        ArgumentNullException.ThrowIfNull(accessors);
        GeneratedSecretMetadata.Register(objectType, accessors);
    }
}
