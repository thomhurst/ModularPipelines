using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "provider-share-subscription", "adjust")]
public record AzDatashareProviderShareSubscriptionAdjustOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--provider-share-subscription-id")]
    public string? ProviderShareSubscriptionId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--share-name")]
    public string? ShareName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}