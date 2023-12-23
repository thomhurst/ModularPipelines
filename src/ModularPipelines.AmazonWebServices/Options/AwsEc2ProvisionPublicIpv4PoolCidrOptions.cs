using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "provision-public-ipv4-pool-cidr")]
public record AwsEc2ProvisionPublicIpv4PoolCidrOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId,
[property: CommandSwitch("--pool-id")] string PoolId,
[property: CommandSwitch("--netmask-length")] int NetmaskLength
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}