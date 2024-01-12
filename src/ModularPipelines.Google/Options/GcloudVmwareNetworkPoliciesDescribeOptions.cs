using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "describe")]
public record GcloudVmwareNetworkPoliciesDescribeOptions(
[property: PositionalArgument] string NetworkPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions;