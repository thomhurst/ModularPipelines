using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pccp", "create")]
public record AzMobileNetworkPccpCreateOptions(
[property: CommandSwitch("--access-interface")] string AccessInterface,
[property: CommandSwitch("--local-diagnostics")] string LocalDiagnostics,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sites")] string Sites,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--core-network-tec")]
    public string? CoreNetworkTec { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--ue-mtu")]
    public string? UeMtu { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}