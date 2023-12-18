using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "update")]
public record AzApimUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [BooleanCommandSwitch("--enable-client-certificate")]
    public bool? EnableClientCertificate { get; set; }

    [BooleanCommandSwitch("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--public-network-access")]
    public bool? PublicNetworkAccess { get; set; }

    [CommandSwitch("--publisher-email")]
    public string? PublisherEmail { get; set; }

    [CommandSwitch("--publisher-name")]
    public string? PublisherName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku-capacity")]
    public string? SkuCapacity { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network")]
    public string? VirtualNetwork { get; set; }
}