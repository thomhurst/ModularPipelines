using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "externalnetwork", "list")]
public record AzNetworkfabricExternalnetworkListOptions(
[property: CommandSwitch("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}