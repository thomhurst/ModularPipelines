using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "validate-assessment-report-integrity")]
public record AwsAuditmanagerValidateAssessmentReportIntegrityOptions(
[property: CliOption("--s3-relative-path")] string S3RelativePath
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}