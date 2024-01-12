using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "external-access-rules", "delete")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesDeleteOptions(
[property: PositionalArgument] string ExternalAccessRule,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string NetworkPolicy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}