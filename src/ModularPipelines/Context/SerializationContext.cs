using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to serialization helpers for JSON, XML, and YAML.
/// </summary>
internal class SerializationContext : ISerializationContext
{
    /// <inheritdoc />
    public IJson Json { get; }

    /// <inheritdoc />
    public IXml Xml { get; }

    /// <inheritdoc />
    public IYaml Yaml { get; }

    public SerializationContext(IJson json, IXml xml, IYaml yaml)
    {
        Json = json;
        Xml = xml;
        Yaml = yaml;
    }
}
