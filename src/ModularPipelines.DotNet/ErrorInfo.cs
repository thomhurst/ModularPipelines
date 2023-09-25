using System.Xml.Serialization;

namespace ModularPipelines.DotNet;

[XmlRoot(ElementName = "ErrorInfo")]
public record ErrorInfo
{
    public string? Message { get; init; }

    public string? StackTrace { get; init; }
}