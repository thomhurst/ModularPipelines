using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "start-bgp-failover-test")]
public record AwsDirectconnectStartBgpFailoverTestOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--bgp-peers")]
    public string[]? BgpPeers { get; set; }

    [CliOption("--test-duration-in-minutes")]
    public int? TestDurationInMinutes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}