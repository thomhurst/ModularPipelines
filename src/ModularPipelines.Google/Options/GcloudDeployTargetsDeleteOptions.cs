using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "delete")]
public record GcloudDeployTargetsDeleteOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region
) : GcloudOptions;