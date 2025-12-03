using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "metrics", "update")]
public record GcloudLoggingMetricsUpdateOptions(
[property: CliArgument] string MetricName,
[property: CliOption("--config-from-file")] string ConfigFromFile,
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--description")] string Description,
[property: CliOption("--log-filter")] string LogFilter
) : GcloudOptions;