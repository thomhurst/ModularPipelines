using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "external-access-rules", "delete")]
public record GcloudVmwareNetworkPoliciesExternalAccessRulesDeleteOptions(
[property: CliArgument] string ExternalAccessRule,
[property: CliArgument] string Location,
[property: CliArgument] string NetworkPolicy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}