using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "provision-ipam-byoasn")]
public record AwsEc2ProvisionIpamByoasnOptions(
[property: CliOption("--ipam-id")] string IpamId,
[property: CliOption("--asn")] string Asn,
[property: CliOption("--asn-authorization-context")] string AsnAuthorizationContext
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}