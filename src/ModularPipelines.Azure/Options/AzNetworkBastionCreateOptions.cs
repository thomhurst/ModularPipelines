using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "bastion", "create")]
public record AzNetworkBastionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--public-ip-address")] string PublicIpAddress,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-name")] string VnetName
) : AzOptions
{
    [CliFlag("--disable-copy-paste")]
    public bool? DisableCopyPaste { get; set; }

    [CliFlag("--enable-ip-connect")]
    public bool? EnableIpConnect { get; set; }

    [CliFlag("--enable-tunneling")]
    public bool? EnableTunneling { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scale-units")]
    public string? ScaleUnits { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}