using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "get-vpc-service-controls")]
public record GcloudServicesVpcPeeringsGetVpcServiceControlsOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }
}