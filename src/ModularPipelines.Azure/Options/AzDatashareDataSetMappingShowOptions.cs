using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "data-set-mapping", "show")]
public record AzDatashareDataSetMappingShowOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--data-set-mapping-name")]
    public string? DataSetMappingName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--share-subscription-name")]
    public string? ShareSubscriptionName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}