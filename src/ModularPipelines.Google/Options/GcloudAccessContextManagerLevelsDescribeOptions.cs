using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "describe")]
public record GcloudAccessContextManagerLevelsDescribeOptions(
[property: PositionalArgument] string Level,
[property: PositionalArgument] string Policy
) : GcloudOptions;