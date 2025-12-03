using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-ipam-byoasn")]
public record AwsEc2DisassociateIpamByoasnOptions(
[property: CliOption("--asn")] string Asn,
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}