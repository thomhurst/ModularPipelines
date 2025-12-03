using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-analytics", "data-product", "list-roles-assignment")]
public record AzNetworkAnalyticsDataProductListRolesAssignmentOptions : AzOptions
{
    [CliOption("--data-product-name")]
    public string? DataProductName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}