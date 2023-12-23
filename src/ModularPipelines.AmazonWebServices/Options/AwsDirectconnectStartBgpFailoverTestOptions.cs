using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "start-bgp-failover-test")]
public record AwsDirectconnectStartBgpFailoverTestOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--bgp-peers")]
    public string[]? BgpPeers { get; set; }

    [CommandSwitch("--test-duration-in-minutes")]
    public int? TestDurationInMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}