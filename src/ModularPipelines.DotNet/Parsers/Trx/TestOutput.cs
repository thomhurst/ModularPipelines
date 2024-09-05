using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace ModularPipelines.DotNet.Parsers.Trx;

[XmlRoot(ElementName = "Output")]
[ExcludeFromCodeCoverage]
public record TestOutput
{
    [XmlElement("StdOut")]
    public string? StdOut { get; init; }

    [XmlElement("ErrorInfo")]
    public ErrorInfo? ErrorInfo { get; init; }
}