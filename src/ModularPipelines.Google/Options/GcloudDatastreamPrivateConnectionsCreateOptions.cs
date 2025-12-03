using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "private-connections", "create")]
public record GcloudDatastreamPrivateConnectionsCreateOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Location,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--subnet")] string Subnet,
[property: CliOption("--vpc")] string Vpc
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}