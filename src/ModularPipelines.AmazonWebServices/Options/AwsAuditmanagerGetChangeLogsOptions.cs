using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "get-change-logs")]
public record AwsAuditmanagerGetChangeLogsOptions(
[property: CliOption("--assessment-id")] string AssessmentId
) : AwsOptions
{
    [CliOption("--control-set-id")]
    public string? ControlSetId { get; set; }

    [CliOption("--control-id")]
    public string? ControlId { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}