using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-endpoint-service-configuration")]
public record AwsEc2ModifyVpcEndpointServiceConfigurationOptions(
[property: CommandSwitch("--service-id")] string ServiceId
) : AwsOptions
{
    [CommandSwitch("--private-dns-name")]
    public string? PrivateDnsName { get; set; }

    [CommandSwitch("--add-network-load-balancer-arns")]
    public string[]? AddNetworkLoadBalancerArns { get; set; }

    [CommandSwitch("--remove-network-load-balancer-arns")]
    public string[]? RemoveNetworkLoadBalancerArns { get; set; }

    [CommandSwitch("--add-gateway-load-balancer-arns")]
    public string[]? AddGatewayLoadBalancerArns { get; set; }

    [CommandSwitch("--remove-gateway-load-balancer-arns")]
    public string[]? RemoveGatewayLoadBalancerArns { get; set; }

    [CommandSwitch("--add-supported-ip-address-types")]
    public string[]? AddSupportedIpAddressTypes { get; set; }

    [CommandSwitch("--remove-supported-ip-address-types")]
    public string[]? RemoveSupportedIpAddressTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}