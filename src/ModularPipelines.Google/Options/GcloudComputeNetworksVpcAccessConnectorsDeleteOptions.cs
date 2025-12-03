using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "connectors", "delete")]
public record GcloudComputeNetworksVpcAccessConnectorsDeleteOptions(
[property: CliArgument] string Connector,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}