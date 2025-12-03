using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "list")]
public record GcloudAiModelMonitoringJobsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}