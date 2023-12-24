using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-client-vpn-target-network")]
public record AwsEc2AssociateClientVpnTargetNetworkOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}