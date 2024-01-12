using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy-intelligence", "query-activity")]
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

    [CommandSwitch("--activity-type")]
    public string ActivityType { get; set; }

    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--query-filter")]
    public string? QueryFilter { get; set; }
}
