using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "metrics", "describe")]
public record GcloudLoggingMetricsDescribeOptions(
[property: CliArgument] string MetricName
) : GcloudOptions;