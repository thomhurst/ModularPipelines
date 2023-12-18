using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "provider-share-subscription", "adjust")]
public record AzDatashareProviderShareSubscriptionAdjustOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions
{
    [CommandSwitch("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--provider-share-subscription-id")]
    public string? ProviderShareSubscriptionId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

