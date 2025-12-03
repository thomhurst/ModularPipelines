using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "metrics", "create")]
public record GcloudLoggingMetricsCreateOptions(
[property: CliArgument] string MetricName,
[property: CliOption("--config-from-file")] string ConfigFromFile,
[property: CliOption("--description")] string Description,
[property: CliOption("--log-filter")] string LogFilter,
[property: CliOption("--bucket-name")] string BucketName
) : GcloudOptions;