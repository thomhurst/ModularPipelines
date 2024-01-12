using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "describe")]
public record GcloudSpannerInstancesDescribeOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;