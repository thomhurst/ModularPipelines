using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "externalnetwork", "list")]
public record AzNetworkfabricExternalnetworkListOptions(
[property: CliOption("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;