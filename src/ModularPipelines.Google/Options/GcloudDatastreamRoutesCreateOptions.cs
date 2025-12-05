using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "routes", "create")]
public record GcloudDatastreamRoutesCreateOptions(
[property: CliArgument] string Route,
[property: CliArgument] string Location,
[property: CliArgument] string PrivateConnection,
[property: CliOption("--destination-address")] string DestinationAddress,
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliOption("--destination-port")]
    public string? DestinationPort { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}