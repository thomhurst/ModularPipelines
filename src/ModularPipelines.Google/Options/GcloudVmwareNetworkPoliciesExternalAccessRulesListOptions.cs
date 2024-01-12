using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "external-access-rules", "list")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesListOptions(
[property: CommandSwitch("--network-policy")] string NetworkPolicy,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;