using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpn-connection-route")]
public record AwsEc2CreateVpnConnectionRouteOptions(
[property: CommandSwitch("--destination-cidr-block")] string DestinationCidrBlock,
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}