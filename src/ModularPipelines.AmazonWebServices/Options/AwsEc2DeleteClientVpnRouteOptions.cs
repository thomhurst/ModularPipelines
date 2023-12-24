using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-client-vpn-route")]
public record AwsEc2DeleteClientVpnRouteOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--destination-cidr-block")] string DestinationCidrBlock
) : AwsOptions
{
    [CommandSwitch("--target-vpc-subnet-id")]
    public string? TargetVpcSubnetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}