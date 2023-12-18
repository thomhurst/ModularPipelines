using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "port", "link", "update")]
public record AzNetworkExpressRoutePortLinkUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--port-name")] string PortName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--admin-state")]
    public string? AdminState { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--macsec-cak-secret-identifier")]
    public string? MacsecCakSecretIdentifier { get; set; }

    [CommandSwitch("--macsec-cipher")]
    public string? MacsecCipher { get; set; }

    [CommandSwitch("--macsec-ckn-secret-identifier")]
    public string? MacsecCknSecretIdentifier { get; set; }

    [CommandSwitch("--macsec-sci-state")]
    public string? MacsecSciState { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}