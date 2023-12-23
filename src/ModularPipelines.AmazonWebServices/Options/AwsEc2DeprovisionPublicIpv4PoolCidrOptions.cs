using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "deprovision-public-ipv4-pool-cidr")]
public record AwsEc2DeprovisionPublicIpv4PoolCidrOptions(
[property: CommandSwitch("--pool-id")] string PoolId,
[property: CommandSwitch("--cidr")] string Cidr
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}