using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "create-certificate-authority-audit-report")]
public record AwsAcmPcaCreateCertificateAuthorityAuditReportOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--s3-bucket-name")] string S3BucketName,
[property: CliOption("--audit-report-response-format")] string AuditReportResponseFormat
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}