using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "port", "link", "update")]
public record AzNetworkExpressRoutePortLinkUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--port-name")] string PortName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--admin-state")]
    public string? AdminState { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--macsec-cak-secret-identifier")]
    public string? MacsecCakSecretIdentifier { get; set; }

    [CliOption("--macsec-cipher")]
    public string? MacsecCipher { get; set; }

    [CliOption("--macsec-ckn-secret-identifier")]
    public string? MacsecCknSecretIdentifier { get; set; }

    [CliOption("--macsec-sci-state")]
    public string? MacsecSciState { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}