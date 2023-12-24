using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-nat-gateway-address")]
public record AwsEc2AssociateNatGatewayAddressOptions(
[property: CommandSwitch("--nat-gateway-id")] string NatGatewayId,
[property: CommandSwitch("--allocation-ids")] string[] AllocationIds
) : AwsOptions
{
    [CommandSwitch("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}