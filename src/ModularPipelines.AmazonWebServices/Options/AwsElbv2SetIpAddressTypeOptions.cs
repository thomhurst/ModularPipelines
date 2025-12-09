using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "set-ip-address-type")]
public record AwsElbv2SetIpAddressTypeOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn,
[property: CliOption("--ip-address-type")] string IpAddressType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}