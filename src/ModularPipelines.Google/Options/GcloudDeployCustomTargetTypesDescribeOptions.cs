using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "custom-target-types", "describe")]
public record GcloudDeployCustomTargetTypesDescribeOptions(
[property: PositionalArgument] string CustomTargetType,
[property: PositionalArgument] string Region
) : GcloudOptions;