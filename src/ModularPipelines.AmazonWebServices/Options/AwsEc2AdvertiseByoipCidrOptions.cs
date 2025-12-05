using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "advertise-byoip-cidr")]
public record AwsEc2AdvertiseByoipCidrOptions(
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--network-border-group")]
    public string? NetworkBorderGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}