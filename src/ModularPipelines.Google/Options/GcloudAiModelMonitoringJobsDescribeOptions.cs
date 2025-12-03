using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "describe")]
public record GcloudAiModelMonitoringJobsDescribeOptions(
[property: CliArgument] string MonitoringJob,
[property: CliArgument] string Region
) : GcloudOptions;