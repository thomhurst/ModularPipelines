using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "external-access-rules", "list")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesListOptions(
[property: CliOption("--network-policy")] string NetworkPolicy,
[property: CliOption("--location")] string Location
) : GcloudOptions;