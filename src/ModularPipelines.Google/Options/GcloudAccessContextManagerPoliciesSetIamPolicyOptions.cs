using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "set-iam-policy")]
public record GcloudAccessContextManagerPoliciesSetIamPolicyOptions(
[property: PositionalArgument] string Policy,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;