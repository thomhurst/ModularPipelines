using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "metrics", "create")]
public record GcloudLoggingMetricsCreateOptions(
[property: PositionalArgument] string MetricName,
[property: CommandSwitch("--config-from-file")] string ConfigFromFile,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--log-filter")] string LogFilter,
[property: CommandSwitch("--bucket-name")] string BucketName
) : GcloudOptions;