using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "resume")]
public record GcloudAiModelMonitoringJobsResumeOptions(
[property: CliArgument] string MonitoringJob,
[property: CliArgument] string Region
) : GcloudOptions;