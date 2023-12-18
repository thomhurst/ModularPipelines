using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "managed-private-endpoint", "create")]
public record AzDatafactoryManagedPrivateEndpointCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--managed-private-endpoint-name")] string ManagedPrivateEndpointName,
[property: CommandSwitch("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--fqdns")]
    public string? Fqdns { get; set; }

    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--private-link")]
    public string? PrivateLink { get; set; }
}