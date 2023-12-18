using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "check-name")]
public record AzApimCheckNameOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [BooleanCommandSwitch("--enable-client-certificate")]
    public bool? EnableClientCertificate { get; set; }

    [BooleanCommandSwitch("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--public-network-access")]
    public bool? PublicNetworkAccess { get; set; }

    [CommandSwitch("--sku-capacity")]
    public string? SkuCapacity { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network")]
    public string? VirtualNetwork { get; set; }
}

