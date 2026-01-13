using ModularPipelines.Context.Domains.Data;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides serialization and encoding operations.
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// JSON serialization and deserialization.
    /// </summary>
    IJsonContext Json { get; }

    /// <summary>
    /// XML serialization and deserialization.
    /// </summary>
    IXmlContext Xml { get; }

    /// <summary>
    /// YAML serialization and deserialization.
    /// </summary>
    IYamlContext Yaml { get; }

    /// <summary>
    /// Base64 encoding and decoding.
    /// </summary>
    IBase64Context Base64 { get; }

    /// <summary>
    /// Hexadecimal encoding and decoding.
    /// </summary>
    IHexContext Hex { get; }
}
