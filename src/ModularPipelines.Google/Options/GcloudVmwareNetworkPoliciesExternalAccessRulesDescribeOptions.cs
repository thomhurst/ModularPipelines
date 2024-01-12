using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "external-access-rules", "describe")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesDescribeOptions(
[property: PositionalArgument] string ExternalAccessRule,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string NetworkPolicy
) : GcloudOptions;