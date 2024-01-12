using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "get-iam-policy")]
public record GcloudComputeNetworksSubnetsGetIamPolicyOptions(
[property: PositionalArgument] string Subnetwork,
[property: PositionalArgument] string Region
) : GcloudOptions;