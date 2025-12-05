using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "import-client-vpn-client-certificate-revocation-list")]
public record AwsEc2ImportClientVpnClientCertificateRevocationListOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--certificate-revocation-list")] string CertificateRevocationList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}