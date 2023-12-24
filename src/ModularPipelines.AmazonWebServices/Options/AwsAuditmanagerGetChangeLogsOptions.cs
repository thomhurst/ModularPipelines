using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "get-change-logs")]
public record AwsAuditmanagerGetChangeLogsOptions(
[property: CommandSwitch("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CommandSwitch("--control-set-id")]
    public string? ControlSetId { get; set; }

    [CommandSwitch("--control-id")]
    public string? ControlId { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}