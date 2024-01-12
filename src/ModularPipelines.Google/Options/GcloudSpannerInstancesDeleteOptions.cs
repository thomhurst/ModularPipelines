using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "delete")]
public record GcloudSpannerInstancesDeleteOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;