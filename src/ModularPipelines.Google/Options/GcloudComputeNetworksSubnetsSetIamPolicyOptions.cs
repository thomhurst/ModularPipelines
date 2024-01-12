using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "set-iam-policy")]
public record GcloudComputeNetworksSubnetsSetIamPolicyOptions(
[property: PositionalArgument] string Subnetwork,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;