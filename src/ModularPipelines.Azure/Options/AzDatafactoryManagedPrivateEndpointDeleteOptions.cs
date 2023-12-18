using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "managed-private-endpoint", "delete")]
public record AzDatafactoryManagedPrivateEndpointDeleteOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--managed-private-endpoint-name")]
    public string? ManagedPrivateEndpointName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}