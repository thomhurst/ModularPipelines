using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "create-assessment-report")]
public record AwsAuditmanagerCreateAssessmentReportOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--query-statement")]
    public string? QueryStatement { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}