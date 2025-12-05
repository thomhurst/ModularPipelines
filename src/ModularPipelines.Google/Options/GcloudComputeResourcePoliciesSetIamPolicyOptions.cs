using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "set-iam-policy")]
public record GcloudComputeResourcePoliciesSetIamPolicyOptions(
[property: CliArgument] string ResourcePolicy,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;