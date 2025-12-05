using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "reject-certificate-transfer")]
public record AwsIotRejectCertificateTransferOptions(
[property: CliOption("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CliOption("--reject-reason")]
    public string? RejectReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}