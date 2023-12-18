using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "bastion", "create")]
public record AzNetworkBastionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--public-ip-address")] string PublicIpAddress,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet-name")] string VnetName
) : AzOptions
{
    [BooleanCommandSwitch("--disable-copy-paste")]
    public bool? DisableCopyPaste { get; set; }

    [BooleanCommandSwitch("--enable-ip-connect")]
    public bool? EnableIpConnect { get; set; }

    [BooleanCommandSwitch("--enable-tunneling")]
    public bool? EnableTunneling { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scale-units")]
    public string? ScaleUnits { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}