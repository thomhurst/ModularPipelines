using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "get-iam-policy")]
public record GcloudAccessContextManagerPoliciesGetIamPolicyOptions(
[property: CliArgument] string Policy
) : GcloudOptions;