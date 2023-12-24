using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "move-byoip-cidr-to-ipam")]
public record AwsEc2MoveByoipCidrToIpamOptions(
[property: CommandSwitch("--cidr")] string Cidr,
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId,
[property: CommandSwitch("--ipam-pool-owner")] string IpamPoolOwner
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}