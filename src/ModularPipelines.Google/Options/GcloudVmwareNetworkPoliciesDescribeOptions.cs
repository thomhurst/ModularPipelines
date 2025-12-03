using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "describe")]
public record GcloudVmwareNetworkPoliciesDescribeOptions(
[property: CliArgument] string NetworkPolicy,
[property: CliArgument] string Location
) : GcloudOptions;