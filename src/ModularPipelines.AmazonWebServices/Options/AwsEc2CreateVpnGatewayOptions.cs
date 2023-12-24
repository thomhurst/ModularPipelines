using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpn-gateway")]
public record AwsEc2CreateVpnGatewayOptions(
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--amazon-side-asn")]
    public long? AmazonSideAsn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}