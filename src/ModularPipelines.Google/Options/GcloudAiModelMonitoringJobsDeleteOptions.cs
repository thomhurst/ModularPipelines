using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "model-monitoring-jobs", "delete")]
public record GcloudAiModelMonitoringJobsDeleteOptions(
[property: PositionalArgument] string MonitoringJob,
[property: PositionalArgument] string Region
) : GcloudOptions;