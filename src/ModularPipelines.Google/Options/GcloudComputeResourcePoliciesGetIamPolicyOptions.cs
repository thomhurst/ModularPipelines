using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "get-iam-policy")]
public record GcloudComputeResourcePoliciesGetIamPolicyOptions(
[property: PositionalArgument] string ResourcePolicy,
[property: PositionalArgument] string Region
) : GcloudOptions;