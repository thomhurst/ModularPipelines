using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "pause")]
public record GcloudAiModelMonitoringJobsPauseOptions(
[property: CliArgument] string MonitoringJob,
[property: CliArgument] string Region
) : GcloudOptions;