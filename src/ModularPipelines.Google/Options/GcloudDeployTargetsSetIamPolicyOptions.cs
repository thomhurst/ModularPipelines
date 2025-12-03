using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "set-iam-policy")]
public record GcloudDeployTargetsSetIamPolicyOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;