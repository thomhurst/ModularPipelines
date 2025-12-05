using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "get-iam-policy")]
public record GcloudComputeResourcePoliciesGetIamPolicyOptions(
[property: CliArgument] string ResourcePolicy,
[property: CliArgument] string Region
) : GcloudOptions;