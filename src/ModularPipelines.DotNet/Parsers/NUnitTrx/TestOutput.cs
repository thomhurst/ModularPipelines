using System.Xml.Serialization;

namespace ModularPipelines.DotNet.Parsers.NUnitTrx;

[XmlRoot(ElementName = "Output")]
public record TestOutput
{
    [XmlElement("StdOut")]
    public string? StdOut { get; init; }

    [XmlElement("ErrorInfo")]
    public ErrorInfo? ErrorInfo { get; init; }
}