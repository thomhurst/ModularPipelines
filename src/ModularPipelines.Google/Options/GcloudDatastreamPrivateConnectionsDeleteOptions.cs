using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "private-connections", "delete")]
public record GcloudDatastreamPrivateConnectionsDeleteOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}