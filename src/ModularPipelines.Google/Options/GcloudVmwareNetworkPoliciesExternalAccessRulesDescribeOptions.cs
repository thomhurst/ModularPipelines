using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "external-access-rules", "describe")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesDescribeOptions(
[property: CliArgument] string ExternalAccessRule,
[property: CliArgument] string Location,
[property: CliArgument] string NetworkPolicy
) : GcloudOptions;