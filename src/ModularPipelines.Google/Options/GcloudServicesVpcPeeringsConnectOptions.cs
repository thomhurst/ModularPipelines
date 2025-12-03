using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "connect")]
public record GcloudServicesVpcPeeringsConnectOptions(
[property: CliOption("--network")] string Network,
[property: CliOption("--ranges")] string Ranges
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}