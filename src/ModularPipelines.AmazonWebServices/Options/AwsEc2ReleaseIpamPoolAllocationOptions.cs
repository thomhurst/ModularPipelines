using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "release-ipam-pool-allocation")]
public record AwsEc2ReleaseIpamPoolAllocationOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId,
[property: CommandSwitch("--cidr")] string Cidr,
[property: CommandSwitch("--ipam-pool-allocation-id")] string IpamPoolAllocationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}