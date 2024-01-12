using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "model-monitoring-jobs", "list")]
public record GcloudAiModelMonitoringJobsListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}