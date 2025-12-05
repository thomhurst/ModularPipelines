using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-certificate")]
public record AwsIotUpdateCertificateOptions(
[property: CliOption("--certificate-id")] string CertificateId,
[property: CliOption("--new-status")] string NewStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}