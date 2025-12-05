using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "add-iam-policy-binding")]
public record GcloudComputeNetworksSubnetsAddIamPolicyBindingOptions(
[property: CliArgument] string Subnetwork,
[property: CliArgument] string Region,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;