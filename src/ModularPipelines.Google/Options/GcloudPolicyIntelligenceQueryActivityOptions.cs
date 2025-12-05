using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy-intelligence", "query-activity")]
public record GcloudPolicyIntelligenceQueryActivityOptions : GcloudOptions
{
    public GcloudPolicyIntelligenceQueryActivityOptions(
        string activityType,
        string project
    )
    {
        ActivityType = activityType;
        Project = project;
    }

    [CliOption("--activity-type")]
    public string ActivityType { get; set; }

    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--query-filter")]
    public string? QueryFilter { get; set; }
}
