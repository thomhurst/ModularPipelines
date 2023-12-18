using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "trunkednetwork", "create")]
public record AzNetworkcloudTrunkednetworkCreateOptions(
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--isolation-domain-ids")] string IsolationDomainIds,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vlans")] string Vlans
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