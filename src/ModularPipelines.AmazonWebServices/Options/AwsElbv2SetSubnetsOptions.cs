using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "set-subnets")]
public record AwsElbv2SetSubnetsOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn
) : AwsOptions
{
    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--subnet-mappings")]
    public string[]? SubnetMappings { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}