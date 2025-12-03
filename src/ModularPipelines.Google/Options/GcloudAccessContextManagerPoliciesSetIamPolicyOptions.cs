using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "set-iam-policy")]
public record GcloudAccessContextManagerPoliciesSetIamPolicyOptions(
[property: CliArgument] string Policy,
[property: CliArgument] string PolicyFile
) : GcloudOptions;