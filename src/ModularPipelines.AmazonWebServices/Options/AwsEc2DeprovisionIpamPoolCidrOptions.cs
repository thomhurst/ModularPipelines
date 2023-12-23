using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "deprovision-ipam-pool-cidr")]
public record AwsEc2DeprovisionIpamPoolCidrOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CommandSwitch("--cidr")]
    public string? Cidr { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}