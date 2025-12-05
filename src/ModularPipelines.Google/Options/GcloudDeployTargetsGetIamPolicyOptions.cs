using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "get-iam-policy")]
public record GcloudDeployTargetsGetIamPolicyOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region
) : GcloudOptions;