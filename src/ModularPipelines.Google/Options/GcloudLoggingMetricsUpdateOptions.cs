using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "metrics", "update")]
public record GcloudLoggingMetricsUpdateOptions(
[property: PositionalArgument] string MetricName,
[property: CommandSwitch("--config-from-file")] string ConfigFromFile,
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--log-filter")] string LogFilter
) : GcloudOptions;