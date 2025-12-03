using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "update")]
public record AzApimUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [CliFlag("--enable-client-certificate")]
    public bool? EnableClientCertificate { get; set; }

    [CliFlag("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--public-network-access")]
    public bool? PublicNetworkAccess { get; set; }

    [CliOption("--publisher-email")]
    public string? PublisherEmail { get; set; }

    [CliOption("--publisher-name")]
    public string? PublisherName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku-capacity")]
    public string? SkuCapacity { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network")]
    public string? VirtualNetwork { get; set; }
}