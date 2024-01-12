using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "config", "controller", "describe")]
public record GcloudAnthosConfigControllerDescribeOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location
) : GcloudOptions;