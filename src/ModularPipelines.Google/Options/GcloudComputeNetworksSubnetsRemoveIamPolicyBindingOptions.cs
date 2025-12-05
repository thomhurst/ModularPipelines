using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "remove-iam-policy-binding")]
public record GcloudComputeNetworksSubnetsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Subnetwork,
[property: CliArgument] string Region,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;