using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "authorize-client-vpn-ingress")]
public record AwsEc2AuthorizeClientVpnIngressOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--target-network-cidr")] string TargetNetworkCidr
) : AwsOptions
{
    [CommandSwitch("--access-group-id")]
    public string? AccessGroupId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}