using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "get-iam-policy")]
public record GcloudAccessContextManagerPoliciesGetIamPolicyOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions;