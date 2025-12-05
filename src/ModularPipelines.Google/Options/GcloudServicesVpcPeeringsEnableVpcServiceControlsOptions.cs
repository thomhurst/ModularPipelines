using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "enable-vpc-service-controls")]
public record GcloudServicesVpcPeeringsEnableVpcServiceControlsOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}