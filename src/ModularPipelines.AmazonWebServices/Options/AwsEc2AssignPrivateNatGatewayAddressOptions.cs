using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "assign-private-nat-gateway-address")]
public record AwsEc2AssignPrivateNatGatewayAddressOptions(
[property: CommandSwitch("--nat-gateway-id")] string NatGatewayId
) : AwsOptions
{
    [CommandSwitch("--private-ip-addresses")]
    public string[]? PrivateIpAddresses { get; set; }

    [CommandSwitch("--private-ip-address-count")]
    public int? PrivateIpAddressCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}