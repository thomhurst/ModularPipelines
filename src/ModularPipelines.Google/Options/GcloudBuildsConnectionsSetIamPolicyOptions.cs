using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "set-iam-policy")]
public record GcloudBuildsConnectionsSetIamPolicyOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;