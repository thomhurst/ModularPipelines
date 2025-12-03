using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "create")]
public record AzApimCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--publisher-email")] string PublisherEmail,
[property: CliOption("--publisher-name")] string PublisherName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [CliFlag("--enable-client-certificate")]
    public bool? EnableClientCertificate { get; set; }

    [CliFlag("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--public-network-access")]
    public bool? PublicNetworkAccess { get; set; }

    [CliOption("--sku-capacity")]
    public string? SkuCapacity { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network")]
    public string? VirtualNetwork { get; set; }
}