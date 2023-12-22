using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-analytics", "data-product", "list-roles-assignment")]
public record AzNetworkAnalyticsDataProductListRolesAssignmentOptions : AzOptions
{
    [CommandSwitch("--data-product-name")]
    public string? DataProductName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}