using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to serialization helpers for JSON, XML, and YAML.
/// </summary>
/// <remarks>
/// This context groups related serialization services to reduce constructor parameter count
/// in PipelineContext while maintaining the same public API.
/// </remarks>
public interface ISerializationContext
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
}
