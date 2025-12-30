using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to serialization and encoding helpers.
/// </summary>
/// <remarks>
/// This interface groups JSON, XML, YAML serialization and encoding helpers (Hex, Base64, Hasher)
/// for better Interface Segregation.
/// </remarks>
public interface IPipelineEncoding
{
    /// <summary>
    /// Gets helpers for converting JSON.
    /// </summary>
    IJson Json { get; }

    /// <summary>
    /// Gets helpers for converting XML.
    /// </summary>
    IXml Xml { get; }

    /// <summary>
    /// Gets helpers for converting YAML.
    /// </summary>
    IYaml Yaml { get; }

    /// <summary>
    /// Gets helpers for converting to and from hex strings.
    /// </summary>
    IHex Hex { get; }

    /// <summary>
    /// Gets helpers for converting to and from Base64 strings.
    /// </summary>
    IBase64 Base64 { get; }

    /// <summary>
    /// Gets helpers for hashing data.
    /// </summary>
    /// <remarks>
    /// Supports MD5, SHA1, SHA256, SHA512 hashing algorithms.
    /// </remarks>
    IHasher Hasher { get; }
}
