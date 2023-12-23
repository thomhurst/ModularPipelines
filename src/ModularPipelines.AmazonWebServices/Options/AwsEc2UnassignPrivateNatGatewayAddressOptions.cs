using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "unassign-private-nat-gateway-address")]
public record AwsEc2UnassignPrivateNatGatewayAddressOptions(
[property: CommandSwitch("--nat-gateway-id")] string NatGatewayId,
[property: CommandSwitch("--private-ip-addresses")] string[] PrivateIpAddresses
) : AwsOptions
{
    [CommandSwitch("--max-drain-duration-seconds")]
    public int? MaxDrainDurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}