using ModularPipelines.Context.Domains.Data;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to serialization and encoding capabilities.
/// </summary>
internal class DataContext : IDataContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </summary>
    /// <param name="json">The JSON context for JSON serialization.</param>
    /// <param name="xml">The XML context for XML serialization.</param>
    /// <param name="yaml">The YAML context for YAML serialization.</param>
    /// <param name="base64">The Base64 context for Base64 encoding.</param>
    /// <param name="hex">The Hex context for hexadecimal encoding.</param>
    public DataContext(
        IJsonContext json,
        IXmlContext xml,
        IYamlContext yaml,
        IBase64Context base64,
        IHexContext hex)
    {
        Json = json;
        Xml = xml;
        Yaml = yaml;
        Base64 = base64;
        Hex = hex;
    }

    /// <inheritdoc />
    public IJsonContext Json { get; }

    /// <inheritdoc />
    public IXmlContext Xml { get; }

    /// <inheritdoc />
    public IYamlContext Yaml { get; }

    /// <inheritdoc />
    public IBase64Context Base64 { get; }

    /// <inheritdoc />
    public IHexContext Hex { get; }
}
