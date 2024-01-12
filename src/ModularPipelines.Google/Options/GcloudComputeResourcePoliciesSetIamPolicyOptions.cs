using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "set-iam-policy")]
public record GcloudComputeResourcePoliciesSetIamPolicyOptions(
[property: PositionalArgument] string ResourcePolicy,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;