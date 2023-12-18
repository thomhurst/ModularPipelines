using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "managed-private-endpoint", "list")]
public record AzDatafactoryManagedPrivateEndpointListOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}