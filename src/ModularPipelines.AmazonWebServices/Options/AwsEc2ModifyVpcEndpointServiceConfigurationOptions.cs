using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-endpoint-service-configuration")]
public record AwsEc2ModifyVpcEndpointServiceConfigurationOptions(
[property: CliOption("--service-id")] string ServiceId
) : AwsOptions
{
    [CliOption("--private-dns-name")]
    public string? PrivateDnsName { get; set; }

    [CliOption("--add-network-load-balancer-arns")]
    public string[]? AddNetworkLoadBalancerArns { get; set; }

    [CliOption("--remove-network-load-balancer-arns")]
    public string[]? RemoveNetworkLoadBalancerArns { get; set; }

    [CliOption("--add-gateway-load-balancer-arns")]
    public string[]? AddGatewayLoadBalancerArns { get; set; }

    [CliOption("--remove-gateway-load-balancer-arns")]
    public string[]? RemoveGatewayLoadBalancerArns { get; set; }

    [CliOption("--add-supported-ip-address-types")]
    public string[]? AddSupportedIpAddressTypes { get; set; }

    [CliOption("--remove-supported-ip-address-types")]
    public string[]? RemoveSupportedIpAddressTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}