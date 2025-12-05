using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instance-configs", "describe")]
public record GcloudSpannerInstanceConfigsDescribeOptions(
[property: CliArgument] string InstanceConfig
) : GcloudOptions;