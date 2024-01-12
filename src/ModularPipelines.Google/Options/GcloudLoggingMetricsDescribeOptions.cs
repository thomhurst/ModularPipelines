using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "metrics", "describe")]
public record GcloudLoggingMetricsDescribeOptions(
[property: PositionalArgument] string MetricName
) : GcloudOptions;