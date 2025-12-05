using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "list")]
public record GcloudServicesVpcPeeringsListOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }
}