using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "add-iam-policy-binding")]
public record GcloudComputeNetworksSubnetsAddIamPolicyBindingOptions(
[property: PositionalArgument] string Subnetwork,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;