using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "validate-assessment-report-integrity")]
public record AwsAuditmanagerValidateAssessmentReportIntegrityOptions(
[property: CommandSwitch("--s3-relative-path")] string S3RelativePath
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}