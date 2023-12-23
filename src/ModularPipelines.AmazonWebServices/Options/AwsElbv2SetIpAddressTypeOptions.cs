using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "set-ip-address-type")]
public record AwsElbv2SetIpAddressTypeOptions(
[property: CommandSwitch("--load-balancer-arn")] string LoadBalancerArn,
[property: CommandSwitch("--ip-address-type")] string IpAddressType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}