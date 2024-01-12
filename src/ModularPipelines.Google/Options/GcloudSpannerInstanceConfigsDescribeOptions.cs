using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instance-configs", "describe")]
public record GcloudSpannerInstanceConfigsDescribeOptions(
[property: PositionalArgument] string InstanceConfig
) : GcloudOptions;