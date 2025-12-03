using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-assessment")]
public record AwsAuditmanagerUpdateAssessmentOptions(
[property: CliOption("--assessment-id")] string AssessmentId,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--assessment-name")]
    public string? AssessmentName { get; set; }

    [CliOption("--assessment-description")]
    public string? AssessmentDescription { get; set; }

    [CliOption("--assessment-reports-destination")]
    public string? AssessmentReportsDestination { get; set; }

    [CliOption("--roles")]
    public string[]? Roles { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}