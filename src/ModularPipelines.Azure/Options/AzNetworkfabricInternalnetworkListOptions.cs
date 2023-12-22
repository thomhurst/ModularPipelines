using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "internalnetwork", "list")]
public record AzNetworkfabricInternalnetworkListOptions(
[property: CommandSwitch("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;