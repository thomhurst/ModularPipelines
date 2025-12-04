using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "managed-private-endpoint", "list")]
public record AzDatafactoryManagedPrivateEndpointListOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;