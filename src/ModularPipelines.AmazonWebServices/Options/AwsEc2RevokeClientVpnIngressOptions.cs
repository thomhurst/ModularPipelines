using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "revoke-client-vpn-ingress")]
public record AwsEc2RevokeClientVpnIngressOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--target-network-cidr")] string TargetNetworkCidr
) : AwsOptions
{
    [CommandSwitch("--access-group-id")]
    public string? AccessGroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}