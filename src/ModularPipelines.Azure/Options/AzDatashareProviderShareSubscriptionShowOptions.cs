using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "provider-share-subscription", "show")]
public record AzDatashareProviderShareSubscriptionShowOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--provider-share-subscription-id")]
    public string? ProviderShareSubscriptionId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}