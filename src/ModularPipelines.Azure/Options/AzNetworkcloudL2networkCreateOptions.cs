using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "l2network", "create")]
public record AzNetworkcloudL2networkCreateOptions(
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--l2-isolation-domain-id")] string L2IsolationDomainId,
[property: CommandSwitch("--l2-network-name")] string L2NetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--interface-name")]
    public string? InterfaceName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

