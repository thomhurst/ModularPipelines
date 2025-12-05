using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "deprovision-ipam-byoasn")]
public record AwsEc2DeprovisionIpamByoasnOptions(
[property: CliOption("--ipam-id")] string IpamId,
[property: CliOption("--asn")] string Asn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}