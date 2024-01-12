using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "model-monitoring-jobs", "pause")]
public record GcloudAiModelMonitoringJobsPauseOptions(
[property: PositionalArgument] string MonitoringJob,
[property: PositionalArgument] string Region
) : GcloudOptions;