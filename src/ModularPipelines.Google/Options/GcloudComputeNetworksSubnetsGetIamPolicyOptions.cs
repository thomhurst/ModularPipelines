using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "get-iam-policy")]
public record GcloudComputeNetworksSubnetsGetIamPolicyOptions(
[property: CliArgument] string Subnetwork,
[property: CliArgument] string Region
) : GcloudOptions;