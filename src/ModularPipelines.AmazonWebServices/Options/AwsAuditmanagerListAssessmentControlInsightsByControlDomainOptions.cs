using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "list-assessment-control-insights-by-control-domain")]
public record AwsAuditmanagerListAssessmentControlInsightsByControlDomainOptions(
[property: CliOption("--control-domain-id")] string ControlDomainId,
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}