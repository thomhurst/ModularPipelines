using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "list-assessment-control-insights-by-control-domain")]
public record AwsAuditmanagerListAssessmentControlInsightsByControlDomainOptions(
[property: CommandSwitch("--control-domain-id")] string ControlDomainId,
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}