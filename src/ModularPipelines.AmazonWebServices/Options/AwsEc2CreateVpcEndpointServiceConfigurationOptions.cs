using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpc-endpoint-service-configuration")]
public record AwsEc2CreateVpcEndpointServiceConfigurationOptions : AwsOptions
{
    [CliOption("--private-dns-name")]
    public string? PrivateDnsName { get; set; }

    [CliOption("--network-load-balancer-arns")]
    public string[]? NetworkLoadBalancerArns { get; set; }

    [CliOption("--gateway-load-balancer-arns")]
    public string[]? GatewayLoadBalancerArns { get; set; }

    [CliOption("--supported-ip-address-types")]
    public string[]? SupportedIpAddressTypes { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}