using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "set-iam-policy")]
public record GcloudBuildsConnectionsSetIamPolicyOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;