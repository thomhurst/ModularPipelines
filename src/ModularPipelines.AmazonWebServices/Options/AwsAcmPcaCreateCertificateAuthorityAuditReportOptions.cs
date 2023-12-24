using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "create-certificate-authority-audit-report")]
public record AwsAcmPcaCreateCertificateAuthorityAuditReportOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName,
[property: CommandSwitch("--audit-report-response-format")] string AuditReportResponseFormat
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}