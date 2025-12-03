using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "set-iam-policy")]
public record GcloudComputeNetworksSubnetsSetIamPolicyOptions(
[property: CliArgument] string Subnetwork,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;