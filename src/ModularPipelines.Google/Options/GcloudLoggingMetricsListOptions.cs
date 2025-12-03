using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "metrics", "list")]
public record GcloudLoggingMetricsListOptions : GcloudOptions;