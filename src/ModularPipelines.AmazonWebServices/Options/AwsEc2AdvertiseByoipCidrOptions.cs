using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "advertise-byoip-cidr")]
public record AwsEc2AdvertiseByoipCidrOptions(
[property: CommandSwitch("--cidr")] string Cidr
) : AwsOptions
{
    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}