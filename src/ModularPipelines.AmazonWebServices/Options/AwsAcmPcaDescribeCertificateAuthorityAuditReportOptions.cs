using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "describe-certificate-authority-audit-report")]
public record AwsAcmPcaDescribeCertificateAuthorityAuditReportOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--audit-report-id")] string AuditReportId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}