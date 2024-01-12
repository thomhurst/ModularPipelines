using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "metrics", "delete")]
public record GcloudLoggingMetricsDeleteOptions(
[property: PositionalArgument] string MetricName
) : GcloudOptions;