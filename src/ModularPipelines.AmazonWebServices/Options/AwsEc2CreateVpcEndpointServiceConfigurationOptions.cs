using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpc-endpoint-service-configuration")]
public record AwsEc2CreateVpcEndpointServiceConfigurationOptions : AwsOptions
{
    [CommandSwitch("--private-dns-name")]
    public string? PrivateDnsName { get; set; }

    [CommandSwitch("--network-load-balancer-arns")]
    public string[]? NetworkLoadBalancerArns { get; set; }

    [CommandSwitch("--gateway-load-balancer-arns")]
    public string[]? GatewayLoadBalancerArns { get; set; }

    [CommandSwitch("--supported-ip-address-types")]
    public string[]? SupportedIpAddressTypes { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}